using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Quras.Core;
using Quras.IO;
using Quras.IO.Caching;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quras.Network
{
    public class LocalNode : IDisposable
    {
        public static event EventHandler<InventoryReceivingEventArgs> InventoryReceiving;
        public static event EventHandler<IInventory> InventoryReceived;

        public const uint ProtocolVersion = 0;
        private const int ConnectedMax = 10;
        private const int UnconnectedMax = 1000;
        public const int MemoryPoolSize = 30000;

        public static LocalNode Default = null;

        static public bool isConsensusNode = false;

        private static readonly Dictionary<UInt256, Transaction> mem_pool = new Dictionary<UInt256, Transaction>();
        private static readonly List<UInt256> mempool_turns = new List<UInt256>();
        private static Dictionary<UInt160, uint> block_pool = new Dictionary<UInt160, uint>();
        private readonly HashSet<Transaction> temp_pool = new HashSet<Transaction>();
        public static Dictionary<UInt160, List<uint>> freetx_pool = new Dictionary<UInt160, List<uint>>();

        internal static readonly List<UInt256> KnownHashes = new List<UInt256>();
        internal readonly RelayCache RelayCache = new RelayCache(100);

        private static readonly HashSet<IPEndPoint> unconnectedPeers = new HashSet<IPEndPoint>();
        private static readonly HashSet<IPEndPoint> badPeers = new HashSet<IPEndPoint>();
        internal readonly List<RemoteNode> connectedPeers = new List<RemoteNode>();

        internal static readonly HashSet<IPAddress> LocalAddresses = new HashSet<IPAddress>();
        internal ushort Port;
        internal readonly uint Nonce;
        private TcpListener listener;
        private IWebHost ws_host;
        private Thread connectThread;
        private Thread poolThread;
        private readonly AutoResetEvent new_tx_event = new AutoResetEvent(false);
        private int started = 0;
        private int disposed = 0;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public bool GlobalMissionsEnabled { get; set; } = true;
        public int RemoteNodeCount => connectedPeers.Count;
        public bool ServiceEnabled { get; set; } = true;
        public bool UpnpEnabled { get; set; } = false;
        public string UserAgent { get; set; }

        private string log_dictionary = Path.Combine(AppContext.BaseDirectory, "Logs");

        static LocalNode()
        {
            LocalAddresses.UnionWith(NetworkInterface.GetAllNetworkInterfaces().SelectMany(p => p.GetIPProperties().UnicastAddresses).Select(p => p.Address.MapToIPv6()));
        }

        public LocalNode()
        {
            var rnd = new byte[4];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(rnd);
            this.Nonce = (uint)BitConverter.ToInt32(rnd, 0);
            this.connectThread = new Thread(ConnectToPeersLoop, 10 * 1024 * 1024)
            {
                IsBackground = true,
                Name = "LocalNode.ConnectToPeersLoop"
            };
            if (Blockchain.Default != null)
            {
                this.poolThread = new Thread(AddTransactionLoop, 2 * 1024 * 1024)
                {
                    IsBackground = true,
                    Name = "LocalNode.AddTransactionLoop"
                };
            }
            this.UserAgent = string.Format("/Quras:{0}/", GetType().GetTypeInfo().Assembly.GetName().Version.ToString(3));
            Blockchain.PersistCompleted += Blockchain_PersistCompleted;
			
			Default = this;
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

        private async Task<bool> AcceptPeers()
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                Socket socket;
                try
                {
                    socket = await listener.AcceptSocketAsync();
                }
                catch (ObjectDisposedException)
                {
                    return false;
                }
                catch (SocketException)
                {
                    continue;
                }
                TcpRemoteNode remoteNode = new TcpRemoteNode(this, socket);
                OnConnected(remoteNode);
            }
            return true;
        }

        private static bool AddTransaction(Transaction tx)
        {
            if (Blockchain.Default == null) return false;
            lock (mem_pool)
            {
                if (mem_pool.ContainsKey(tx.Hash)) return false; 
                if (Blockchain.Default.ContainsTransaction(tx.Hash)) return false; 
                if (!tx.Verify(mem_pool.Values)) return false;
                mem_pool.Add(tx.Hash, tx);
                CheckMemPool();
            }
            return true;
        }

        private void AddTransactionLoop()
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                new_tx_event.WaitOne();
                Transaction[] transactions;
                lock (temp_pool)
                {
                    if (temp_pool.Count == 0) continue;
                    transactions = temp_pool.ToArray();
                    temp_pool.Clear();
                }
                
                lock(temp_pool)
                {
                    temp_pool.Clear();
                }

                ConcurrentBag<Transaction> verified = new ConcurrentBag<Transaction>();
                lock (mem_pool)
                {
                    transactions = transactions.Where(p => !mem_pool.ContainsKey(p.Hash) && !Blockchain.Default.ContainsTransaction(p.Hash)).ToArray();
                    if (transactions.Length == 0) continue;

                    Transaction[] tmpool = mem_pool.Values.Concat(transactions).ToArray();

                    transactions.AsParallel().ForAll(tx =>
                    {
                        try
                        {
                            Console.WriteLine("New Transaction is pending");
                            tx.is_consensus_mempool = true;
                            if (tx.Verify(tmpool))
                            {
                                Console.WriteLine("New transaction added to verify");
                                tx.is_consensus_mempool = false;
                                verified.Add(tx);
                            }
                        }
                        catch(Exception ex)
                        {
                            Log(ex.StackTrace);
                        }
                    });

                    if (verified.Count == 0) continue;

                    foreach (Transaction tx in verified)
                    {
                        mem_pool.Add(tx.Hash, tx);
                    }

                    CheckMemPool();
                }
                RelayDirectly(verified);
                if (InventoryReceived != null)
                    foreach (Transaction tx in verified)
                        InventoryReceived(this, tx);
            }
        }

        public static void AllowHashes(IEnumerable<UInt256> hashes)
        {
            lock (KnownHashes)
            {
                foreach(UInt256 hash in hashes)
                    KnownHashes.Remove(hash);
            }
        }

        private void Blockchain_PersistCompleted(object sender, Block block)
        {
            Transaction[] remain;
            lock (mem_pool)
            {
                foreach (Transaction tx in block.Transactions)
                {
                    mem_pool.Remove(tx.Hash);
                }
                if (mem_pool.Count == 0) return;

                remain = mem_pool.Values.ToArray();

                List<UInt160> scriptHashDictionary = new List<UInt160>();
                Console.WriteLine(remain.Length);
                for (int i = 0; i < remain.Length; i++)
                {
                    if (remain[i].Type == TransactionType.MinerTransaction ||
                            remain[i].Type == TransactionType.AnonymousContractTransaction ||
                            remain[i].Type == TransactionType.RingConfidentialTransaction)
                    {
                        continue;
                    }

                    if (remain[i].Inputs == null || remain[i].Inputs.Length == 0)
                    {
                        continue;
                    }

                    UInt256 prevHash = remain[i].Inputs[0].PrevHash;
                    Transaction tempTx = Blockchain.Default.GetTransaction(prevHash);
                    if (tempTx == null)
                    {
                        bool is_next_trasaction = false;
                        foreach (Transaction prevTx in mem_pool.Values)
                        {
                            if (prevTx.Hash == prevHash)
                            {
                                is_next_trasaction = true;
                                break;
                            }
                        }
                        if (is_next_trasaction == true)
                            continue;
                    }
                    try
                    {
                        UInt160 scripthash = tempTx.Outputs[remain[i].Inputs[0].PrevIndex].ScriptHash;
                        if (scriptHashDictionary.Contains(scripthash))
                        {
                            continue;
                        }
                        else
                        {
                            scriptHashDictionary.Add(scripthash);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("No reference in the blockchain");
                    }
                    if (!remain[i].Verify(remain))
                    {
                        List<Transaction> lst = remain.ToList();
                        lst.RemoveAt(i);
                        remain = lst.ToArray();
                        i--;
                    }
                }

                mem_pool.Clear();
            }
            lock (temp_pool)
            {
                temp_pool.UnionWith(remain);
            }
            new_tx_event.Set();
        }

        public static void RemoveTxFromMempool(UInt256 hash)
        {
            try
            {
                lock (KnownHashes)
                {
                    KnownHashes.Remove(hash);
                }
                if (mem_pool.ContainsKey(hash))
                {
                    foreach (RemoteNode node in LocalNode.Default.GetRemoteNodes()) // enqueue message
                        node.EnqueueMessage("removemem", hash);
                }
                lock (mem_pool)
                {
                    mem_pool.Remove(hash);
                }
            }
            catch(Exception ex)
            {

            }
        }

        public RemoteNode[] GetRemoteNodes()
        {
            lock (connectedPeers)
            {
                return connectedPeers.ToArray();
            }
        }

        public static UInt160 GetFromAddressFromTx(Transaction tx)
        {
            UInt160 senderScriptHash = UInt160.Zero;
            try
            {
                if (tx.Inputs.Length != 0)
                {
                    Transaction prevTx = Blockchain.Default.GetTransaction(tx.Inputs[0].PrevHash);
                    if (prevTx == null)
                    {
                        if (!mem_pool.TryGetValue(tx.Inputs[0].PrevHash, out prevTx))
                            prevTx = null;
                    }

                    if (prevTx != null)
                        senderScriptHash = prevTx.Outputs[tx.Inputs[0].PrevIndex].ScriptHash;
                }
                else if (tx is ClaimTransaction ctx && ctx.Claims.Length != 0)
                {
                    Transaction prevTx = Blockchain.Default.GetTransaction(ctx.Claims[0].PrevHash);
                    if (prevTx == null)
                    {
                        if (!mem_pool.TryGetValue(ctx.Claims[0].PrevHash, out prevTx))
                            prevTx = null;
                    }

                    if (prevTx != null)
                        senderScriptHash = prevTx.Outputs[ctx.Claims[0].PrevIndex].ScriptHash;
                }
            }
            catch (Exception ex)
            {
                senderScriptHash = UInt160.Zero;
            }
            
            return senderScriptHash;
        }

        private static void CheckMemPool()
        {
            Dictionary<UInt160, int> txCountDic = new Dictionary<UInt160, int>();

            for (int i = 0; i < mem_pool.Count; i++)
            {
                try
                {
                    UInt160 senderScriptHash;
                    Transaction tx = mem_pool.ElementAt(i).Value;
                    if (tx.Inputs.Length != 0)
                    {
                        Transaction prevTx = Blockchain.Default.GetTransaction(tx.Inputs[0].PrevHash);
                        if (prevTx == null)
                        {
                            if (!mem_pool.TryGetValue(tx.Inputs[0].PrevHash, out prevTx)) continue;
                        }

                        if (prevTx == null) continue;

                        senderScriptHash = prevTx.Outputs[tx.Inputs[0].PrevIndex].ScriptHash;
                    }
                    else
                        continue;

                    if (Blockchain.IsConsensusAddress(senderScriptHash))
                        continue;

                    for (int j = 0; j < block_pool.Count; j++)
                    {
                        if (Blockchain.Default.Height - block_pool.ElementAt(j).Value > Blockchain.Default.BlockTxPeriodNum)
                        {
                            RemoveBlockPool(block_pool.ElementAt(j).Key);
                            j--;
                        }
                    }

                    if (block_pool.ContainsKey(senderScriptHash))
                    {
                        RemoveTxFromMempool(mem_pool.ElementAt(i).Key);
                        continue;
                    }

                    if (txCountDic.ContainsKey(senderScriptHash))
                        txCountDic[senderScriptHash]++;
                    else
                        txCountDic.Add(senderScriptHash, 1);
                    if (txCountDic[senderScriptHash] > Blockchain.Default.BlockTransactionLimit)
                    {
                        lock(mem_pool)
                        {
                            RemoveTxFromMempool(mem_pool.ElementAt(i).Key);
                        }
                        i--;

                        if (!block_pool.ContainsKey(senderScriptHash))
                        {
                            LocalNode.Default.AddBlockAddressToPool(senderScriptHash);

                            foreach (RemoteNode node in LocalNode.Default.GetRemoteNodes()) // enqueue message
                                node.EnqueueMessage("blockwallet", senderScriptHash);
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }

            if (mem_pool.Count <= MemoryPoolSize) return;
            UInt256[] hashes = mem_pool.Values.AsParallel().OrderBy(p => p.NetworkFee / p.Size).Take(mem_pool.Count - MemoryPoolSize).Select(p => p.Hash).ToArray();
            foreach (UInt256 hash in hashes)
            {
                RemoveTxFromMempool(hash);
            }
        }
        public void AddBlockAddressToPool(UInt160 scripthash)
        {
            lock(block_pool)
            {
                if (!block_pool.ContainsKey(scripthash))
                    block_pool.Add(scripthash, Blockchain.Default.Height);
                Console.WriteLine("Block item added - " + scripthash.ToString());
            }
        }
        public static void RemoveBlockPool(UInt160 scripthash)
        {
            lock (block_pool)
            {
                if (block_pool.ContainsKey(scripthash))
                    block_pool.Remove(scripthash);
                Console.WriteLine("Block item removed - " + scripthash.ToString());
            }
        }

        public void AddFreeTxAddressToPool(UInt160 scripthash)
        {
            lock(freetx_pool)
            {
                if (freetx_pool.ContainsKey(scripthash) && freetx_pool[scripthash].Contains(Blockchain.Default.Height))
                    return;

                if (!freetx_pool.ContainsKey(scripthash))
                {
                    List<uint> lstHeight = new List<uint>();
                    lstHeight.Add(Blockchain.Default.Height);
                    freetx_pool.Add(scripthash, lstHeight);
                }
                else
                {
                    if (freetx_pool[scripthash].Count >= 3)
                    {
                        if (Blockchain.Default.Height - freetx_pool[scripthash].ElementAt(freetx_pool[scripthash].Count - 1) < Blockchain.Default.FreeTransactionBlockPeriodNum)
                            return;
                        freetx_pool[scripthash].Clear();
                    }
                    freetx_pool[scripthash].Add(Blockchain.Default.Height);
                }
                Console.WriteLine("Free Tx item added - " + Wallets.Wallet.ToAddress(scripthash));
            }
        }

        public void RemoveFreeTxAddressToPool(UInt160 scripthash)
        {
            lock (freetx_pool)
            {
                if (freetx_pool.ContainsKey(scripthash))
                    freetx_pool.Remove(scripthash);
                Console.WriteLine("Free Tx item removed - " + Wallets.Wallet.ToAddress(scripthash));
            }
        }

        public async Task ConnectToPeerAsync(string hostNameOrAddress, int port)
        {
            IPAddress ipAddress;
            if (IPAddress.TryParse(hostNameOrAddress, out ipAddress))
            {
                ipAddress = ipAddress.MapToIPv6();
            }
            else
            {
                IPHostEntry entry;
                try
                {
                    entry = await Dns.GetHostEntryAsync(hostNameOrAddress);
                }
                catch (SocketException)
                {
                    return;
                }
                ipAddress = entry.AddressList.FirstOrDefault(p => p.AddressFamily == AddressFamily.InterNetwork || p.IsIPv6Teredo)?.MapToIPv6();
                if (ipAddress == null) return;
            }
            await ConnectToPeerAsync(new IPEndPoint(ipAddress, port));
        }

        public async Task ConnectToPeerAsync(IPEndPoint remoteEndpoint)
        {
            if (remoteEndpoint.Port == Port && LocalAddresses.Contains(remoteEndpoint.Address)) return;
            lock (unconnectedPeers)
            {
                unconnectedPeers.Remove(remoteEndpoint);
            }
            lock (connectedPeers)
            {
                if (connectedPeers.Any(p => remoteEndpoint.Equals(p.ListenerEndpoint)))
                    return;
            }
            TcpRemoteNode remoteNode = new TcpRemoteNode(this, remoteEndpoint);
            if (await remoteNode.ConnectAsync())
            {
                OnConnected(remoteNode);
            }
        }

        private async void ConnectToPeersLoop()
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                int connectedCount = connectedPeers.Count;
                int unconnectedCount = unconnectedPeers.Count;
                if (connectedCount < ConnectedMax)
                {
                    Task[] tasks = { };
                    if (unconnectedCount > 0)
                    {
                        IPEndPoint[] endpoints;
                        lock (unconnectedPeers)
                        {
                            endpoints = unconnectedPeers.Take(ConnectedMax - connectedCount).ToArray();
                        }
                        tasks = endpoints.Select(p => ConnectToPeerAsync(p)).ToArray();
                    }
                    else if (connectedCount > 0)
                    {
                        lock (connectedPeers)
                        {
                            foreach (RemoteNode node in connectedPeers)
                                node.RequestPeers();
                        }
                    }
                    else
                    {
                        tasks = Settings.Default.SeedList.OfType<string>().Select(p => p.Split(':')).Select(p => ConnectToPeerAsync(p[0], int.Parse(p[1]))).ToArray();
                    }
                    try
                    {
                        Task.WaitAll(tasks, cancellationTokenSource.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
                for (int i = 0; i < 50 && !cancellationTokenSource.IsCancellationRequested; i++)
                {
                    await Task.Delay(100);
                }
            }
        }

        public static bool ContainsTransaction(UInt256 hash)
        {
            lock (mem_pool)
            {
                return mem_pool.ContainsKey(hash);
            }
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref disposed, 1) == 0)
            {
                cancellationTokenSource.Cancel();
                if (started > 0)
                {
                    Blockchain.PersistCompleted -= Blockchain_PersistCompleted;
                    if (listener != null) listener.Stop();
                    if (!connectThread.ThreadState.HasFlag(ThreadState.Unstarted)) connectThread.Join();
                    lock (unconnectedPeers)
                    {
                        if (unconnectedPeers.Count < UnconnectedMax)
                        {
                            lock (connectedPeers)
                            {
                                unconnectedPeers.UnionWith(connectedPeers.Select(p => p.ListenerEndpoint).Where(p => p != null).Take(UnconnectedMax - unconnectedPeers.Count));
                            }
                        }
                    }
                    RemoteNode[] nodes;
                    lock (connectedPeers)
                    {
                        nodes = connectedPeers.ToArray();
                    }
                    Task.WaitAll(nodes.Select(p => Task.Run(() => p.Disconnect(false))).ToArray());
                    new_tx_event.Set();
                    if (poolThread?.ThreadState.HasFlag(ThreadState.Unstarted) == false)
                        poolThread.Join();
                    new_tx_event.Dispose();
                }
            }
        }

        public static IEnumerable<Transaction> GetMemoryPool()
        {
            lock (mem_pool)
            {
                foreach(Transaction tx in mem_pool.Values)
                {
                    yield return tx;
                }
            }
        }

        public static IEnumerable<Transaction> GetMemoryPoolByScriptHash(UInt160 ScriptHash)
        {
            lock(mem_pool)
            {
                Console.WriteLine(mem_pool.Count);
                foreach (Transaction tx in mem_pool.Values)
                {
                    foreach(CoinReference refer in tx.Inputs)
                    {
                        Transaction prevTrans = Blockchain.Default.GetTransaction(refer.PrevHash);
                        if (prevTrans != null)
                        {
                            if (prevTrans.Outputs.Length > refer.PrevIndex)
                            {
                                if (prevTrans.Outputs[refer.PrevIndex].ScriptHash.Equals(ScriptHash))
                                    yield return tx;
                            }
                        }
                        else
                        {
                            foreach(Transaction txTemp in mem_pool.Values)
                            {
                                if (txTemp.Hash.Equals(refer.PrevHash))
                                {
                                    if (txTemp.Outputs.Length > refer.PrevIndex)
                                        if (txTemp.Outputs[refer.PrevIndex].ScriptHash.Equals(ScriptHash))
                                            yield return tx;
                                }
                                if (txTemp.Equals(tx))
                                    continue;
                            }
                        }
                    }
                }
            }
        }

        public static Transaction GetTransaction(UInt256 hash)
        {
            lock (mem_pool)
            {
                if (!mem_pool.TryGetValue(hash, out Transaction tx))
                    return null;
                return tx;
            }
        }

        private static bool IsIntranetAddress(IPAddress address)
        {
            byte[] data = address.MapToIPv4().GetAddressBytes();
            Array.Reverse(data);
            uint value = data.ToUInt32(0);
            return (value & 0xff000000) == 0x0a000000 || (value & 0xff000000) == 0x7f000000 || (value & 0xfff00000) == 0xac100000 || (value & 0xffff0000) == 0xc0a80000 || (value & 0xffff0000) == 0xa9fe0000;
        }

        public static void LoadState(Stream stream)
        {
            lock (unconnectedPeers)
            {
                unconnectedPeers.Clear();
                using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII, true))
                {
                    int count = reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        IPAddress address = new IPAddress(reader.ReadBytes(4));
                        int port = reader.ReadUInt16();
                        unconnectedPeers.Add(new IPEndPoint(address.MapToIPv6(), port));
                    }
                }
            }
        }

        private void OnConnected(RemoteNode remoteNode)
        {
            lock (connectedPeers)
            {
                connectedPeers.Add(remoteNode);
            }
            remoteNode.Disconnected += RemoteNode_Disconnected;
            remoteNode.InventoryReceived += RemoteNode_InventoryReceived;
            remoteNode.PeersReceived += RemoteNode_PeersReceived;
            remoteNode.StartProtocol();
        }

        private async Task ProcessWebSocketAsync(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest) return;
            WebSocket ws = await context.WebSockets.AcceptWebSocketAsync();
            WebSocketRemoteNode remoteNode = new WebSocketRemoteNode(this, ws, new IPEndPoint(context.Connection.RemoteIpAddress, context.Connection.RemotePort));
            OnConnected(remoteNode);
        }

        public bool Relay(IInventory inventory, bool IsPendingData = false)
        {
            if (IsPendingData == false)
            {
                if (inventory is MinerTransaction) return false;
                lock (KnownHashes)
                {
                    if (KnownHashes.Contains(inventory.Hash))
                    {
                        return false;
                    }
                }
            }

            InventoryReceivingEventArgs args = new InventoryReceivingEventArgs(inventory);
            InventoryReceiving?.Invoke(this, args);
            if (args.Cancel) return false;
            if (inventory is Block block)
            {
                if (Blockchain.Default == null) return false;
                if (Blockchain.Default.ContainsBlock(block.Hash)) return false;
                if (!Blockchain.Default.AddBlock(block)) return false;
            }
            else if (inventory is Transaction)
            {
                if (!AddTransaction((Transaction)inventory)) return false;
            }
            else //if (inventory is Consensus)
            {
                if (!inventory.Verify()) return false;
            }

            KnownHashes.Add(inventory.Hash);

            bool relayed = RelayDirectly(inventory);
            InventoryReceived?.Invoke(this, inventory);
            return relayed;
        }

        public bool RelayDirectly(IInventory inventory)
        {
            bool relayed = false;
            lock (connectedPeers)
            {
                RelayCache.Add(inventory);
                foreach (RemoteNode node in connectedPeers)
                    relayed |= node.Relay(inventory);
            }
            return relayed;
        }
         
        private void RelayDirectly(IReadOnlyCollection<Transaction> transactions)
        {
            lock (connectedPeers)
            {
                foreach (RemoteNode node in connectedPeers)
                    node.Relay(transactions);
            }
        }

        private void RemoteNode_Disconnected(object sender, bool error)
        {
            RemoteNode remoteNode = (RemoteNode)sender;
            remoteNode.Disconnected -= RemoteNode_Disconnected;
            remoteNode.InventoryReceived -= RemoteNode_InventoryReceived;
            remoteNode.PeersReceived -= RemoteNode_PeersReceived;
            if (error && remoteNode.ListenerEndpoint != null)
            {
                lock (badPeers)
                {
                    badPeers.Add(remoteNode.ListenerEndpoint);
                }
            }
            lock (unconnectedPeers)
            {
                lock (connectedPeers)
                {
                    if (remoteNode.ListenerEndpoint != null)
                    {
                        unconnectedPeers.Remove(remoteNode.ListenerEndpoint);
                    }
                    connectedPeers.Remove(remoteNode);
                }
            }
        }

        private void RemoteNode_InventoryReceived(object sender, IInventory inventory)
        {
            if (inventory is Transaction tx && tx.Type != TransactionType.ClaimTransaction && tx.Type != TransactionType.IssueTransaction)
            {
                if (Blockchain.Default == null) return;
                lock (KnownHashes)
                {
                    if (!KnownHashes.Contains(inventory.Hash))
                    {
                        KnownHashes.Add(inventory.Hash);
                    }
                }
                InventoryReceivingEventArgs args = new InventoryReceivingEventArgs(inventory);
                InventoryReceiving?.Invoke(this, args);
                if (args.Cancel) return;
                lock (temp_pool)
                {
                    temp_pool.Add(tx);
                }
                new_tx_event.Set();
            }
            else
            {
                Relay(inventory);
            }
        }

        private void RemoteNode_PeersReceived(object sender, IPEndPoint[] peers)
        {
            lock (unconnectedPeers)
            {
                if (unconnectedPeers.Count < UnconnectedMax)
                {
                    lock (badPeers)
                    {
                        lock (connectedPeers)
                        {
                            unconnectedPeers.UnionWith(peers);
                            unconnectedPeers.ExceptWith(badPeers);
                            unconnectedPeers.ExceptWith(connectedPeers.Select(p => p.ListenerEndpoint));
                        }
                    }
                }
            }
        }

        public IPEndPoint[] GetUnconnectedPeers()
        {
            lock (unconnectedPeers)
            {
                return unconnectedPeers.ToArray();
            }
        }

        public IPEndPoint[] GetBadPeers()
        {
            lock (badPeers)
            {
                return badPeers.ToArray();
            }
        }

        public static void SaveState(Stream stream)
        {
            IPEndPoint[] peers;
            lock (unconnectedPeers)
            {
                peers = unconnectedPeers.Take(UnconnectedMax).ToArray();
            }
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII, true))
            {
                writer.Write(peers.Length);
                foreach (IPEndPoint endpoint in peers)
                {
                    writer.Write(endpoint.Address.MapToIPv4().GetAddressBytes());
                    writer.Write((ushort)endpoint.Port);
                }
            }
        }

        public void Start(int port = 0, int ws_port = 0)
        {
            if (Interlocked.Exchange(ref started, 1) == 0)
            {
                Task.Run(async () =>
                {
                    if ((port > 0 || ws_port > 0)
                        && UpnpEnabled
                        && LocalAddresses.All(p => !p.IsIPv4MappedToIPv6 || IsIntranetAddress(p))
                        && await UPnP.DiscoverAsync())
                    {
                        try
                        {
                            LocalAddresses.Add(await UPnP.GetExternalIPAsync());
                            if (port > 0)
                                await UPnP.ForwardPortAsync(port, ProtocolType.Tcp, "Quras");
                            if (ws_port > 0)
                                await UPnP.ForwardPortAsync(ws_port, ProtocolType.Tcp, "Quras WebSocket");
                        }
                        catch { }
                    }
                    connectThread.Start();
                    poolThread?.Start();
                    if (port > 0)
                    {
                        listener = new TcpListener(IPAddress.Any, port);
                        try
                        {
                            listener.Start();
                            Port = (ushort)port;
                            await AcceptPeers();
                        }
                        catch (SocketException) { }
                    }
                    if (ws_port > 0)
                    {
                        ws_host = new WebHostBuilder().UseKestrel().UseUrls($"http://*:{ws_port}").Configure(app => app.UseWebSockets().Run(ProcessWebSocketAsync)).Build();
                        ws_host.Start();
                    }
                });
            }
        }

        public void SynchronizeMemoryPool()
        {
            lock (connectedPeers)
            {
                foreach (RemoteNode node in connectedPeers)
                    node.RequestMemoryPool();
            }
        }
    }
}
