using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Quras.Core;
using Quras.IO;
using Quras.IO.Json;
using Quras.SmartContract;
using Quras.VM;
using Quras.Wallets;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Collections.Generic;

namespace Quras.Network.RPC
{
    public class RpcServer : IDisposable
    {
        protected readonly LocalNode LocalNode;
        private IWebHost host;

        private Boolean IsRPC = false;

        private string log_dictionary = Path.Combine(AppContext.BaseDirectory, "Logs");

        public RpcServer(LocalNode localNode, bool isRPC)
        {
            this.LocalNode = localNode;
            IsRPC = isRPC;
        }

        private static JObject CreateErrorResponse(JObject id, int code, string message, JObject data = null)
        {
            JObject response = CreateResponse(id);
            response["error"] = new JObject();
            response["error"]["code"] = code;
            response["error"]["message"] = message;
            if (data != null)
                response["error"]["data"] = data;
            return response;
        }

        private static JObject CreateResponse(JObject id)
        {
            JObject response = new JObject();
            response["jsonrpc"] = "2.0";
            response["id"] = id;
            return response;
        }

        public void Dispose()
        {
            if (host != null)
            {
                host.Dispose();
                host = null;
            }
        }

        private static JObject GetInvokeResult(byte[] script)
        {
            ApplicationEngine engine = ApplicationEngine.Run(script);
            JObject json = new JObject();
            json["state"] = engine.State;
            json["gas_consumed"] = engine.GasConsumed.ToString();
            json["stack"] = new JArray(engine.EvaluationStack.Select(p => p.ToParameter().ToJson()));
            return json;
        }

