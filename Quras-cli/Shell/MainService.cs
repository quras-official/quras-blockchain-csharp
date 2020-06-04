﻿using Quras.Consensus;
using Quras.Core;
using Quras.Implementations.Blockchains.LevelDB;
using Quras.Implementations.Wallets.EntityFramework;
using Quras.IO;
using Quras.IO.Json;
using Quras.Network;
using Quras.Network.RPC;
using Quras.Services;
using Quras.SmartContract;
using Quras.VM;
using Quras.Wallets;
using Quras.Core.Anonoymous;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace Quras.Shell
{
    internal class MainService : ConsoleServiceBase
    {
        private const string PeerStatePath = "peers.dat";

        private RpcServerWithWallet rpc;
        private ConsensusWithPolicy consensus;

        protected LocalNode LocalNode { get; private set; }
        protected override string Prompt => "quras";
        public override string ServiceName => "QURAS-CLI";

        private void ImportBlocks(Stream stream)
        {
            LevelDBBlockchain blockchain = (LevelDBBlockchain)Blockchain.Default;
            blockchain.VerifyBlocks = false;
            using (BinaryReader r = new BinaryReader(stream))
            {
                uint count = r.ReadUInt32();
                for (int height = 0; height < count; height++)
                {
                    byte[] array = r.ReadBytes(r.ReadInt32());
                    if (height > Blockchain.Default.Height)
                    {
                        Block block = array.AsSerializable<Block>();
                        Blockchain.Default.AddBlock(block);
                    }
                }
            }
            blockchain.VerifyBlocks = true;
        }

        protected override bool OnCommand(string[] args)
        {
            switch (args[0].ToLower())
            {
                case "create":
                    return OnCreateCommand(args);
                case "export":
                    return OnExportCommand(args);
                case "help":
                    return OnHelpCommand(args);
                case "import":
                    return OnImportCommand(args);
                case "list":
                    return OnListCommand(args);
                case "claim":
                    return OnClaimCommand(args);
                case "open":
                    return OnOpenCommand(args);
                case "rebuild":
                    return OnRebuildCommand(args);
                case "refresh":
                    return OnRefreshCommand(args);
                case "send":
                    return OnSendCommand(args);
                case "show":
                    return OnShowCommand(args);
                case "start":
                    return OnStartCommand(args);
                case "upgrade":
                    return OnUpgradeCommand(args);
                default:
                    return base.OnCommand(args);
            }
        }

        private bool OnCreateCommand(string[] args)
        {
            switch (args[1].ToLower())
            {
                case "address":
                    return OnCreateAddressCommand(args);
                case "wallet":
                    return OnCreateWalletCommand(args);
                default:
                    return base.OnCommand(args);
            }
        }

        private bool OnCreateAddressCommand(string[] args)
        {
            if (Program.Wallet == null)
            {
                Console.WriteLine("You have to open the wallet first.");
                return true;
            }
            if (args.Length > 3)
            {
                Console.WriteLine("error");
                return true;
            }
            ushort count = 1;
            if (args.Length >= 3)
                count = ushort.Parse(args[2]);
            List<string> addresses = new List<string>();
            for (int i = 1; i <= count; i++)
            {
                KeyPair key = (KeyPair)Program.Wallet.CreateKey();
                VerificationContract contract = Program.Wallet.GetContracts(key.PublicKeyHash).First(p => p.IsStandard);
                addresses.Add(contract.Address);
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"[{i}/{count}]");
            }
            Console.WriteLine();
            string path = "address.txt";
            Console.WriteLine($"export addresses to {path}");
            File.WriteAllLines(path, addresses);
            return true;
        }

        private bool OnCreateWalletCommand(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("error");
                return true;
            }
            using (SecureString password = ReadSecureString("password"))
            using (SecureString password2 = ReadSecureString("password"))
            {
                if (!password.CompareTo(password2))
                {
                    Console.WriteLine("error");
                    return true;
                }
                Program.Wallet = UserWallet.Create(args[2], password);
            }
            VerificationContract contract = Program.Wallet.GetContracts().First(p => p.IsStandard);
            KeyPair key = (KeyPair)Program.Wallet.GetKey(contract.PublicKeyHash);
            Console.WriteLine($"address: {contract.Address}");
            Console.WriteLine($" pubkey: {key.PublicKey.EncodePoint(true).ToHexString()}");
            Console.WriteLine($"   Type: {key.nVersion}");
            return true;
        }

        private bool OnExportCommand(string[] args)
        {
            switch (args[1].ToLower())
            {
                case "blocks":
                    return OnExportBlocksCommand(args);
                case "key":
                    return OnExportKeyCommand(args);
                default:
                    return base.OnCommand(args);
            }
        }

        private bool OnExportBlocksCommand(string[] args)
        {
            if (args.Length > 3)
            {
                Console.WriteLine("error");
                return true;
            }
            string path = args.Length >= 3 ? args[2] : "chain.acc";
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                uint count = Blockchain.Default.Height + 1;
                uint start = 0;
                if (fs.Length > 0)
                {
                    byte[] buffer = new byte[sizeof(uint)];
                    fs.Read(buffer, 0, buffer.Length);
                    start = BitConverter.ToUInt32(buffer, 0);
                    fs.Seek(0, SeekOrigin.Begin);
                }
                if (start < count)
                    fs.Write(BitConverter.GetBytes(count), 0, sizeof(uint));
                fs.Seek(0, SeekOrigin.End);
                for (uint i = start; i < count; i++)
                {
                    Block block = Blockchain.Default.GetBlock(i);
                    byte[] array = block.ToArray();
                    fs.Write(BitConverter.GetBytes(array.Length), 0, sizeof(int));
                    fs.Write(array, 0, array.Length);
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write($"[{i + 1}/{count}]");
                }
            }
            Console.WriteLine();
            return true;
        }

        private bool OnExportKeyCommand(string[] args)
        {
            if (Program.Wallet == null)
            {
                Console.WriteLine("You have to open the wallet first.");
                return true;
            }
            if (args.Length < 2 || args.Length > 4)
            {
                Console.WriteLine("error");
                return true;
            }
            UInt160 scriptHash = null;
            string path = null;
            if (args.Length == 3)
            {
                try
                {
                    scriptHash = Wallet.ToScriptHash(args[2]);
                }
                catch (FormatException)
                {
                    path = args[2];
                }
            }
            else if (args.Length == 4)
            {
                scriptHash = Wallet.ToScriptHash(args[2]);
                path = args[3];
            }
            using (SecureString password = ReadSecureString("password"))
            {
                if (password.Length == 0)
                {
                    Console.WriteLine("cancelled");
                    return true;
                }
                if (!Program.Wallet.VerifyPassword(password))
                {
                    Console.WriteLine("Incorrect password");
                    return true;
                }
            }
            IEnumerable<KeyPair> keys;
            if (scriptHash == null)
                keys = (KeyPair[])Program.Wallet.GetKeys();
            else
                keys = new[] { (KeyPair)Program.Wallet.GetKeyByScriptHash(scriptHash) };
            if (path == null)
                foreach (KeyPair key in keys)
                    Console.WriteLine(key.Export());
            else
                File.WriteAllLines(path, keys.Select(p => p.Export()));
            return true;
        }

        private bool OnHelpCommand(string[] args)
        {
            Console.Write(
                "Normal Commands:\n" +
                "\tversion\n" +
                "\thelp\n" +
                "\tclear\n" +
                "\texit\n" +
                "Wallet Commands:\n" +
                "\tcreate wallet <path>\n" +
                "\topen wallet <path>\n" +
                "\tupgrade wallet <path>\n" +
                "\trebuild index\n" +
                "\tlist address\n" +
                "\tlist asset\n" +
                "\tlist key\n" +
                "\tshow utxo [id|alias]\n" +
                "\tshow gas\n" +
                "\tclaim gas\n" +
                "\tcreate address [n=1]\n" +
                "\timport key <wif|path>\n" +
                "\texport key [address] [path]\n" +
                "\tsend <id|alias> <address> <value>|all [fee=0]\n" +
                "Node Commands:\n" +
                "\tshow state\n" +
                "\tshow node\n" +
                "\tshow pool\n" +
                "\texport blocks [path=chain.acc]\n" +
                "Advanced Commands:\n" +
                "\tstart consensus\n" +
                "\trefresh policy\n");
            return true;
        }

        private bool OnImportCommand(string[] args)
        {
            switch (args[1].ToLower())
            {
                case "key":
                    return OnImportKeyCommand(args);
                default:
                    return base.OnCommand(args);
            }
        }

        private bool OnImportKeyCommand(string[] args)
        {
            if (args.Length > 3)
            {
                Console.WriteLine("error");
                return true;
            }
            byte[] prikey = null;
            try
            {
                prikey = Wallet.GetPrivateKeyFromWIF(args[2]);
            }
            catch (FormatException) { }
            if (prikey == null)
            {
                string[] lines = File.ReadAllLines(args[2]);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Length == 64)
                        prikey = lines[i].HexToBytes();
                    else
                        prikey = Wallet.GetPrivateKeyFromWIF(lines[i]);
                    Program.Wallet.CreateKey(prikey);
                    Array.Clear(prikey, 0, prikey.Length);
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write($"[{i + 1}/{lines.Length}]");
                }
                Console.WriteLine();
            }
            else
            {
                KeyPair key = Program.Wallet.CreateKey(prikey);
                Array.Clear(prikey, 0, prikey.Length);
                VerificationContract contract = Program.Wallet.GetContracts(key.PublicKeyHash).First(p => p.IsStandard);
                Console.WriteLine($"address: {contract.Address}");
                Console.WriteLine($" pubkey: {key.PublicKey.EncodePoint(true).ToHexString()}");
            }
            return true;
        }

        private bool OnListCommand(string[] args)
        {
            switch (args[1].ToLower())
            {
                case "address":
                    return OnListAddressCommand(args);
                case "asset":
                    return OnListAssetCommand(args);
                case "key":
                    return OnListKeyCommand(args);
                default:
                    return base.OnCommand(args);
            }
        }

        private bool OnClaimCommand(string[] args)
        {
            if (Program.Wallet == null)
            {
                Console.WriteLine($"Please open a wallet");
                return true;
            }

            Coins coins = new Coins(Program.Wallet, LocalNode);

            switch (args[1].ToLower())
            {
                case "gas":
                    ClaimTransaction tx = coins.Claim();
                    if (tx is ClaimTransaction)
                    {
                        Console.WriteLine($"Tranaction Suceeded: {tx.Hash.ToString()}");
                    }
                    return true;
                default:
                    return base.OnCommand(args);
            }
        }

        private bool OnShowGasCommand(string[] args)
        {
            if (Program.Wallet == null)
            {
                Console.WriteLine($"Please open a wallet");
                return true;
            }

            Coins coins = new Coins(Program.Wallet, LocalNode);
            Console.WriteLine($"unavailable: {coins.UnavailableBonus().ToString()}");
            Console.WriteLine($"  available: {coins.AvailableBonus().ToString()}");
            return true;
        }

        private bool OnListKeyCommand(string[] args)
        {
            if (Program.Wallet == null) return true;
            foreach (KeyPair key in Program.Wallet.GetKeys())
            {
                Console.WriteLine(key.PublicKey);
            }
            return true;
        }

        private bool OnListAddressCommand(string[] args)
        {
            if (Program.Wallet == null) return true;
            foreach (VerificationContract contract in Program.Wallet.GetContracts())
            {
                Console.WriteLine($"{contract.Address}\t{(contract.IsStandard ? "Standard" : "Nonstandard")}");
            }
            return true;
        }

        private bool OnListAssetCommand(string[] args)
        {
            if (Program.Wallet == null) return true;
            foreach (var item in Program.Wallet.GetCoins().Where(p => !p.State.HasFlag(CoinState.Spent)).GroupBy(p => p.Output.AssetId, (k, g) => new
            {
                Asset = Blockchain.Default.GetAssetState(k),
                Balance = g.Sum(p => p.Output.Value),
                Confirmed = g.Where(p => p.State.HasFlag(CoinState.Confirmed)).Sum(p => p.Output.Value)
            }))
            {
                Console.WriteLine($"       id:{item.Asset.AssetId}");
                Console.WriteLine($"     name:{item.Asset.GetName()}");
                Console.WriteLine($"  balance:{item.Balance}");
                Console.WriteLine($"confirmed:{item.Confirmed}");
                Console.WriteLine();
            }
            return true;
        }

        private bool OnOpenCommand(string[] args)
        {
            switch (args[1].ToLower())
            {
                case "wallet":
                    return OnOpenWalletCommand(args);
                default:
                    return base.OnCommand(args);
            }
        }

        //TODO: 目前没有想到其它安全的方法来保存密码
        //所以只能暂时手动输入，但如此一来就不能以服务的方式启动了
        //未来再想想其它办法，比如采用智能卡之类的
        private bool OnOpenWalletCommand(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("error");
                return true;
            }
            if (!File.Exists(args[2]))
            {
                Console.WriteLine($"File does not exist");
                return true;
            }
            using (SecureString password = ReadSecureString("password"))
            {
                if (password.Length == 0)
                {
                    Console.WriteLine("cancelled");
                    return true;
                }
                try
                {
                    Program.Wallet = UserWallet.Open(args[2], password);
                }
                catch
                {
                    Console.WriteLine($"failed to open file \"{args[2]}\"");
                    return true;
                }
            }
            return true;
        }

        private bool OnRebuildCommand(string[] args)
        {
            switch (args[1].ToLower())
            {
                case "index":
                    return OnRebuildIndexCommand(args);
                default:
                    return base.OnCommand(args);
            }
        }

        private bool OnRebuildIndexCommand(string[] args)
        {
            if (Program.Wallet == null) return true;
            Program.Wallet.Rebuild();
            return true;
        }

        private bool OnRefreshCommand(string[] args)
        {
            switch (args[1].ToLower())
            {
                case "policy":
                    return OnRefreshPolicyCommand(args);
                default:
                    return base.OnCommand(args);
            }
        }

        private bool OnRefreshPolicyCommand(string[] args)
        {
            if (consensus == null) return true;
            consensus.RefreshPolicy();
            return true;
        }

        private bool OnSendCommand(string[] args)
        {
            if (args.Length < 4 || args.Length > 5)
            {
                Console.WriteLine("error");
                return true;
            }
            if (Program.Wallet == null)
            {
                Console.WriteLine("You have to open the wallet first.");
                return true;
            }
            using (SecureString password = ReadSecureString("password"))
            {
                if (password.Length == 0)
                {
                    Console.WriteLine("cancelled");
                    return true;
                }
                if (!Program.Wallet.VerifyPassword(password))
                {
                    Console.WriteLine("Incorrect password");
                    return true;
                }
            }
            UIntBase assetId;
            switch (args[1].ToLower())
            {
                case "Quras":
                case "ans":
                    assetId = Blockchain.GoverningToken.Hash;
                    break;
                case "gas":
                case "anc":
                    assetId = Blockchain.UtilityToken.Hash;
                    break;
                default:
                    assetId = UIntBase.Parse(args[1]);
                    break;
            }
            UInt160 scriptHash = Wallet.ToScriptHash(args[2]);
            bool isSendAll = string.Equals(args[3], "all", StringComparison.OrdinalIgnoreCase);
            Transaction tx;
            if (isSendAll)
            {
                Coin[] coins = Program.Wallet.FindUnspentCoins().Where(p => p.Output.AssetId.Equals(assetId)).ToArray();
                tx = new ContractTransaction
                {
                    Attributes = new TransactionAttribute[0],
                    Inputs = coins.Select(p => p.Reference).ToArray(),
                    Outputs = new[]
                    {
                        new TransactionOutput
                        {
                            AssetId = (UInt256)assetId,
                            Value = coins.Sum(p => p.Output.Value),
                            ScriptHash = scriptHash
                        }
                    }
                };
            }
            else
            {
                AssetDescriptor descriptor = new AssetDescriptor(assetId);
                if (!BigDecimal.TryParse(args[3], descriptor.Decimals, out BigDecimal amount))
                {
                    Console.WriteLine("Incorrect Amount Format");
                    return true;
                }
                Fixed8 fee = args.Length >= 5 ? Fixed8.Parse(args[4]) : Fixed8.Zero;
                tx = Program.Wallet.MakeTransaction(null, new[]
                {
                    new TransferOutput
                    {
                        AssetId = assetId,
                        Value = amount,
                        ScriptHash = scriptHash
                    }
                }, fee: fee);
                if (tx == null)
                {
                    Console.WriteLine("Insufficient funds");
                    return true;
                }
            }
            ContractParametersContext context = new ContractParametersContext(tx);
            Program.Wallet.Sign(context);
            if (context.Completed)
            {
                tx.Scripts = context.GetScripts();
                Program.Wallet.SaveTransaction(tx);
                LocalNode.Relay(tx);
                Console.WriteLine($"TXID: {tx.Hash}");
            }
            else
            {
                Console.WriteLine("SignatureContext:");
                Console.WriteLine(context.ToString());
            }
            return true;
        }

        private bool OnShowCommand(string[] args)
        {
            switch (args[1].ToLower())
            {
                case "gas":
                    return OnShowGasCommand(args);
                case "node":
                    return OnShowNodeCommand(args);
                case "pool":
                    return OnShowPoolCommand(args);
                case "state":
                    return OnShowStateCommand(args);
                case "utxo":
                    return OnShowUtxoCommand(args);
                default:
                    return base.OnCommand(args);
            }
        }

        private bool OnShowNodeCommand(string[] args)
        {
            RemoteNode[] nodes = LocalNode.GetRemoteNodes();
            for (int i = 0; i < nodes.Length; i++)
            {
                Console.WriteLine($"{nodes[i].RemoteEndpoint.Address} port:{nodes[i].RemoteEndpoint.Port} listen:{nodes[i].ListenerEndpoint?.Port ?? 0} [{i + 1}/{nodes.Length}]");
            }
            return true;
        }

        private bool OnShowPoolCommand(string[] args)
        {
            foreach (Transaction tx in LocalNode.GetMemoryPool())
            {
                Console.WriteLine($"{tx.Hash} {tx.GetType().Name}");
            }
            return true;
        }

        private bool OnShowStateCommand(string[] args)
        {
            Console.WriteLine($"Height: {Blockchain.Default.Height}/{Blockchain.Default.HeaderHeight}, Nodes: {LocalNode.RemoteNodeCount}");
            return true;
        }

        private bool OnShowUtxoCommand(string[] args)
        {
            if (Program.Wallet == null)
            {
                Console.WriteLine("You have to open the wallet first.");
                return true;
            }
            IEnumerable<Coin> coins = Program.Wallet.FindUnspentCoins();
            if (args.Length >= 3)
            {
                UInt256 assetId;
                switch (args[2].ToLower())
                {
                    case "Quras":
                    case "ans":
                        assetId = Blockchain.GoverningToken.Hash;
                        break;
                    case "gas":
                    case "anc":
                        assetId = Blockchain.UtilityToken.Hash;
                        break;
                    default:
                        assetId = UInt256.Parse(args[2]);
                        break;
                }
                coins = coins.Where(p => p.Output.AssetId.Equals(assetId));
            }
            Coin[] coins_array = coins.ToArray();
            const int MAX_SHOW = 100;
            for (int i = 0; i < coins_array.Length && i < MAX_SHOW; i++)
                Console.WriteLine($"{coins_array[i].Reference.PrevHash}:{coins_array[i].Reference.PrevIndex}");
            if (coins_array.Length > MAX_SHOW)
                Console.WriteLine($"({coins_array.Length - MAX_SHOW} more)");
            Console.WriteLine($"total: {coins_array.Length} UTXOs");
            return true;
        }

        protected internal override void OnStart(string[] args)
        {
            Blockchain.RegisterBlockchain(new LevelDBBlockchain(Settings.Default.DataDirectoryPath));
            if (File.Exists(PeerStatePath))
                using (FileStream fs = new FileStream(PeerStatePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    LocalNode.LoadState(fs);
                }

            // Load Verify Key
            string vk_path = ".//crypto//vk.key";
            string pk_path = ".//crypto//pk.key";

            int ret = SnarkDllApi.Snark_DllInit(1, vk_path.ToArray(), pk_path.ToArray());
            if (ret > 0)
            {
                Console.WriteLine($"Verify key loading success");
            }
            else
            {
                Console.WriteLine($"Verify key loading unseccess");
            }

            LocalNode = new LocalNode();
            Task.Run(() =>
            {
                const string acc_path = "chain.acc";
                const string acc_zip_path = acc_path + ".zip";
                if (File.Exists(acc_path))
                {
                    using (FileStream fs = new FileStream(acc_path, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        ImportBlocks(fs);
                    }
                    File.Delete(acc_path);
                }
                else if (File.Exists(acc_zip_path))
                {
                    using (FileStream fs = new FileStream(acc_zip_path, FileMode.Open, FileAccess.Read, FileShare.None))
                    using (ZipArchive zip = new ZipArchive(fs, ZipArchiveMode.Read))
                    using (Stream zs = zip.GetEntry(acc_path).Open())
                    {
                        ImportBlocks(zs);
                    }
                    File.Delete(acc_zip_path);
                }
                LocalNode.Start(Settings.Default.NodePort, Settings.Default.WsPort);
                bool isRPC = false;
                bool recordNotifications = false;
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "/rpc":
                        case "--rpc":
                        case "-r":
                            isRPC = true;
                            break;
                        case "--record-notifications":
                            recordNotifications = true;
                            break;
                    }
                }

                if (rpc == null)
                {
                    rpc = new RpcServerWithWallet(LocalNode, isRPC);
                    rpc.Start(Settings.Default.UriPrefix.OfType<string>().ToArray(), Settings.Default.SslCert, Settings.Default.SslCertPassword);
                }
                if (recordNotifications)
                    Blockchain.Notify += Blockchain_Notify;
            });
        }

        private bool OnStartCommand(string[] args)
        {
            switch (args[1].ToLower())
            {
                case "consensus":
                    return OnStartConsensusCommand(args);
                default:
                    return base.OnCommand(args);
            }
        }

        private bool OnStartConsensusCommand(string[] args)
        {
            if (consensus != null) return true;
            if (Program.Wallet == null)
            {
                Console.WriteLine("You have to open the wallet first.");
                return true;
            }
            string log_dictionary = Path.Combine(AppContext.BaseDirectory, "Logs");
            consensus = new ConsensusWithPolicy(LocalNode, Program.Wallet, log_dictionary);
            ShowPrompt = false;
            consensus.Start();
            return true;
        }

        protected internal override void OnStop()
        {
            if (consensus != null) consensus.Dispose();
            if (rpc != null) rpc.Dispose();
            LocalNode.Dispose();
            using (FileStream fs = new FileStream(PeerStatePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                LocalNode.SaveState(fs);
            }
            Blockchain.Default.Dispose();
        }

        private bool OnUpgradeCommand(string[] args)
        {
            switch (args[1].ToLower())
            {
                case "wallet":
                    return OnUpgradeWalletCommand(args);
                default:
                    return base.OnCommand(args);
            }
        }

        private bool OnUpgradeWalletCommand(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("error");
                return true;
            }
            string path = args[2];
            if (!File.Exists(path))
            {
                Console.WriteLine("File does not exist");
                return true;
            }
            string path_old = Path.ChangeExtension(path, ".old.db3");
            string path_new = Path.ChangeExtension(path, ".new.db3");
            UserWallet.Migrate(path, path_new);
            File.Move(path, path_old);
            File.Move(path_new, path);
            Console.WriteLine($"Wallet file upgrade complete. Old file has been auto-saved at: {path_old}");
            return true;
        }

        private void Blockchain_Notify(object sender, BlockNotifyEventArgs e)
        {
            JArray jArray = new JArray(e.Notifications.Select(p =>
            {
                JObject json = new JObject();
                json["txid"] = ((Transaction)p.ScriptContainer).Hash.ToString();
                json["contract"] = p.ScriptHash.ToString();
                json["state"] = p.State.ToParameter().ToJson();
                return json;
            }));
            string path = Path.Combine(AppContext.BaseDirectory, "Notifications");
            Directory.CreateDirectory(path);
            path = Path.Combine(path, $"block-{e.Block.Index}.json");
            File.WriteAllText(path, jArray.ToString());
        }
    }
}