        private static JArray GetMempoolTransaction(UInt160 ScriptHash)
        {
            JArray result = new JArray();
            foreach(Transaction tx in LocalNode.GetMemoryPool())
            {
                UInt160 from_script = null;
                UInt160 to_script = null;
                JObject obj = new JObject();

                if (tx is ContractTransaction)
                {
                    try
                    {
                        foreach (CoinReference input in tx.Inputs)
                        {
                            Transaction inputTx = Blockchain.Default.GetTransaction(input.PrevHash);
                            if (inputTx != null)
                            {
                                from_script = inputTx.Outputs[input.PrevIndex].ScriptHash;
                            }
                            else
                            {
                                foreach(Transaction inTx in LocalNode.GetMemoryPool())
                                {
                                    if (inTx.Hash.Equals(input.PrevHash))
                                        from_script = inTx.Outputs[input.PrevIndex].ScriptHash;
                                }
                            }
                        }
                    }
                    catch (Exception ex) { }

                    if (tx.Outputs.Length > 0)
                        to_script = tx.Outputs[0].ScriptHash;

                    Dictionary<UInt256, Fixed8> balance = new Dictionary<UInt256, Fixed8>();
                    Dictionary<UInt256, Fixed8> fee = new Dictionary<UInt256, Fixed8>();
                    for (int i = 0; i < tx.Outputs.Length; i++)
                    {
                        AssetState state = Blockchain.Default.GetAssetState(tx.Outputs[i].AssetId);
                        if (tx.Outputs[i].ScriptHash.Equals(to_script))
                        {
                            if (balance.ContainsKey(tx.Outputs[i].AssetId))
                            {
                                balance[tx.Outputs[i].AssetId] += tx.Outputs[i].Value;
                            }
                            else
                            {
                                balance[tx.Outputs[i].AssetId] = tx.Outputs[i].Value;
                            }
                        }

                        if (!fee.ContainsKey(tx.Outputs[i].AssetId))
                        {
                            if (tx.Outputs[i].Fee != Fixed8.Zero)
                            {
                                fee[tx.Outputs[i].AssetId] = tx.Outputs[i].Fee;
                            }
                        }
                    }
                    Fixed8 qrgFee = fee.Sum(p => p.Value);
                    if (balance.Count > 0)
                    {
                        UInt256 assetId = balance.First().Key;
                        Fixed8 value = balance.Sum(p => p.Value);

                        AssetState assetState = Blockchain.Default.GetAssetState(assetId);
                        obj["asset"] = assetState.GetName();
                        obj["amount"] = value.ToString();
                        obj["fee"] = qrgFee.ToString();
                    }
                }
                else if (tx is ClaimTransaction)
                {
                    ClaimTransaction claimTx = (ClaimTransaction)tx;
                    if (claimTx.Outputs.Length > 0)
                        from_script = to_script = tx.Outputs[0].ScriptHash;

                    Dictionary<UInt256, Fixed8> balance = new Dictionary<UInt256, Fixed8>();
                    Dictionary<UInt256, Fixed8> fee = new Dictionary<UInt256, Fixed8>();

                    for (int i = 0; i < tx.Outputs.Length; i++)
                    {
                        AssetState state = Blockchain.Default.GetAssetState(tx.Outputs[i].AssetId);
                        if (tx.Outputs[i].ScriptHash.Equals(to_script))
                        {
                            if (balance.ContainsKey(tx.Outputs[i].AssetId))
                            {
                                balance[tx.Outputs[i].AssetId] += tx.Outputs[i].Value;
                            }
                            else
                            {
                                balance[tx.Outputs[i].AssetId] = tx.Outputs[i].Value;
                            }
                        }

                        if (!fee.ContainsKey(tx.Outputs[i].AssetId))
                        {
                            if (tx.Outputs[i].Fee != Fixed8.Zero)
                            {
                                fee[tx.Outputs[i].AssetId] = tx.Outputs[i].Fee;
                            }
                        }
                    }
                    obj["asset"] = Blockchain.UtilityToken.Name;
                    obj["amount"] = balance[Blockchain.UtilityToken.Hash].value.ToString();
                    obj["fee"] = Fixed8.Zero.ToString();
                }
                else if (tx is InvocationTransaction)
                {
                    InvocationTransaction invocTx = (InvocationTransaction)tx;
                    try
                    {
                        foreach (CoinReference input in tx.Inputs)
                        {
                            from_script = tx.References[input].ScriptHash;
                        }

                    }
                    catch (Exception ex) { }
                    obj["asset"] = Blockchain.UtilityToken.Name;
                    obj["amount"] = invocTx.Gas.ToString();
                    obj["fee"] = Fixed8.Zero.ToString();
                }
                else if (tx is IssueTransaction)
                {
                    AssetState state = Blockchain.Default.GetAssetState(Blockchain.UtilityToken.Hash);
                    IssueTransaction issueTx = (IssueTransaction)tx;

                    try
                    {
                        foreach (CoinReference input in tx.Inputs)
                        {
                            from_script = tx.References[input].ScriptHash;
                        }

                    }
                    catch (Exception ex) { }

                    if (tx.Outputs.Length > 0)
                        to_script = tx.Outputs[0].ScriptHash;


                    if (tx.Outputs.Length > 0)
                    {
                        state = Blockchain.Default.GetAssetState(tx.Outputs[0].AssetId);
                    }

                    Fixed8 totalAmount = Fixed8.Zero;
                    foreach (TransactionOutput output in tx.Outputs)
                    {
                        if (output.AssetId != Blockchain.UtilityToken.Hash)
                        {
                            totalAmount += output.Value;
                        }
                    }

                    Fixed8 fee = issueTx.NetworkFee + issueTx.SystemFee;

                    obj["asset"] = state.GetName();
                    obj["amount"] = totalAmount.ToString();
                    obj["fee"] = fee.ToString();
                }

                if (!ScriptHash.Equals(from_script) && !ScriptHash.Equals(to_script))
                    continue;
               
                if (from_script != null)
                    obj["from"] = Wallet.ToAddress(from_script);
                else
                    obj["from"] = "";

                if (to_script != null)
                    obj["to"] = Wallet.ToAddress(to_script);
                else
                    obj["to"] = "";

                obj["txType"] = tx.Type;
                obj["blockHeight"] = Fixed8.Zero.ToString();
                obj["txid"] = tx.Hash.ToString();
                obj["timestamp"] = DateTime.Now.ToTimestamp();
                obj["status"] = 0;
                result.Add(obj);
            }
            return result;
        }

        private static JArray GetMempoolCoin(IEnumerable<Transaction> Transactions, UInt160 ScriptHash)
        {
            JArray result = new JArray();

            foreach (Transaction tx in Transactions)
            {
                // Add output to Result
                int i = 0;
                foreach (TransactionOutput output in tx.Outputs)
                {
                    i++;
                    if (!output.ScriptHash.Equals(ScriptHash)) continue;

                    JObject newOutput = new JObject();
                    newOutput["index"] = i - 1;
                    newOutput["txid"] = tx.Hash.ToString();
                    newOutput["value"] = output.Value.ToString();
                    newOutput["fee"] = output.Fee.ToString();

                    bool is_added = false;
                    foreach (JObject obj in result)
                    {
                        if (UInt256.Parse(obj["AssetId"].AsString()).Equals(output.AssetId))
                        {
                            is_added = true;
                            ((JArray)obj["unconfirmed"]).Add(newOutput);
                            break;
                        }
                    }
                    if (is_added == false)
                    {
                        JObject newCoin = new JObject();
                        newCoin["AssetId"] = output.AssetId.ToString();
                        newCoin["spent"] = new JArray();
                        newCoin["unconfirmed"] = new JArray();
                        ((JArray)newCoin["unconfirmed"]).Add(newOutput);
                        result.Add(newCoin);
                    }
                }
            }
            foreach (Transaction tx in Transactions)
            {
                // Add input to Result

                foreach (CoinReference input in tx.Inputs)
                {
                    Transaction prevTx = Blockchain.Default.GetTransaction(input.PrevHash);
                    UInt256 AssetId = null;
                    if (prevTx != null && prevTx.Outputs.Length > input.PrevIndex && input.PrevIndex >= 0)
                    {
                        AssetId = prevTx.Outputs[input.PrevIndex].AssetId;
                    }
                    else
                    {
                        foreach (JObject obj in result)
                        {
                            for (int j = 0; j < ((JArray)obj["unconfirmed"]).Count; j ++)
                            {
                                JObject addedTx = ((JArray)obj["unconfirmed"])[j];
                                if (addedTx["txid"].AsString().Equals(input.PrevHash.ToString()))
                                {
                                    AssetId = UInt256.Parse(obj["AssetId"].AsString());
                                    ((JArray)obj["unconfirmed"]).RemoveAt(j);
                                    break;
                                }
                            }
                        }

                        if (AssetId != null)
                            continue;
                    }

                    if (AssetId == null)
                    {
                        Console.WriteLine("AssetId is null: " + input.PrevHash);
                        continue;
                    }

                    JObject newInput = new JObject();
                    newInput["PrevHash"] = input.PrevHash.ToString();
                    newInput["PrevIndex"] = input.PrevIndex;
                    bool is_added = false;
                    foreach (JObject obj in result)
                    {
                        if (UInt256.Parse(obj["AssetId"].AsString()).Equals(AssetId))
                        {
                            is_added = true;
                            ((JArray)obj["spent"]).Add(newInput);
                            break;
                        }
                    }
                    if (is_added == false)
                    {
                        JObject newCoin = new JObject();
                        newCoin["AssetId"] = AssetId.ToString();
                        newCoin["spent"] = new JArray();
                        newCoin["unconfirmed"] = new JArray();
                        ((JArray)newCoin["spent"]).Add(newInput);
                        result.Add(newCoin);
                    }
                }
            }

            return result;
        }

        protected virtual JObject Process(string method, JArray _params)
        {
            if (IsRPC == false && !method.Equals("getblockcount"))
            {
                throw new RpcException(-32601, "Method not found");
            }

            switch (method)
            {
                case "getaccountstate":
                    {
                        UInt160 script_hash = Wallet.ToScriptHash(_params[0].AsString());
                        AccountState account = Blockchain.Default.GetAccountState(script_hash) ?? new AccountState(script_hash);
                        return account.ToJson();
                    }
                case "getassetstate":
                    {
                        UInt256 asset_id = UInt256.Parse(_params[0].AsString());
                        AssetState asset = Blockchain.Default.GetAssetState(asset_id);
                        return asset?.ToJson() ?? throw new RpcException(-100, "Unknown asset");
                    }
                case "getbestblockhash":
                    return Blockchain.Default.CurrentBlockHash.ToString();
                case "getblock":
                    {
                        Block block;
                        if (_params[0] is JNumber)
                        {
                            uint index = (uint)_params[0].AsNumber();
                            block = Blockchain.Default.GetBlock(index);
                        }
                        else
                        {
                            UInt256 hash = UInt256.Parse(_params[0].AsString());
                            block = Blockchain.Default.GetBlock(hash);
                        }
                        if (block == null)
                            throw new RpcException(-100, "Unknown block");
                        bool verbose = _params.Count >= 2 && _params[1].AsBooleanOrDefault(false);
                        if (verbose)
                        {
                            JObject json = block.ToJson();
                            json["confirmations"] = Blockchain.Default.Height - block.Index + 1;
                            UInt256 hash = Blockchain.Default.GetNextBlockHash(block.Hash);
                            if (hash != null)
                                json["nextblockhash"] = hash.ToString();
                            return json;
                        }
                        else
                        {
                            return block.ToArray().ToHexString();
                        }
                    }
                case "getblockcount":
                    return Blockchain.Default.Height + 1;
                case "getblockhash":
                    {
                        uint height = (uint)_params[0].AsNumber();
                        if (height >= 0 && height <= Blockchain.Default.Height)
                        {
                            return Blockchain.Default.GetBlockHash(height).ToString();
                        }
                        else
                        {
                            throw new RpcException(-100, "Invalid Height");
                        }
                    }
                case "getblocksysfee":
                    {
                        uint height = (uint)_params[0].AsNumber();
                        if (height >= 0 && height <= Blockchain.Default.Height)
                        {
                            return Blockchain.Default.GetSysFeeAmount(height).ToString();
                        }
                        else
                        {
                            throw new RpcException(-100, "Invalid Height");
                        }
                    }
                case "getconnectioncount":
                    return LocalNode.RemoteNodeCount;
                case "getcontractstate":
                    {
                        UInt160 script_hash = UInt160.Parse(_params[0].AsString());
                        ContractState contract = Blockchain.Default.GetContract(script_hash);
                        return contract?.ToJson() ?? throw new RpcException(-100, "Unknown contract");
                    }
                case "getrawmempool":
                    return new JArray(LocalNode.GetMemoryPool().Select(p => (JObject)p.Hash.ToString()));
                case "getmempoolcoin":
                    {
                        UInt160 script_hash = UInt160.Parse(_params[0].AsString());
                        return GetMempoolCoin(LocalNode.GetMemoryPool(), script_hash);
                    }
                case "getmempooltransaction":
                    {
                        UInt160 script_hash = UInt160.Parse(_params[0].AsString());
                        return GetMempoolTransaction(script_hash);
                    }
                    
                case "getrawtransaction":
                    {
                        UInt256 hash = UInt256.Parse(_params[0].AsString());
                        bool verbose = _params.Count >= 2 && _params[1].AsBooleanOrDefault(false);
                        int height = -1;
                        Transaction tx = LocalNode.GetTransaction(hash);
                        if (tx == null)
                            tx = Blockchain.Default.GetTransaction(hash, out height);
                        if (tx == null)
                            throw new RpcException(-100, "Unknown transaction");
                        if (verbose)
                        {
                            JObject json = tx.ToJson();
                            if (height >= 0)
                            {
                                Header header = Blockchain.Default.GetHeader((uint)height);
                                json["blockhash"] = header.Hash.ToString();
                                json["confirmations"] = Blockchain.Default.Height - header.Index + 1;
                                json["blocktime"] = header.Timestamp;
                            }
                            return json;
                        }
                        else
                        {
                            return tx.ToArray().ToHexString();
                        }
                    }
                case "getstorage":
                    {
                        UInt160 script_hash = UInt160.Parse(_params[0].AsString());
                        byte[] key = _params[1].AsString().HexToBytes();
                        StorageItem item = Blockchain.Default.GetStorageItem(new StorageKey
                        {
                            ScriptHash = script_hash,
                            Key = key
                        }) ?? new StorageItem();
                        return item.Value?.ToHexString();
                    }
                case "gettxout":
                    {
                        UInt256 hash = UInt256.Parse(_params[0].AsString());
                        ushort index = (ushort)_params[1].AsNumber();
                        return Blockchain.Default.GetUnspent(hash, index)?.ToJson(index);
                    }
                case "invoke":
                    {
                        UInt160 script_hash = UInt160.Parse(_params[0].AsString());
                        ContractParameter[] parameters = ((JArray)_params[1]).Select(p => ContractParameter.FromJson(p)).ToArray();
                        byte[] script;
                        using (ScriptBuilder sb = new ScriptBuilder())
                        {
                            script = sb.EmitAppCall(script_hash, parameters).ToArray();
                        }
                        return GetInvokeResult(script);
                    }
                case "invokefunction":
                    {
                        UInt160 script_hash = UInt160.Parse(_params[0].AsString());
                        string operation = _params[1].AsString();
                        ContractParameter[] args = _params.Count >= 3 ? ((JArray)_params[2]).Select(p => ContractParameter.FromJson(p)).ToArray() : new ContractParameter[0];
                        byte[] script;
                        using (ScriptBuilder sb = new ScriptBuilder())
                        {
                            script = sb.EmitAppCall(script_hash, operation, args).ToArray();
                        }
                        return GetInvokeResult(script);
                    }
                case "invokescript":
                    {
                        byte[] script = _params[0].AsString().HexToBytes();
                        return GetInvokeResult(script);
                    }
                case "sendrawtransaction":
                    {
                        Transaction tx = Transaction.DeserializeFrom(_params[0].AsString().HexToBytes());
                        tx.is_consensus_mempool = true;
                        return LocalNode.Relay(tx);
                    }
                case "submitblock":
                    {
                        Block block = _params[0].AsString().HexToBytes().AsSerializable<Block>();
                        return LocalNode.Relay(block);
                    }
                case "validateaddress":
                    {
                        JObject json = new JObject();
                        UInt160 scriptHash;
                        try
                        {
                            scriptHash = Wallet.ToScriptHash(_params[0].AsString());
                        }
                        catch
                        {
                            scriptHash = null;
                        }
                        json["address"] = _params[0];
                        json["isvalid"] = scriptHash != null;
                        return json;
                    }
                case "getpeers":
                    {
                        JObject json = new JObject();

                        {
                            JArray unconnectedPeers = new JArray();
                            foreach (IPEndPoint peer in LocalNode.GetUnconnectedPeers())
                            {
                                JObject peerJson = new JObject();
                                peerJson["address"] = peer.Address.ToString();
                                peerJson["port"] = peer.Port;
                                unconnectedPeers.Add(peerJson);
                            }
                            json["unconnected"] = unconnectedPeers;
                        }

                        {
                            JArray badPeers = new JArray();
                            foreach (IPEndPoint peer in LocalNode.GetBadPeers())
                            {
                                JObject peerJson = new JObject();
                                peerJson["address"] = peer.Address.ToString();
                                peerJson["port"] = peer.Port;
                                badPeers.Add(peerJson);
                            }
                            json["bad"] = badPeers;
                        }

                        {
                            JArray connectedPeers = new JArray();
                            foreach (RemoteNode node in LocalNode.GetRemoteNodes())
                            {
                                JObject peerJson = new JObject();
                                peerJson["address"] = node.RemoteEndpoint.Address.ToString();
                                peerJson["port"] = node.ListenerEndpoint?.Port ?? 0;
                                connectedPeers.Add(peerJson);
                            }
                            json["connected"] = connectedPeers;
                        }

                        return json;
                    }
                case "getversion":
                    {
                        JObject json = new JObject();
                        json["port"] = LocalNode.Port;
                        json["nonce"] = LocalNode.Nonce;
                        json["useragent"] = LocalNode.UserAgent;
                        return json;
                    }
                case "getclaimamount":
                    {
                        JObject json = new JObject();
                        CoinReference[] args = _params.Count >= 1 ? ((JArray)_params[0]).Select(p => CoinReference.FromJson(p)).ToArray() : new CoinReference[0];
                        Fixed8 amount = Fixed8.Zero;
                        if (_params.Count > 0 && ((JArray)_params[0]).Count > 0 && ((JArray)_params[0])[0].ContainsProperty("status") == true)
                            amount = Blockchain.CalculateBonus(args, Blockchain.Default.Height + 1);
                        else
                            amount = Blockchain.CalculateBonus(args);

                        json["unclaimed"] = amount.ToString();
                        return json;
                    }
                default:
                    throw new RpcException(-32601, "Method not found");
            }
        }

        private async Task ProcessAsync(HttpContext context)
        {
            context.Response.Headers["Access-Control-Allow-Origin"] = "*";
            context.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST";
            context.Response.Headers["Access-Control-Allow-Headers"] = "Content-Type";
            context.Response.Headers["Access-Control-Max-Age"] = "31536000";
            if (context.Request.Method != "GET" && context.Request.Method != "POST") return;
            JObject request = null;
            if (context.Request.Method == "GET")
            {
                string jsonrpc = context.Request.Query["jsonrpc"];
                string id = context.Request.Query["id"];
                string method = context.Request.Query["method"];
                string _params = context.Request.Query["params"];
                if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(method) && !string.IsNullOrEmpty(_params))
                {
                    try
                    {
                        _params = Encoding.UTF8.GetString(Convert.FromBase64String(_params));
                    }
                    catch (FormatException) { }
                    request = new JObject();
                    if (!string.IsNullOrEmpty(jsonrpc))
                        request["jsonrpc"] = jsonrpc;
                    request["id"] = double.Parse(id);
                    request["method"] = method;
                    request["params"] = JObject.Parse(_params);
                }
            }
            else if (context.Request.Method == "POST")
            {
                using (StreamReader reader = new StreamReader(context.Request.Body))
                {
                    try
                    {
                        request = JObject.Parse(reader);
                    }
                    catch (FormatException) { }
                }
            }
            JObject response;
            if (request == null)
            {
                response = CreateErrorResponse(null, -32700, "Parse error");
            }
            else if (request is JArray array)
            {
                if (array.Count == 0)
                {
                    response = CreateErrorResponse(request["id"], -32600, "Invalid Request");
                }
                else
                {
                    response = array.Select(p => ProcessRequest(p)).Where(p => p != null).ToArray();
                }
            }
            else
            {
                response = ProcessRequest(request);
            }
            if (response == null || (response as JArray)?.Count == 0) return;
            context.Response.ContentType = "application/json-rpc";
            await context.Response.WriteAsync(response.ToString());
        }

        private JObject ProcessRequest(JObject request)
        {
            if (!request.ContainsProperty("id")) return null;
            if (!request.ContainsProperty("method") || !request.ContainsProperty("params") || !(request["params"] is JArray))
            {
                return CreateErrorResponse(request["id"], -32600, "Invalid Request");
            }
            JObject result = null;
            try
            {
                Log("Request: " + request);
                Log("Method: " + request["method"].AsString() + " Params: " + (JArray)request["params"]);
                result = Process(request["method"].AsString(), (JArray)request["params"]);
                Log("Method: " + request["method"].AsString() + " Result: " + result);
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
#if DEBUG
                return CreateErrorResponse(request["id"], ex.HResult, ex.Message, ex.StackTrace);
#else
                return CreateErrorResponse(request["id"], ex.HResult, ex.Message);
#endif
            }
            JObject response = CreateResponse(request["id"]);
            response["result"] = result;
            return response;
        }

        public void Start(params string[] uriPrefix)
        {
            Start(uriPrefix, null, null);
        }

        public void Start(string[] uriPrefix, string sslCert, string password)
        {
            int port = 10030;
            host = new WebHostBuilder().UseKestrel(options => options.Listen(IPAddress.Any, port, listenOptions =>
            {
                if (!string.IsNullOrEmpty(sslCert))
                    listenOptions.UseHttps(sslCert, password);
            }))
            .Configure(app =>
            {
                app.UseResponseCompression();
                app.Run(ProcessAsync);
            })
            .ConfigureServices(services =>
            {
                services.AddResponseCompression(options =>
                {
                    // options.EnableForHttps = false;
                    options.Providers.Add<GzipCompressionProvider>();
                    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json-rpc" });
                });

                services.Configure<GzipCompressionProviderOptions>(options =>
                {
                    options.Level = CompressionLevel.Fastest;
                });
            })
            .Build();

            host.Start();
        }

        protected void Log(string message)
        {
            DateTime now = DateTime.Now;
            string line = $"[{now.TimeOfDay:hh\\:mm\\:ss}] {message}";
            //Console.WriteLine(line);
            if (string.IsNullOrEmpty(log_dictionary)) return;
            lock (log_dictionary)
            {
                Directory.CreateDirectory(log_dictionary);
                string path = Path.Combine(log_dictionary, $"{now:yyyy-MM-dd}.log");
                File.AppendAllLines(path, new[] { line });
            }
        }
    }
}
