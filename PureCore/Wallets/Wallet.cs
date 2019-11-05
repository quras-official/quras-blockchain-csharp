using Pure.Core;
using Pure.Cryptography;
using Pure.IO.Caching;
using Pure.IO;
using Pure.SmartContract;
using Pure.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

using Pure.Wallets.JsonWallet;
using Pure.Wallets.StealthKey;
using Pure.Core.Anonoymous;
using Pure.Core.RingCT.Types;
using Pure.Core.RingCT.Impls;
using PureCore.Wallets.AnonymousKey.Key;

namespace Pure.Wallets
{
    public abstract class Wallet : IDisposable
    {
        public event EventHandler BalanceChanged;

        public static readonly byte AnonymouseDiff = 1;
        public static readonly byte AddressVersion = Settings.Default.AddressVersion;
        public static readonly byte AnonymouseAddressVersion = Settings.Default.AnonymousAddressVersion;
        public static readonly byte StealthAddressVersion = Settings.Default.StealthAddressVersion;
        public static readonly byte RingSize = Settings.Default.StealthAddressRingSize;

        public event EventHandler<string> ErrorsOccured;

        private readonly string path;
        private readonly byte[] iv;
        private readonly byte[] masterKey;
        private readonly Dictionary<UInt160, KeyPairBase> keys;
        private readonly Dictionary<UInt160, VerificationContract> contracts;
        private readonly HashSet<UInt160> watchOnly;
        private readonly TrackableCollection<CoinReference, Coin> coins;
        private readonly TrackableCollection<JSCoinReference, JSCoin> jscoins;
        private readonly TrackableCollection<RCTCoinReference, RCTCoin> rctcoins;
        private readonly List<RCTCoin> rctcoinCache;
        private readonly IntPtr cmMerkleTree;

        private uint current_height;

        private static readonly Random rand = new Random();
        private readonly Thread thread;
        private bool isrunning = true;

        protected string DbPath => path;
        protected object SyncRoot { get; } = new object();
        public uint WalletHeight => current_height;
        protected abstract Version Version { get; }

        private Wallet(string path, byte[] passwordKey, bool create, bool checkpwd)
        {
            if (checkpwd == false)
            {
                this.path = path;
                if (create)
                {
                    this.iv = new byte[16];
                    this.masterKey = new byte[32];
                    this.keys = new Dictionary<UInt160, KeyPairBase>();
                    this.contracts = new Dictionary<UInt160, VerificationContract>();
                    this.watchOnly = new HashSet<UInt160>();
                    this.coins = new TrackableCollection<CoinReference, Coin>();
                    this.jscoins = new TrackableCollection<JSCoinReference, JSCoin>();
                    this.rctcoins = new TrackableCollection<RCTCoinReference, RCTCoin>();
                    this.rctcoinCache = new List<RCTCoin>();
                    this.current_height = Blockchain.Default?.HeaderHeight + 1 ?? 0;
                    //this.current_height = 0;
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(iv);
                        rng.GetBytes(masterKey);
                    }
                    BuildDatabase();
                    SaveStoredData("PasswordHash", passwordKey.Sha256());
                    SaveStoredData("IV", iv);
                    SaveStoredData("MasterKey", masterKey.AesEncrypt(passwordKey, iv));
                    SaveStoredData("Version", new[] { Version.Major, Version.Minor, Version.Build, Version.Revision }.Select(p => BitConverter.GetBytes(p)).SelectMany(p => p).ToArray());
                    SaveStoredData("Height", BitConverter.GetBytes(current_height));

                    cmMerkleTree = SnarkDllApi.CmMerkleTree_Create();
                    cmMerkleTree = Blockchain.Default.GetCmMerkleTree();
                    SaveCmMerkleTree();
#if NET461
                ProtectedMemory.Protect(masterKey, MemoryProtectionScope.SameProcess);
#endif
                }
                else
                {
                    try
                    {
                        byte[] passwordHash = LoadStoredData("PasswordHash");
                        if (passwordHash != null && !passwordHash.SequenceEqual(passwordKey.Sha256()))
                            throw new CryptographicException();
                        this.iv = LoadStoredData("IV");
                        this.masterKey = LoadStoredData("MasterKey").AesDecrypt(passwordKey, iv);
#if NET461
                ProtectedMemory.Protect(masterKey, MemoryProtectionScope.SameProcess);
#endif
                        this.keys = LoadKeyPairs().ToDictionary(p => p.PublicKeyHash);
                        this.contracts = LoadContracts().ToDictionary(p => p.ScriptHash);
                        this.watchOnly = new HashSet<UInt160>(LoadWatchOnly());
                        this.coins = new TrackableCollection<CoinReference, Coin>(LoadCoins());
                        this.jscoins = new TrackableCollection<JSCoinReference, JSCoin>(LoadJSCoins());
                        this.rctcoins = new TrackableCollection<RCTCoinReference, RCTCoin>(LoadRCTCoins());
                        this.rctcoinCache = new List<RCTCoin>();
                        this.current_height = LoadStoredData("Height").ToUInt32(0);

                        cmMerkleTree = SnarkDllApi.CmMerkleTree_Create();

                        LoadCmMerkleTree();
                    }
                    catch (FormatException ex)
                    {
                        throw new FormatException(ex.Message);
                    }
                }
                Array.Clear(passwordKey, 0, passwordKey.Length);
                this.thread = new Thread(ProcessBlocks);
                this.thread.IsBackground = true;
                this.thread.Name = "Wallet.ProcessBlocks";
                this.thread.Start();
            }
            else
            {
                this.path = path;
                if (create)
                {
                    this.iv = new byte[16];
                    this.masterKey = new byte[32];
                    this.keys = new Dictionary<UInt160, KeyPairBase>();
                    this.contracts = new Dictionary<UInt160, VerificationContract>();
                    this.watchOnly = new HashSet<UInt160>();
                    this.coins = new TrackableCollection<CoinReference, Coin>();
                    this.jscoins = new TrackableCollection<JSCoinReference, JSCoin>();
                    this.rctcoins = new TrackableCollection<RCTCoinReference, RCTCoin>();
                    this.rctcoinCache = new List<RCTCoin>();
                    this.current_height = Blockchain.Default?.HeaderHeight + 1 ?? 0;
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(iv);
                        rng.GetBytes(masterKey);
                    }
                    BuildDatabase();
                    SaveStoredData("PasswordHash", passwordKey.Sha256());
                    SaveStoredData("IV", iv);
                    SaveStoredData("MasterKey", masterKey.AesEncrypt(passwordKey, iv));
                    SaveStoredData("Version", new[] { Version.Major, Version.Minor, Version.Build, Version.Revision }.Select(p => BitConverter.GetBytes(p)).SelectMany(p => p).ToArray());
                    SaveStoredData("Height", BitConverter.GetBytes(current_height));
#if NET461
                ProtectedMemory.Protect(masterKey, MemoryProtectionScope.SameProcess);
#endif
                }
                else
                {
                    try
                    {
                        byte[] passwordHash = LoadStoredData("PasswordHash");
                        if (passwordHash != null && !passwordHash.SequenceEqual(passwordKey.Sha256()))
                            throw new CryptographicException();
                        this.iv = LoadStoredData("IV");
                        this.masterKey = LoadStoredData("MasterKey").AesDecrypt(passwordKey, iv);
#if NET461
                ProtectedMemory.Protect(masterKey, MemoryProtectionScope.SameProcess);
#endif
                        this.keys = LoadKeyPairs().ToDictionary(p => p.PublicKeyHash);
                        this.contracts = LoadContracts().ToDictionary(p => p.ScriptHash);
                        this.watchOnly = new HashSet<UInt160>(LoadWatchOnly());
                        this.coins = new TrackableCollection<CoinReference, Coin>(LoadCoins());
                        this.jscoins = new TrackableCollection<JSCoinReference, JSCoin>(LoadJSCoins());
                        this.rctcoins = new TrackableCollection<RCTCoinReference, RCTCoin>(LoadRCTCoins());
                        this.rctcoinCache = new List<RCTCoin>();
                        this.current_height = LoadStoredData("Height").ToUInt32(0);
                    }
                    catch (FormatException ex)
                    {
                        throw new FormatException(ex.Message);
                    }
                }
            }
        }

        protected Wallet(string path, string password, bool create, bool checkpwd)
            : this(path, password.ToAesKey(), create, checkpwd)
        {
        }

        protected Wallet(string path, SecureString password, bool create, bool checkpwd)
            : this(path, password.ToAesKey(), create, checkpwd)
        {
        }

        public virtual void AddContract(VerificationContract contract)
        {
            lock (keys)
            {
                if (!keys.ContainsKey(contract.PublicKeyHash))
                    throw new InvalidOperationException();
                lock (contracts)
                    lock (watchOnly)
                    {
                        contracts[contract.ScriptHash] = contract;
                        watchOnly.Remove(contract.ScriptHash);
                    }
            }
        }

        public virtual void LoadCmMerkleTree()
        {
            byte[] byMerkletree = LoadStoredData("CmMerkleTree");
            SnarkDllApi.SetCMTreeFromBinary(cmMerkleTree, byMerkletree, byMerkletree.Length);
        }

        public virtual void SaveCmMerkleTree()
        {
            int[] out_len = new int[1];
            IntPtr pTree = SnarkDllApi.GetCMTreeInBinary(cmMerkleTree, out_len);

            byte[] byTree = new byte[out_len[0]];
            System.Runtime.InteropServices.Marshal.Copy(pTree, byTree, 0, out_len[0]);

            SaveStoredData("CmMerkleTree", byTree);
        }

        public virtual void AddWatchOnly(UInt160 scriptHash)
        {
            lock (contracts)
            {
                if (contracts.ContainsKey(scriptHash))
                    return;
                lock (watchOnly)
                {
                    watchOnly.Add(scriptHash);
                }
            }
        }

        protected virtual void BuildDatabase()
        {
        }

        public bool ChangePassword(string password_old, string password_new)
        {
            if (!VerifyPassword(password_old)) return false;
            byte[] passwordKey = password_new.ToAesKey();
#if NET461
            using (new ProtectedMemoryContext(masterKey, MemoryProtectionScope.SameProcess))
#endif
            {
                try
                {
                    SaveStoredData("PasswordHash", passwordKey.Sha256());
                    SaveStoredData("MasterKey", masterKey.AesEncrypt(passwordKey, iv));
                    return true;
                }
                finally
                {
                    Array.Clear(passwordKey, 0, passwordKey.Length);
                }
            }
        }

        private AddressState CheckAddressState(UInt160 scriptHash)
        {
            lock (contracts)
            {
                if (contracts.ContainsKey(scriptHash))
                    return AddressState.InWallet;
            }
            lock (watchOnly)
            {
                if (watchOnly.Contains(scriptHash))
                    return AddressState.InWallet | AddressState.WatchOnly;
            }
            return AddressState.None;
        }

        public bool ContainsKey(Cryptography.ECC.ECPoint publicKey)
        {
            return ContainsKey(publicKey.EncodePoint(true).ToScriptHash());
        }

        public bool ContainsKey(UInt160 publicKeyHash)
        {
            lock (keys)
            {
                return keys.ContainsKey(publicKeyHash);
            }
        }

        public bool ContainsAddress(UInt160 scriptHash)
        {
            return CheckAddressState(scriptHash).HasFlag(AddressState.InWallet);
        }

        public KeyPairBase CreateKey(KeyType nVersion = KeyType.Transparent)
        {
            if (nVersion == KeyType.Transparent)
            {
                byte[] privateKey = new byte[32];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(privateKey);
                }
                KeyPair key = CreateKey(privateKey, nVersion);
                Array.Clear(privateKey, 0, privateKey.Length);
                return key;
            }
            else if (nVersion == KeyType.Anonymous)
            {
                SpendingKey spendingKey = SpendingKey.random();
                KeyPair key = CreateKey(spendingKey.ToArray(), nVersion);
                return key;
            }
            else if (nVersion == KeyType.Stealth)
            {
                byte[] payloadPrivKey = new byte[32];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(payloadPrivKey);
                }

                byte[] viewPrivKey = new byte[32];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(viewPrivKey);
                }

                StealthKeyPair key = CreateKey(payloadPrivKey, viewPrivKey);
                return key;
            }
            else
            {
                return null;
            }
        }

        public virtual StealthKeyPair CreateKey(byte[] payloadPrivKey, byte[] viewPrivKey)
        {
            StealthKeyPair key = new StealthKeyPair(payloadPrivKey, viewPrivKey);

            lock (keys)
            {
                keys[key.PublicKeyHash] = key;
            }
            return key;
        }

        public virtual KeyPair CreateKey(byte[] privateKey, KeyType nVersion = KeyType.Transparent)
        {
            if (nVersion == KeyType.Transparent)
            {
                KeyPair key = new KeyPair(privateKey, nVersion);
                lock (keys)
                {
                    keys[key.PublicKeyHash] = key;
                }
                return key;
            }
            else if (nVersion == KeyType.Anonymous)
            {
                KeyPair key = new KeyPair(privateKey, nVersion);
                lock (keys)
                {
                    keys[key.PublicKeyHash] = key;
                }
                return key;
            }
            else
            {
                return null;
            }
        }

        protected byte[] DecryptPrivateKey(byte[] encryptedPrivateKey)
        {
            if (encryptedPrivateKey == null) throw new ArgumentNullException(nameof(encryptedPrivateKey));
            if (encryptedPrivateKey.Length != 96) throw new ArgumentException();
#if NET461
            using (new ProtectedMemoryContext(masterKey, MemoryProtectionScope.SameProcess))
#endif
            {
                return encryptedPrivateKey.AesDecrypt(masterKey, iv);
            }
        }

        public virtual bool DeleteKey(UInt160 publicKeyHash)
        {
            lock (keys)
            {
                lock (contracts)
                {
                    foreach (VerificationContract contract in contracts.Values.Where(p => p.PublicKeyHash == publicKeyHash).ToArray())
                    {
                        DeleteAddress(contract.ScriptHash);
                    }
                }
                return keys.Remove(publicKeyHash);
            }
        }

        public virtual bool DeleteAddress(UInt160 scriptHash)
        {
            lock (contracts)
                lock (watchOnly)
                    lock (coins)
                    {
                        foreach (CoinReference key in coins.Where(p => p.Output.ScriptHash == scriptHash).Select(p => p.Reference).ToArray())
                        {
                            coins.Remove(key);
                        }
                        coins.Commit();
                        return contracts.Remove(scriptHash) || watchOnly.Remove(scriptHash);
                    }
        }

        public virtual byte[] GetCmMerkleTreeInBytes()
        {
            int[] out_len = new int[1];
            IntPtr pTree = SnarkDllApi.GetCMTreeInBinary(cmMerkleTree, out_len);

            byte[] byTree = new byte[out_len[0]];
            System.Runtime.InteropServices.Marshal.Copy(pTree, byTree, 0, out_len[0]);

            return byTree;
        }

        public virtual void Dispose()
        {
            isrunning = false;
            if (!thread.ThreadState.HasFlag(ThreadState.Unstarted)) thread.Join();
        }

        protected byte[] EncryptPrivateKey(byte[] decryptedPrivateKey)
        {
#if NET461
            using (new ProtectedMemoryContext(masterKey, MemoryProtectionScope.SameProcess))
#endif
            {
                return decryptedPrivateKey.AesEncrypt(masterKey, iv);
            }
        }

        public IEnumerable<Coin> FindUnspentCoins()
        {
            return GetCoins().Where(p => p.State.HasFlag(
                                    CoinState.Confirmed) && 
                                    !p.State.HasFlag(CoinState.Spent) && 
                                    !p.State.HasFlag(CoinState.Locked) && 
                                    !p.State.HasFlag(CoinState.Frozen) && 
                                    !p.State.HasFlag(CoinState.WatchOnly));
        }

        public IEnumerable<Coin> FindUnspentCoinsFrom(string from_addr)
        {
            return GetCoins().Where(p => p.State.HasFlag(CoinState.Confirmed) &&
                                    !p.State.HasFlag(CoinState.Spent) &&
                                    !p.State.HasFlag(CoinState.Locked) &&
                                    !p.State.HasFlag(CoinState.Frozen) &&
                                    !p.State.HasFlag(CoinState.WatchOnly) &&
                                    p.Address == from_addr);
        }

        public virtual Coin[] FindUnspentCoins(UInt256 asset_id, Fixed8 amount, string from_addr = null)
        {
            if (from_addr == null)
                return FindUnspentCoins(FindUnspentCoins(), asset_id, amount);
            else
                return FindUnspentCoins(FindUnspentCoinsFrom(from_addr), asset_id, amount);
        }

        protected static Coin[] FindUnspentCoins(IEnumerable<Coin> unspents, UInt256 asset_id, Fixed8 amount)
        {
            Coin[] unspents_asset = unspents.Where(p => p.Output.AssetId == asset_id).ToArray();
            Fixed8 sum = unspents_asset.Sum(p => p.Output.Value);
            if (sum < amount) return null;
            if (sum == amount) return unspents_asset;
            Coin[] unspents_ordered = unspents_asset.OrderByDescending(p => p.Output.Value).ToArray();
            int i = 0;
            while (unspents_ordered[i].Output.Value <= amount)
                amount -= unspents_ordered[i++].Output.Value;
            if (amount == Fixed8.Zero)
                return unspents_ordered.Take(i).ToArray();
            else
                return unspents_ordered.Take(i).Concat(new[] { unspents_ordered.Last(p => p.Output.Value >= amount) }).ToArray();
        }

        public IEnumerable<JSCoin> FindUnspentNotesFrom(string from_addr)
        {
            return GetJSCoins().Where(p => p.State.HasFlag(CoinState.Confirmed) &&
                                    !p.State.HasFlag(CoinState.Spent) &&
                                    !p.State.HasFlag(CoinState.Locked) &&
                                    !p.State.HasFlag(CoinState.Frozen) &&
                                    !p.State.HasFlag(CoinState.WatchOnly) &&
                                    p.Address == from_addr);
        }

        public virtual JSCoin[] FindUnspentNotes(UInt256 asset_id, Fixed8 amount, string from_addr)
        {
            return FindUnspentNotes(FindUnspentNotesFrom(from_addr), asset_id, amount);
        }

        protected static JSCoin[] FindUnspentNotes(IEnumerable<JSCoin> unspents, UInt256 asset_id, Fixed8 amount)
        {
            JSCoin[] unspents_asset = unspents.Where(p => p.Output.AssetId == asset_id).ToArray();
            Fixed8 sum = unspents_asset.Sum(p => p.Output.Value);
            if (sum < amount) return null;
            if (sum == amount) return unspents_asset;
            JSCoin[] unspents_ordered = unspents_asset.OrderByDescending(p => p.Output.Value).ToArray();
            int i = 0;
            while (unspents_ordered[i].Output.Value <= amount)
                amount -= unspents_ordered[i++].Output.Value;
            if (amount == Fixed8.Zero)
                return unspents_ordered.Take(i).ToArray();
            else
                return unspents_ordered.Take(i).Concat(new[] { unspents_ordered.Last(p => p.Output.Value >= amount) }).ToArray();
        }

        public IEnumerable<RCTCoin> FindUnspentRCNotesFrom(StealthKeyPair fromKeyPair)
        {
            return GetRCTCoins().Where(p => p.State.HasFlag(CoinState.Confirmed) &&
                                        !p.State.HasFlag(CoinState.Spent) &&
                                        !p.State.HasFlag(CoinState.Locked) &&
                                        !p.State.HasFlag(CoinState.Frozen) &&
                                        !p.State.HasFlag(CoinState.WatchOnly) &&
                                        p.Output.PubKey.ToString() == Cryptography.ECC.ECPoint.DecodePoint(fromKeyPair.GetPaymentPubKeyFromR(p.Reference.TxRCTHash), Cryptography.ECC.ECCurve.Secp256r1).ToString());
        }

        protected static RCTCoin[] FindUnspentRCTNotes(IEnumerable<RCTCoin> unspents, UInt256 asset_id, Fixed8 amount)
        {
            RCTCoin[] unspents_asset = unspents.Where(p => p.Output.AssetId == asset_id).ToArray();
            Fixed8 sum = unspents_asset.Sum(p => p.Output.Value);

            if (sum < amount) return null;
            if (sum == amount) return unspents_asset;

            RCTCoin[] unspent_ordered = unspents_asset.OrderByDescending(p => p.Output.Value).ToArray();

            int i = 0;
            while (unspent_ordered[i].Output.Value <= amount)
                amount -= unspent_ordered[i++].Output.Value;

            if (amount == Fixed8.Zero)
                return unspent_ordered.Take(i).ToArray();
            else
                return unspent_ordered.Take(i).Concat(new[] { unspent_ordered.Last(p => p.Output.Value >= amount) }).ToArray();

        }

        public virtual RCTCoin[] FindUnspentRCTNotes(UInt256 asset_id, Fixed8 amount, StealthKeyPair fromKeyPair)
        {
            return FindUnspentRCTNotes(FindUnspentRCNotesFrom(fromKeyPair), asset_id, amount);
        }

        public KeyPairBase GetKey(Cryptography.ECC.ECPoint publicKey)
        {
            return GetKey(publicKey.EncodePoint(true).ToScriptHash());
        }

        public KeyPairBase GetKey(UInt160 publicKeyHash)
        {
            lock (keys)
            {
                keys.TryGetValue(publicKeyHash, out KeyPairBase key);
                return key;
            }
        }

        public KeyPairBase GetKeyByScriptHash(UInt160 scriptHash)
        {
            lock (keys)
                lock (contracts)
                {
                    return !contracts.TryGetValue(scriptHash, out VerificationContract contract) ? null : keys[contract.PublicKeyHash];
                }
        }

        public IEnumerable<KeyPairBase> GetKeys()
        {
            lock (keys)
            {
                foreach (var pair in keys)
                {
                    yield return pair.Value;
                }
            }
        }

        public IEnumerable<UInt160> GetAddresses()
        {
            lock (contracts)
            {
                foreach (var pair in contracts)
                    yield return pair.Key;
            }
            lock (watchOnly)
            {
                foreach (UInt160 hash in watchOnly)
                    yield return hash;
            }
        }

        public Fixed8 GetAvailable(UInt256 asset_id)
        {
            return FindUnspentCoins().Where(p => p.Output.AssetId.Equals(asset_id)).Sum(p => p.Output.Value);
        }

        public BigDecimal GetAvailable(UIntBase asset_id)
        {
            if (asset_id is UInt160 asset_id_160)
            {
                byte[] script;
                using (ScriptBuilder sb = new ScriptBuilder())
                {
                    foreach (UInt160 account in GetContracts().Select(p => p.ScriptHash))
                        sb.EmitAppCall(asset_id_160, "balanceOf", account);
                    sb.Emit(OpCode.DEPTH, OpCode.PACK);
                    sb.EmitAppCall(asset_id_160, "decimals");
                    script = sb.ToArray();
                }
                ApplicationEngine engine = ApplicationEngine.Run(script);
                byte decimals = (byte)engine.EvaluationStack.Pop().GetBigInteger();
                BigInteger amount = engine.EvaluationStack.Pop().GetArray().Aggregate(BigInteger.Zero, (x, y) => x + y.GetBigInteger());
                return new BigDecimal(amount, decimals);
            }
            else
            {
                return new BigDecimal(GetAvailable((UInt256)asset_id).GetData(), 8);
            }
        }

        public Fixed8 GetBalance(UInt256 asset_id)
        {
            return GetCoins().Where(p => !p.State.HasFlag(CoinState.Spent) && p.Output.AssetId.Equals(asset_id)).Sum(p => p.Output.Value);
        }

        public virtual UInt160 GetChangeAddress()
        {
            lock (contracts)
            {
                return contracts.Values.FirstOrDefault(p => p.IsStandard)?.ScriptHash ?? contracts.Keys.FirstOrDefault();
            }
        }

        public virtual PaymentAddress GetChangeAddressAsPaymentAddress()
        {
            lock (contracts)
            {
                return contracts[contracts.Values.FirstOrDefault(p => p.IsStandard)?.ScriptHash ?? contracts.Keys.FirstOrDefault()].paymentAddress;
            }
        }

        public IEnumerable<Coin> GetCoins()
        {
            lock (coins)
            {
                foreach (Coin coin in coins)
                    yield return coin;
            }
        }

        public IEnumerable<JSCoin> GetJSCoins()
        {
            lock (jscoins)
            {
                foreach (JSCoin coin in jscoins)
                    yield return coin;
            }
        }

        public IEnumerable<RCTCoin> GetRCTCoins()
        {
            lock (rctcoins)
            {
                foreach (RCTCoin coin in rctcoins)
                    yield return coin;
            }
        }

        public List<RCTCoin> GetRCTCoinCache()
        {
            lock (rctcoinCache)
            {
                return rctcoinCache;
            }
        }

        public VerificationContract GetContract(UInt160 scriptHash)
        {
            lock (contracts)
            {
                contracts.TryGetValue(scriptHash, out VerificationContract contract);
                return contract;
            }
        }

        public IEnumerable<VerificationContract> GetContracts()
        {
            lock (contracts)
            {
                foreach (var pair in contracts)
                {
                    yield return pair.Value;
                }
            }
        }

        public IEnumerable<VerificationContract> GetContracts(UInt160 publicKeyHash)
        {
            lock (contracts)
            {
                foreach (VerificationContract contract in contracts.Values.Where(p => p.PublicKeyHash.Equals(publicKeyHash)))
                {
                    yield return contract;
                }
            }
        }

        public static byte[] GetPrivateKeyFromNEP2(string nep2, string passphrase)
        {
            if (nep2 == null) throw new ArgumentNullException(nameof(nep2));
            if (passphrase == null) throw new ArgumentNullException(nameof(passphrase));
            byte[] data = nep2.Base58CheckDecode();
            if (data.Length != 39 || data[0] != 0x00 || data[1] != 0x02 || data[2] != 0x19)
                throw new FormatException();
            byte[] addresshash = new byte[4];
            Buffer.BlockCopy(data, 3, addresshash, 0, 4);
            byte[] derivedkey = SCrypt.DeriveKey(Encoding.UTF8.GetBytes(passphrase), addresshash, 16384, 8, 8, 64);
            byte[] derivedhalf1 = derivedkey.Take(32).ToArray();
            byte[] derivedhalf2 = derivedkey.Skip(32).ToArray();
            byte[] encryptedkey = new byte[32];
            Buffer.BlockCopy(data, 7, encryptedkey, 0, 32);
            byte[] prikey = XOR(encryptedkey.AES256Decrypt(derivedhalf2), derivedhalf1);
            Cryptography.ECC.ECPoint pubkey = Cryptography.ECC.ECCurve.Secp256r1.G * prikey;
            UInt160 script_hash = Contract.CreateSignatureRedeemScript(pubkey).ToScriptHash();
            string address = ToAddress(script_hash);
            if (!Encoding.ASCII.GetBytes(address).Sha256().Sha256().Take(4).SequenceEqual(addresshash))
                throw new FormatException();
            return prikey;
        }

        public static byte[] GetPrivateKeyFromWIF(string wif)
        {
            if (wif == null) throw new ArgumentNullException();
            byte[] data = wif.Base58CheckDecode();
            if (data.Length != 34 || data[0] != 0x80 || data[33] != 0x01)
                throw new FormatException();
            byte[] privateKey = new byte[32];
            Buffer.BlockCopy(data, 1, privateKey, 0, privateKey.Length);
            Array.Clear(data, 0, data.Length);
            return privateKey;
        }

        public static bool ExportJsonPrivateKeyFile(string path, byte[] privateKey, string password, KeyType version)
        {
            byte[] iv = new byte[16];
            byte[] masterKey = new byte[32];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(iv);
                rng.GetBytes(masterKey);
            }

            byte[] passwordKey = password.ToAesKey();
            byte[] passwordHash = passwordKey.Sha256();

            byte[] EncryptedMasterKey = masterKey.AesEncrypt(passwordKey, iv);

            byte[] EncryptedPrivKey = privateKey.AesEncrypt(masterKey, iv);

            JsonWalletStructure data = new JsonWalletStructure();

            data.key_type = version.ToString();
            data.p1_hash = passwordHash.ToHexString();
            data.p2_hash = iv.ToHexString();
            data.p3_hash = EncryptedMasterKey.ToHexString();
            data.p4_hash = EncryptedPrivKey.ToHexString();

            string json = JsonConvert.SerializeObject(data);
            System.IO.File.WriteAllText(path, json);

            return true;
        }

        public abstract IEnumerable<T> GetTransactions<T>() where T : Transaction;

        public IEnumerable<Coin> GetUnclaimedCoins()
        {
            lock (coins)
            {
                foreach (var coin in coins)
                {
                    if (!coin.Output.AssetId.Equals(Blockchain.GoverningToken.Hash)) continue;
                    if (!coin.State.HasFlag(CoinState.Confirmed)) continue;
                    if (!coin.State.HasFlag(CoinState.Spent)) continue;
                    if (coin.State.HasFlag(CoinState.Claimed)) continue;
                    if (coin.State.HasFlag(CoinState.Frozen)) continue;
                    if (coin.State.HasFlag(CoinState.WatchOnly)) continue;
                    yield return coin;
                }
            }
        }

        public KeyPair Import(X509Certificate2 cert)
        {
            byte[] privateKey;
            using (ECDsa ecdsa = cert.GetECDsaPrivateKey())
            {
#if NET461
                privateKey = ((ECDsaCng)ecdsa).Key.Export(CngKeyBlobFormat.EccPrivateBlob);
#else
                privateKey = ecdsa.ExportParameters(true).D;
#endif
            }
            KeyPair key = CreateKey(privateKey);
            Array.Clear(privateKey, 0, privateKey.Length);
            return key;
        }

        public KeyPair Import(string wif)
        {
            byte[] privateKey = GetPrivateKeyFromWIF(wif);
            KeyPair key = CreateKey(privateKey);
            Array.Clear(privateKey, 0, privateKey.Length);
            return key;
        }

        public KeyPair Import(string nep2, string passphrase)
        {
            byte[] privateKey = GetPrivateKeyFromNEP2(nep2, passphrase);
            KeyPair key = CreateKey(privateKey);
            Array.Clear(privateKey, 0, privateKey.Length);
            return key;
        }

        protected bool IsWalletTransaction(Transaction tx)
        {
            lock (contracts)
            {
                if (tx.Outputs.Any(p => contracts.ContainsKey(p.ScriptHash)))
                    return true;
                if (tx.Scripts.Any(p => p.VerificationScript != null && contracts.ContainsKey(p.VerificationScript.ToScriptHash())))
                    return true;
            }
            lock (watchOnly)
            {
                if (tx.Outputs.Any(p => watchOnly.Contains(p.ScriptHash)))
                    return true;
                if (tx.Scripts.Any(p => p.VerificationScript != null && watchOnly.Contains(p.VerificationScript.ToScriptHash())))
                    return true;
            }

            if (tx is AnonymousContractTransaction ctx)
            {
                for (int jsIndex = 0; jsIndex < ctx.byJoinSplit.Count; jsIndex ++)
                {
                    foreach (KeyPairBase basekey in GetKeys())
                    {
                        if (basekey is StealthKeyPair)
                            continue;
                        KeyPair key = (KeyPair)basekey;
                        if (key.nVersion == KeyType.Anonymous)
                        {
                            using (key.Decrypt())
                            {
                                IntPtr byRet = SnarkDllApi.Snark_FindMyNotes(ctx.byJoinSplit[jsIndex], key.PrivateKey, ctx.joinSplitPubKey.ToArray());

                                byte[] byCount = new byte[1];
                                System.Runtime.InteropServices.Marshal.Copy(byRet, byCount, 0, 1);
                                if (byCount[0] > 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            if (tx is RingConfidentialTransaction rctx)
            {
                bool bResult = false;
                for (int i = 0; i < rctx.RingCTSig.Count; i++)
                {
                    foreach (KeyPairBase key in GetKeys())
                    {
                        if (key is StealthKeyPair rctKey)
                        {
                            for (int j = 0; j < rctx.RingCTSig[i].outPK.Count; j++)
                            {
                                if (rctx.RingCTSig[i].outPK[j].dest.ToString() == Cryptography.ECC.ECPoint.DecodePoint(rctKey.GetPaymentPubKeyFromR(rctx.RHashKey), Cryptography.ECC.ECCurve.Secp256r1).ToString())
                                {
                                    bResult = true;
                                    return bResult;
                                }
                                else
                                    bResult = false;
                            }
                        }
                    }
                }

                if (bResult == true)
                    return true;
            }
            return false;
        }

        protected abstract IEnumerable<KeyPairBase> LoadKeyPairs();
        protected abstract IEnumerable<Coin> LoadCoins();
        protected abstract IEnumerable<JSCoin> LoadJSCoins();
        protected abstract IEnumerable<RCTCoin> LoadRCTCoins();

        protected abstract IEnumerable<VerificationContract> LoadContracts();

        protected abstract byte[] LoadStoredData(string name);

        protected virtual IEnumerable<UInt160> LoadWatchOnly()
        {
            return Enumerable.Empty<UInt160>();
        }

        public RingConfidentialTransaction MakeRCTransaction(RingConfidentialTransaction tx, string from_addr, List<RCTransactionOutput> rctOutput, StealthKeyPair fromKeyPair, byte[] r, Fixed8 fee = default(Fixed8), UInt160 change_address = null)
        {
            if (tx.Outputs == null) tx.Outputs = new TransactionOutput[0];
            if (tx.Attributes == null) tx.Attributes = new TransactionAttribute[0];
            if (tx.Inputs == null) tx.Inputs = new CoinReference[0];

            byte fromAddressType = GetAddressVersion(from_addr);
            Fixed8 sysFee = Fixed8.Zero;
            Fixed8 qrsSysFee = Fixed8.Zero;
            RingConfidentialTransactionType tx_type;

            tx.RHashKey = Cryptography.ECC.ECCurve.Secp256r1.G * r;

            #region Define Tx_Type
            if (fromAddressType == StealthAddressVersion)
            {
                if (tx.Outputs.Length > 0 && rctOutput.Count > 0)
                {
                    tx_type = RingConfidentialTransactionType.S_ST_Transaction;
                }
                else if (tx.Outputs.Length > 0)
                {
                    tx_type = RingConfidentialTransactionType.S_T_Transaction;
                }
                else if (rctOutput.Count > 0)
                {
                    tx_type = RingConfidentialTransactionType.S_S_Transaction;
                }
                else
                {
                    tx_type = RingConfidentialTransactionType.NONE;
                }
            }
            else if (fromAddressType == AddressVersion)
            {
                if (tx.Outputs.Length > 0 && rctOutput.Count > 0)
                {
                    tx_type = RingConfidentialTransactionType.T_ST_Transaction;
                }
                else if (tx.Outputs.Length > 0)
                {
                    throw new Exception("This tx is not Ring Confidential Tx");
                }
                else if (rctOutput.Count > 0)
                {
                    tx_type = RingConfidentialTransactionType.T_S_Transaction;
                }
                else
                {
                    tx_type = RingConfidentialTransactionType.NONE;
                }
            }
            else if (fromAddressType == AnonymouseAddressVersion)
            {
                throw new Exception("This tx dosn't support anonymous address");
            }
            else
            {
                tx_type = RingConfidentialTransactionType.NONE;
            }
            #endregion

            #region Calculate the Fee

            // Check the Asset ID
            for (int i = 0; i < tx.Outputs.Length; i++)
            {
                if (tx.Outputs[i].AssetId == Blockchain.GoverningToken.Hash)
                {
                    switch (tx_type)
                    {
                        case RingConfidentialTransactionType.S_S_Transaction:
                        case RingConfidentialTransactionType.S_T_Transaction:
                        case RingConfidentialTransactionType.S_ST_Transaction:
                            qrsSysFee = Blockchain.UtilityToken.A_Fee;
                            break;
                        case RingConfidentialTransactionType.T_S_Transaction:
                        case RingConfidentialTransactionType.T_ST_Transaction:
                            qrsSysFee = Blockchain.UtilityToken.A_Fee;
                            break;
                        default:
                            qrsSysFee = Fixed8.Zero;
                            break;
                    }
                }
                else
                {
                    switch (tx_type)
                    {
                        case RingConfidentialTransactionType.S_S_Transaction:
                        case RingConfidentialTransactionType.S_T_Transaction:
                        case RingConfidentialTransactionType.S_ST_Transaction:
                            sysFee = Blockchain.UtilityToken.A_Fee;
                            break;
                        case RingConfidentialTransactionType.T_S_Transaction:
                        case RingConfidentialTransactionType.T_ST_Transaction:
                            sysFee = Blockchain.UtilityToken.A_Fee;
                            break;
                        default:
                            sysFee = Fixed8.Zero;
                            break;
                    }
                }
            }

            for (int i = 0; i < rctOutput.Count; i++)
            {
                if (rctOutput[i].AssetId == Blockchain.GoverningToken.Hash)
                {
                    switch (tx_type)
                    {
                        case RingConfidentialTransactionType.S_S_Transaction:
                        case RingConfidentialTransactionType.S_T_Transaction:
                        case RingConfidentialTransactionType.S_ST_Transaction:
                            qrsSysFee = Blockchain.UtilityToken.A_Fee;
                            break;
                        case RingConfidentialTransactionType.T_S_Transaction:
                        case RingConfidentialTransactionType.T_ST_Transaction:
                            qrsSysFee = Blockchain.UtilityToken.A_Fee;
                            break;
                        default:
                            qrsSysFee = Fixed8.Zero;
                            break;
                    }
                }
                else
                {
                    switch (tx_type)
                    {
                        case RingConfidentialTransactionType.S_S_Transaction:
                        case RingConfidentialTransactionType.S_T_Transaction:
                        case RingConfidentialTransactionType.S_ST_Transaction:
                            sysFee = Blockchain.UtilityToken.A_Fee;
                            break;
                        case RingConfidentialTransactionType.T_S_Transaction:
                        case RingConfidentialTransactionType.T_ST_Transaction:
                            sysFee = Blockchain.UtilityToken.A_Fee;
                            break;
                        default:
                            sysFee = Fixed8.Zero;
                            break;
                    }
                }
            }
            #endregion

            #region Calculate the total payment amount
            // Calculate the tx_output value
            var output_pay_total = tx.Outputs.GroupBy(p => p.AssetId, (k, g) => new
            {
                AssetId = k,
                Value = g.Sum(p => p.Value)
            }).ToDictionary(p => p.AssetId);

            var rctoutput_pay_total = rctOutput.GroupBy(p => p.AssetId, (k, g) => new
            {
                AssetId = k,
                Value = g.Sum(p => p.Value)
            }).ToDictionary(p => p.AssetId);


            // Constructthe pay_total
            Dictionary<UInt256, Fixed8> pay_total = new Dictionary<UInt256, Fixed8>();
            
            foreach(UInt256 key in output_pay_total.Keys)
            {
                if (pay_total.ContainsKey(key))
                {
                    pay_total[key] += output_pay_total[key].Value;
                }
                else
                {
                    pay_total[key] = output_pay_total[key].Value;
                }
            }

            foreach (UInt256 key in rctoutput_pay_total.Keys)
            {
                if (pay_total.ContainsKey(key))
                {
                    pay_total[key] += rctoutput_pay_total[key].Value;
                }
                else
                {
                    pay_total[key] = rctoutput_pay_total[key].Value;
                }
            }

            if (qrsSysFee > Fixed8.Zero)
            {
                if (pay_total.ContainsKey(Blockchain.UtilityToken.Hash))
                {
                    pay_total[Blockchain.UtilityToken.Hash] += qrsSysFee;
                }
                else
                {
                    pay_total[Blockchain.UtilityToken.Hash] = qrsSysFee;
                }
            }

            if (sysFee > Fixed8.Zero)
            {
                if (pay_total.ContainsKey(Blockchain.UtilityToken.Hash))
                {
                    pay_total[Blockchain.UtilityToken.Hash] += sysFee;
                }
                else
                {
                    pay_total[Blockchain.UtilityToken.Hash] = sysFee;
                }
            }
            #endregion

            #region Get the Input Address & Amount
            if (fromAddressType == StealthAddressVersion)
            {
                var pay_coins = pay_total.Select(p => new
                {
                    AssetId = p.Key,
                    Unspents = FindUnspentRCTNotes(p.Key, p.Value, fromKeyPair)
                }).ToDictionary(p => p.AssetId);

                if (pay_coins.Any(p => p.Value.Unspents == null)) return null;

                var input_sum = pay_coins.Values.ToDictionary(p => p.AssetId, p => new
                {
                    AssetId = p.AssetId,
                    Value = p.Unspents.Sum(q => q.Output.Value)
                });

                List<RCTransactionOutput> outputs_new = rctOutput;
                foreach (UInt256 asset_id in input_sum.Keys)
                {
                    if (input_sum[asset_id].Value > pay_total[asset_id])
                    {
                        outputs_new.Add(new RCTransactionOutput
                        {
                            AssetId = asset_id,
                            Value = input_sum[asset_id].Value - pay_total[asset_id],
                            PubKey = Cryptography.ECC.ECPoint.DecodePoint(fromKeyPair.GenPaymentPubKeyHash(r), Cryptography.ECC.ECCurve.Secp256r1),
                            ScriptHash = Contract.CreateRingSignatureRedeemScript(fromKeyPair.PayloadPubKey, fromKeyPair.ViewPubKey).ToScriptHash()
                        });
                    }
                }

                //Fixed8 fee_amount = (qrsSysFee > sysFee ? qrsSysFee : sysFee );

                foreach(var key in pay_coins.Keys)
                {
                    List<CTCommitment> inSK = new List<CTCommitment>();
                    List<CTKey> inPK = new List<CTKey>();
                    List<MixRingCTKey> inPKIndex = new List<MixRingCTKey>();
                    List<Cryptography.ECC.ECPoint> destinations = new List<Cryptography.ECC.ECPoint>();
                    List<Fixed8> amounts = new List<Fixed8>();

                    // 01. Construct inSK and pubs_index

                    byte[] out_amount = new byte[32];
                    byte[] out_mask = new byte[32];
                    var asset_outputs = outputs_new.Where(p => p.AssetId == key);

                    for (int i = 0; i < pay_coins[key].Unspents.Length; i++)
                    {
                        RCTCoin coin = pay_coins[key].Unspents[i];

                        byte[] privKey = fromKeyPair.GenOneTimePrivKey(coin.Reference.TxRCTHash);
                        Cryptography.ECC.ECPoint pubKey = coin.Output.PubKey;

                        byte[] mask;
                        Cryptography.ECC.ECPoint C_Target;

                        if (Blockchain.Default.GetTransaction(coin.Reference.PrevHash) is RingConfidentialTransaction refRtx)
                        {
                            if (pay_coins[key].Unspents[i].Output.Value != RingCTSignature.DecodeRct(refRtx.RingCTSig[coin.Reference.PrevRCTSigId], privKey, coin.Reference.PrevRCTSigIndex, out mask))
                            {
                                return null;
                            }

                            C_Target = refRtx.RingCTSig[coin.Reference.PrevRCTSigId].outPK[coin.Reference.PrevRCTSigIndex].mask;
                        }
                        else
                        {
                            return null;
                        }

                        Fixed8 amount = pay_coins[key].Unspents[i].Output.Value;
                        byte[] b_amount = amount.ToBinaryFormat().ToBinary();

                        Cryptography.ECC.ECPoint C_i_in = RingCTSignature.GetCommitment(mask, b_amount);

                        if (C_Target.ToString() != C_i_in.ToString())
                        {
                            return null;
                        }

                        out_amount = ScalarFunctions.Add(out_amount, b_amount);
                        out_mask = ScalarFunctions.Add(out_mask, mask);

                        CTKey ctKey = new CTKey(pubKey, C_i_in);
                        MixRingCTKey mixRingCtKey = new MixRingCTKey(coin.Reference.PrevHash, (byte)coin.Reference.PrevRCTSigId, (byte)coin.Reference.PrevRCTSigIndex);

                        CTCommitment i_insk = new CTCommitment(privKey, mask);
                        inSK.Add(i_insk);

                        inPKIndex.Add(mixRingCtKey);
                        inPK.Add(ctKey);
                    }

                    foreach(var item in asset_outputs)
                    {
                        destinations.Add(item.PubKey);
                        amounts.Add(item.Value);
                    }

                    Fixed8 vPub = tx.Outputs.Where(p => p.AssetId == key).Sum(p => p.Value);

                    if (tx.Outputs.Length == 0) // S -> S 
                    {
                        if (key == Blockchain.UtilityToken.Hash && sysFee == Fixed8.Zero)
                            vPub = (sysFee > qrsSysFee ? sysFee : qrsSysFee);
                    }
                    else if (vPub == Fixed8.Zero)
                        vPub = (sysFee > qrsSysFee ? sysFee : qrsSysFee);


                    if (key == Blockchain.GoverningToken.Hash)
                    {
                        RingCTSignatureType sig = RingCTSignature.Generate(inSK, inPKIndex, destinations, amounts, sysFee + vPub, RingSize, key, Fixed8.Zero);
                        sig.AssetID = key;
                        tx.RingCTSig.Add(sig);
                    }
                    else if (key == Blockchain.UtilityToken.Hash)
                    {
                        RingCTSignatureType sig = RingCTSignature.Generate(inSK, inPKIndex, destinations, amounts, sysFee + vPub, RingSize, key, Fixed8.Zero);
                        sig.AssetID = key;
                        tx.RingCTSig.Add(sig);
                    }
                    else
                    {
                        RingCTSignatureType sig = RingCTSignature.Generate(inSK, inPKIndex, destinations, amounts, vPub, RingSize, key, Fixed8.Zero);
                        sig.AssetID = key;
                        tx.RingCTSig.Add(sig);
                    }

                    if (!RingCTSignature.Verify(tx.RingCTSig[tx.RingCTSig.Count - 1], Fixed8.Zero))
                    {
                        return null;
                    }
                }
                
            }
            else if (fromAddressType == AnonymouseAddressVersion)
            {
                
            }
            else if (fromAddressType == AddressVersion)
            {
                var pay_coins = pay_total.Select(p => new
                {
                    AssetId = p.Key,
                    Unspents = FindUnspentCoins(p.Key, p.Value, from_addr)
                }).ToDictionary(p => p.AssetId);
                
                if (pay_coins.Any(p => p.Value.Unspents == null)) return null;
                var input_sum = pay_coins.Values.ToDictionary(p => p.AssetId, p => new
                {
                    AssetId = p.AssetId,
                    Value = p.Unspents.Sum(q => q.Output.Value)
                });
                if (change_address == null) change_address = GetChangeAddress();
                List<TransactionOutput> outputs_new = new List<TransactionOutput>(tx.Outputs);
                foreach (UInt256 asset_id in input_sum.Keys)
                {
                    if (input_sum[asset_id].Value > pay_total[asset_id])
                    {
                        outputs_new.Add(new TransactionOutput
                        {
                            AssetId = asset_id,
                            Value = input_sum[asset_id].Value - pay_total[asset_id],
                            ScriptHash = change_address
                        });
                    }
                }
                tx.Inputs = pay_coins.Values.SelectMany(p => p.Unspents).Select(p => p.Reference).ToArray();
                tx.Outputs = outputs_new.ToArray();

                foreach (var key in pay_coins.Keys)
                {
                    List<CTCommitment> inSK = new List<CTCommitment>();
                    List<CTKey> inPK = new List<CTKey>();
                    List<MixRingCTKey> inPKIndex = new List<MixRingCTKey>();
                    List<Cryptography.ECC.ECPoint> destinations = new List<Cryptography.ECC.ECPoint>();
                    List<Fixed8> amounts = new List<Fixed8>();

                    // 00. Calculate the input values
                    Fixed8 vPubOld = pay_coins[key].Unspents.Sum(p => p.Output.Value);

                    // 01. Construct inSK and pubs_index
                    var asset_outputs = rctOutput.Where(p => p.AssetId == key);

                    for (int i = 0; i < 1; i++)
                    {
                        byte[] privKey = new byte[32];
                        Cryptography.ECC.ECPoint pubKey = Cryptography.ECC.ECCurve.Secp256r1.G * privKey;

                        byte[] mask = new byte[32];

                        Fixed8 amount = vPubOld;
                        byte[] b_amount = amount.ToBinaryFormat().ToBinary();

                        Cryptography.ECC.ECPoint C_i_in = RingCTSignature.GetCommitment(mask, b_amount);

                        CTKey ctKey = new CTKey(pubKey, C_i_in);
                        MixRingCTKey mixRingCTKey = new MixRingCTKey(new UInt256(), 0xff, 0xff);

                        CTCommitment i_insk = new CTCommitment(privKey, mask);
                        inSK.Add(i_insk);

                        inPK.Add(ctKey);
                        inPKIndex.Add(mixRingCTKey);
                    }

                    foreach (var item in asset_outputs)
                    {
                        destinations.Add(item.PubKey);
                        amounts.Add(item.Value);
                    }

                    Fixed8 vPub = tx.Outputs.Where(p => p.AssetId == key).Sum(p => p.Value);

                    if (key == Blockchain.GoverningToken.Hash)
                    {
                        RingCTSignatureType sig = RingCTSignature.Generate(inSK, inPKIndex, destinations, amounts, sysFee + vPub, RingSize, key, vPubOld);
                        sig.AssetID = key;
                        tx.RingCTSig.Add(sig);
                    }
                    else if (key == Blockchain.UtilityToken.Hash)
                    {
                        RingCTSignatureType sig = RingCTSignature.Generate(inSK, inPKIndex, destinations, amounts, sysFee + vPub, RingSize, key, vPubOld);
                        sig.AssetID = key;
                        tx.RingCTSig.Add(sig);
                    }
                    else
                    {
                        RingCTSignatureType sig = RingCTSignature.Generate(inSK, inPKIndex, destinations, amounts, vPub, RingSize, key, vPubOld);
                        sig.AssetID = key;
                        tx.RingCTSig.Add(sig);
                    }

                    if (!RingCTSignature.Verify(tx.RingCTSig[0], vPubOld))
                    {
                        return null;
                    }
                    break;
                }

                //return tx;
            }
            #endregion

            return tx;
        }

        public T MakeTransactionFrom<T>(T tx, string from_addr, UInt160 change_address = null, Fixed8 fee = default(Fixed8)) where T : Transaction
        {
            if (tx.Outputs == null) tx.Outputs = new TransactionOutput[0];
            if (tx.Attributes == null) tx.Attributes = new TransactionAttribute[0];

            foreach (TransactionOutput output in tx.Outputs)
            {
                fee += output.Fee;
            }

            var pay_total = (typeof(T) == typeof(IssueTransaction) ? new TransactionOutput[0] : tx.Outputs).GroupBy(p => p.AssetId, (k, g) => new
            {
                AssetId = k,
                Value = g.Sum(p => p.Value)
            }).ToDictionary(p => p.AssetId);
            if (fee > Fixed8.Zero)
            {
                if (pay_total.ContainsKey(Blockchain.UtilityToken.Hash))
                {
                    pay_total[Blockchain.UtilityToken.Hash] = new
                    {
                        AssetId = Blockchain.UtilityToken.Hash,
                        Value = pay_total[Blockchain.UtilityToken.Hash].Value + fee
                    };
                }
                else
                {
                    pay_total.Add(Blockchain.UtilityToken.Hash, new
                    {
                        AssetId = Blockchain.UtilityToken.Hash,
                        Value = fee
                    });
                }
            }
            var pay_coins = pay_total.Select(p => new
            {
                AssetId = p.Key,
                Unspents = FindUnspentCoins(p.Key, p.Value.Value, from_addr)
            }).ToDictionary(p => p.AssetId);
            if (pay_coins.Any(p => p.Value.Unspents == null)) return null;
            var input_sum = pay_coins.Values.ToDictionary(p => p.AssetId, p => new
            {
                AssetId = p.AssetId,
                Value = p.Unspents.Sum(q => q.Output.Value)
            });
            if (change_address == null) change_address = GetChangeAddress();
            List<TransactionOutput> outputs_new = new List<TransactionOutput>(tx.Outputs);
            foreach (UInt256 asset_id in input_sum.Keys)
            {
                if (input_sum[asset_id].Value > pay_total[asset_id].Value)
                {
                    outputs_new.Add(new TransactionOutput
                    {
                        AssetId = asset_id,
                        Value = input_sum[asset_id].Value - pay_total[asset_id].Value,
                        ScriptHash = change_address
                    });
                }
            }
            tx.Inputs = pay_coins.Values.SelectMany(p => p.Unspents).Select(p => p.Reference).ToArray();
            tx.Outputs = outputs_new.ToArray();
            return tx;
        }

        public T MakeTransaction<T>(T tx, UInt160 change_address = null, Fixed8 fee = default(Fixed8)) where T : Transaction
        {
            if (tx.Outputs == null) tx.Outputs = new TransactionOutput[0];
            if (tx.Attributes == null) tx.Attributes = new TransactionAttribute[0];
            fee += tx.SystemFee;        // comment engine or pure-gui, else, not comment this line.
            
            Dictionary<UInt256, Fixed8> dicQRGFee = new Dictionary<UInt256, Fixed8>();
            Dictionary<UInt256, Fixed8> dicQRSFee = new Dictionary<UInt256, Fixed8>();

            foreach (var group in tx.Outputs.GroupBy(p => p.AssetId))
            {
                AssetState asset = Blockchain.Default.GetAssetState(group.Key);

                if (!dicQRGFee.ContainsKey(asset.AssetId) && asset.AssetId != Blockchain.GoverningToken.Hash)
                {
                    dicQRGFee[asset.AssetId] = asset.Fee;
                }

                if (!dicQRSFee.ContainsKey(asset.AssetId) && asset.AssetId == Blockchain.GoverningToken.Hash)
                {
                    dicQRSFee[asset.AssetId] = asset.Fee;
                }
            }

            Fixed8 qrsFee = dicQRSFee.Sum(p => p.Value);
            Fixed8 qrgFee = dicQRGFee.Sum(p => p.Value);

            fee += qrgFee;

            var pay_total = (typeof(T) == typeof(IssueTransaction) ? new TransactionOutput[0] : tx.Outputs).GroupBy(p => p.AssetId, (k, g) => new
            {
                AssetId = k,
                Value = g.Sum(p => p.Value)
            }).ToDictionary(p => p.AssetId);
            if (fee > Fixed8.Zero)
            {
                if (pay_total.ContainsKey(Blockchain.UtilityToken.Hash))
                {
                    pay_total[Blockchain.UtilityToken.Hash] = new
                    {
                        AssetId = Blockchain.UtilityToken.Hash,
                        Value = pay_total[Blockchain.UtilityToken.Hash].Value + fee
                    };
                }
                else
                {
                    pay_total.Add(Blockchain.UtilityToken.Hash, new
                    {
                        AssetId = Blockchain.UtilityToken.Hash,
                        Value = fee
                    });
                }
            }
            var pay_coins = pay_total.Select(p => new
            {
                AssetId = p.Key,
                Unspents = FindUnspentCoins(p.Key, p.Value.Value)
            }).ToDictionary(p => p.AssetId);
            if (pay_coins.Any(p => p.Value.Unspents == null)) return null;
            var input_sum = pay_coins.Values.ToDictionary(p => p.AssetId, p => new
            {
                AssetId = p.AssetId,
                Value = p.Unspents.Sum(q => q.Output.Value)
            });
            if (change_address == null) change_address = GetChangeAddress();
            List<TransactionOutput> outputs_new = new List<TransactionOutput>(tx.Outputs);
            foreach (UInt256 asset_id in input_sum.Keys)
            {
                if (input_sum[asset_id].Value > pay_total[asset_id].Value)
                {
                    outputs_new.Add(new TransactionOutput
                    {
                        AssetId = asset_id,
                        Value = input_sum[asset_id].Value - pay_total[asset_id].Value,
                        ScriptHash = change_address
                    });
                }
            }
            tx.Inputs = pay_coins.Values.SelectMany(p => p.Unspents).Select(p => p.Reference).ToArray();
            tx.Outputs = outputs_new.ToArray();

            //if (qrgFee < tx.SystemFee) return null; //revised
            return tx;
        }

        public T Perform_JoinSplit<T>(T tx, AsyncJoinSplitInfo info, UInt256 joinSplitPubKey_, byte[] joinSplitPrivKey_, UInt256 Asset_ID, IntPtr witnesses, UInt256 anchor) where T : AnonymousContractTransaction
        {
            //SnarkDllApi.Snark_DllInit(1, "d:\\pk.key", "d:\\vk.key");

            QrsJoinSplit qrsJSparam = new QrsJoinSplit();

            IntPtr asyncJoinSplitInfo_ = SnarkDllApi.Snark_AsyncJoinSplitInfo_Create();

            SnarkDllApi.Snark_AsyncJoinSplitInfo_Add_Amounts(asyncJoinSplitInfo_, info.vpub_old.GetData(), info.vpub_new.GetData(), Asset_ID.ToArray());

            for (int i = 0; i < info.vjsout.Count; i ++)
            {
                IntPtr pJsOutput = SnarkDllApi.Snark_Jsoutput_Create();

                SnarkDllApi.Snark_Jsoutput_Init(pJsOutput, info.vjsout[i].addr.a_pk.ToArray(), info.vjsout[i].addr.pk_enc.ToArray(), info.vjsout[i].value.GetData(), info.vjsout[i].memo, info.vjsout[i].AssetID.ToArray());
                SnarkDllApi.Snark_AsyncJoinSplitInfo_Add_JSOutput(asyncJoinSplitInfo_, pJsOutput);
                SnarkDllApi.Snark_Jsoutput_Delete(pJsOutput);
            }

            for (int i = 0; i < info.notes.Count; i++)
            {
                IntPtr pNote = SnarkDllApi.Note_Create();
                SnarkDllApi.Note_Init(pNote, info.notes[i].a_pk.ToArray(), info.notes[i].rho.ToArray(), info.notes[i].r.ToArray(), info.notes[i].value.GetData(), info.notes[i].assetID.ToArray());
                SnarkDllApi.Snark_AsyncJoinSplitInfo_Add_Notes(asyncJoinSplitInfo_, pNote);
                SnarkDllApi.Note_Delete(pNote);
            }

            int[] out_len = new int[1];
            out_len[0] = 0;

            byte[] a_sk = new byte[32];

            if (info.vjsin.Count > 0)
            {
                SpendingKey key = info.vjsin[0].key;
                a_sk = key.ToArray();
            }
            else if (tx.Inputs.Length > 0)
            {
                ContractParametersContext context;
                try
                {
                    context = new ContractParametersContext(tx);
                }
                catch (InvalidOperationException)
                {
                    return tx;
                }

                foreach (UInt160 scriptHash in context.ScriptHashes)
                {
                    VerificationContract contract = GetContract(scriptHash);
                    if (contract == null) continue;
                    KeyPair key = (KeyPair)GetKeyByScriptHash(scriptHash);
                    if (key == null) continue;
                    a_sk = key.PrivateKey;
                    break;
                }
            }

            if (info.notes.Count == 0)
            {
                UInt256 key = new UInt256();
                a_sk = key.ToArray();
            }

            IntPtr byRet = SnarkDllApi.Snark_Perform_joinsplit(asyncJoinSplitInfo_, joinSplitPubKey_.ToArray(), joinSplitPrivKey_, out_len, a_sk, anchor.ToArray(), witnesses);

            byte[] jsBytes = new byte[out_len[0]];
            System.Runtime.InteropServices.Marshal.Copy(byRet, jsBytes, 0, out_len[0]);

            tx.byJoinSplit.Add(jsBytes);

            tx.joinSplitPubKey = joinSplitPubKey_;

            bool byr = SnarkDllApi.Snark_JSVerify(tx.byJoinSplit[tx.byJoinSplit.Count - 1], joinSplitPubKey_.ToArray());
            SnarkDllApi.Snark_AsyncJoinSplitInfo_Delete(asyncJoinSplitInfo_);
            SnarkDllApi.Snark_FreeMeomory(byRet);

            return tx;
        }

        public T MakeTandATransaction<T>(T tx, string from_addr, AsyncJoinSplitInfo info, UInt160 change_address = null, Fixed8 fee = default(Fixed8)) where T : AnonymousContractTransaction
        {
            byte fromAddrVersion;
            fromAddrVersion = Wallet.GetAddressVersion(from_addr);

            if (fromAddrVersion == Wallet.AddressVersion)
            {
                if (tx.Outputs == null) tx.Outputs = new TransactionOutput[0];
                if (tx.Attributes == null) tx.Attributes = new TransactionAttribute[0];
                //fee += tx.FromTSysFee;

                foreach (var txOut in tx.Outputs.GroupBy(p => p.AssetId))
                {
                    AssetState asset = Blockchain.Default.GetAssetState(txOut.Key);
                    fee += asset.AFee;
                }

                foreach (var jsOut in info.vjsout.GroupBy(p => p.AssetID))
                {
                    AssetState asset = Blockchain.Default.GetAssetState(jsOut.Key);
                    fee += asset.AFee;
                }

                TransactionOutput[] outSample = new TransactionOutput[1];

                outSample[0] = new TransactionOutput();
                outSample[0].AssetId = info.vjsout[0].AssetID;
                outSample[0].Value = info.vpub_old;
                var pay_total = outSample.GroupBy(p => p.AssetId, (k, g) => new
                {
                    AssetId = k,
                    Value = g.Sum(p => p.Value)
                }).ToDictionary(p => p.AssetId);

                
                if (fee > Fixed8.Zero)
                {
                    if (pay_total.ContainsKey(Blockchain.UtilityToken.Hash))
                    {
                        pay_total[Blockchain.UtilityToken.Hash] = new
                        {
                            AssetId = Blockchain.UtilityToken.Hash,
                            Value = pay_total[Blockchain.UtilityToken.Hash].Value + fee
                        };
                    }
                    else
                    {
                        pay_total.Add(Blockchain.UtilityToken.Hash, new
                        {
                            AssetId = Blockchain.UtilityToken.Hash,
                            Value = fee
                        });
                    }
                }
                var pay_coins = pay_total.Select(p => new
                {
                    AssetId = p.Key,
                    Unspents = FindUnspentCoins(p.Key, p.Value.Value, from_addr)
                }).ToDictionary(p => p.AssetId);
                if (pay_coins.Any(p => p.Value.Unspents == null)) return null;
                var input_sum = pay_coins.Values.ToDictionary(p => p.AssetId, p => new
                {
                    AssetId = p.AssetId,
                    Value = p.Unspents.Sum(q => q.Output.Value)
                });
                if (change_address == null) change_address = GetChangeAddress();
                List<TransactionOutput> outputs_new = new List<TransactionOutput>(tx.Outputs);
                foreach (UInt256 asset_id in input_sum.Keys)
                {
                    if (input_sum[asset_id].Value > pay_total[asset_id].Value)
                    {
                        outputs_new.Add(new TransactionOutput
                        {
                            AssetId = asset_id,
                            Value = input_sum[asset_id].Value - pay_total[asset_id].Value,
                            ScriptHash = change_address
                        });
                    }
                }
                tx.Inputs = pay_coins.Values.SelectMany(p => p.Unspents).Select(p => p.Reference).ToArray();
                tx.Outputs = outputs_new.ToArray();
            }
            
            return tx;
        }

        public T MakeAandATransaction<T>(T tx, string from_addr, AsyncJoinSplitInfo info, PaymentAddress change_address = null, Fixed8 fee = default(Fixed8)) where T : AnonymousContractTransaction
        {
            byte fromAddrVersion;
            fromAddrVersion = Wallet.GetAddressVersion(from_addr);

            if (tx.Outputs == null) tx.Outputs = new TransactionOutput[0];
            if (tx.Attributes == null) tx.Attributes = new TransactionAttribute[0];
            
            Fixed8 qrgAssetFee = Fixed8.Zero;
            foreach (var txOut in info.vjsout.GroupBy(p => p.AssetID))
            {
                AssetState asset = Blockchain.Default.GetAssetState(txOut.Key);
                qrgAssetFee += asset.AFee;
            }
            
            var pay_total = (typeof(T) == typeof(IssueTransaction) ? new JSOutput[0] : info.vjsout.ToArray()).GroupBy(p => p.AssetID, (k, g) => new
            {
                AssetId = k,
                Value = g.Sum(p => p.value)
            }).ToDictionary(p => p.AssetId);

            if (fromAddrVersion == Wallet.AnonymouseAddressVersion)
            {
                if (fee > Fixed8.Zero || qrgAssetFee > Fixed8.Zero)
                {
                    if (pay_total.ContainsKey(Blockchain.UtilityToken.Hash))
                    {
                        pay_total[Blockchain.UtilityToken.Hash] = new
                        {
                            AssetId = Blockchain.UtilityToken.Hash,
                            Value = pay_total[Blockchain.UtilityToken.Hash].Value + qrgAssetFee
                        };
                    }
                    else
                    {
                        pay_total.Add(Blockchain.UtilityToken.Hash, new
                        {
                            AssetId = Blockchain.UtilityToken.Hash,
                            Value = qrgAssetFee
                        });
                    }
                }

                var pay_coins = pay_total.Select(p => new
                {
                    AssetId = p.Key,
                    Unspents = FindUnspentNotes(p.Key, p.Value.Value, from_addr)
                }).ToDictionary(p => p.AssetId);
                if (pay_coins.Any(p => p.Value.Unspents == null)) return null;
                var input_sum = pay_coins.Values.ToDictionary(p => p.AssetId, p => new
                {
                    AssetId = p.AssetId,
                    Value = p.Unspents.Sum(q => q.Output.Value)
                });
                if (change_address == null) change_address = GetChangeAddressAsPaymentAddress();
                List<JSOutput> outputs_new = new List<JSOutput>(info.vjsout);
                foreach (UInt256 asset_id in input_sum.Keys)
                {
                    if (input_sum[asset_id].Value > pay_total[asset_id].Value)
                    {
                        outputs_new.Add(new JSOutput
                        {
                            AssetID = asset_id,
                            value = input_sum[asset_id].Value - pay_total[asset_id].Value,
                            addr = change_address
                        });
                    }
                }

                var vin = pay_coins.Values.SelectMany(p => p.Unspents).ToArray();

                List<JSInput> input_new = new List<JSInput>(info.vjsin);
                List<Note> input_notes = new List<Note>(info.notes);
                foreach (JSCoin coin in vin)
                {
                    Note lnote = new Note(coin.Output.addr.a_pk, coin.Output.Value, coin.Output.rho, coin.Output.r, coin.Output.AssetId);

                    KeyPair key = (KeyPair)GetKeyByScriptHash(coin.Output.ScriptHash);
                    SpendingKey lkey;

                    using (key.Decrypt())
                    {
                        byte[] pk = new byte[32];
                        Buffer.BlockCopy(key.PrivateKey, 0, pk, 0, 32);
                        lkey = new SpendingKey(new Pure.UInt256(pk));
                        string addr = ToAnonymousAddress(lkey.address());
                    }

                    input_new.Add(new JSInput
                    {
                        note = lnote,
                        key = lkey,
                        witness = coin.Output.witness,
                        AssetID = coin.Output.AssetId
                    });

                    input_notes.Add(lnote);
                }
                info.notes = input_notes;
                info.vjsin = input_new;
                info.vjsout = outputs_new;
            }

            if (qrgAssetFee < tx.SystemFee) return null;
            return tx;
        }

        public T MakeAandTTransaction<T>(T tx, string from_addr, AsyncJoinSplitInfo info, PaymentAddress change_address = null, Fixed8 fee = default(Fixed8)) where T : AnonymousContractTransaction
        {
            byte fromAddrVersion;
            fromAddrVersion = Wallet.GetAddressVersion(from_addr);
            
            if (fromAddrVersion == Wallet.AnonymouseAddressVersion)
            {
                if (tx.Outputs == null) tx.Outputs = new TransactionOutput[0];
                if (tx.Attributes == null) tx.Attributes = new TransactionAttribute[0];

                Fixed8 qrgAssetFee = Fixed8.Zero;
                foreach (var txOut in tx.Outputs.GroupBy(p => p.AssetId))
                {
                    AssetState asset = Blockchain.Default.GetAssetState(txOut.Key);
                    qrgAssetFee += asset.AFee;
                }

                var pay_total = (typeof(T) == typeof(IssueTransaction) ? new TransactionOutput[0] : tx.Outputs).GroupBy(p => p.AssetId, (k, g) => new
                {
                    AssetId = k,
                    Value = g.Sum(p => p.Value)
                }).ToDictionary(p => p.AssetId);

                if (fee > Fixed8.Zero || qrgAssetFee > Fixed8.Zero)
                {
                    if (pay_total.ContainsKey(Blockchain.UtilityToken.Hash))
                    {
                        pay_total[Blockchain.UtilityToken.Hash] = new
                        {
                            AssetId = Blockchain.UtilityToken.Hash,
                            Value = pay_total[Blockchain.UtilityToken.Hash].Value + qrgAssetFee
                        };
                    }
                    else
                    {
                        pay_total.Add(Blockchain.UtilityToken.Hash, new
                        {
                            AssetId = Blockchain.UtilityToken.Hash,
                            Value = qrgAssetFee
                        });
                    }
                }


                var pay_coins = pay_total.Select(p => new
                {
                    AssetId = p.Key,
                    Unspents = FindUnspentNotes(p.Key, p.Value.Value, from_addr)
                }).ToDictionary(p => p.AssetId);
                if (pay_coins.Any(p => p.Value.Unspents == null)) return null;
                var input_sum = pay_coins.Values.ToDictionary(p => p.AssetId, p => new
                {
                    AssetId = p.AssetId,
                    Value = p.Unspents.Sum(q => q.Output.Value)
                });
                if (change_address == null) change_address = GetChangeAddressAsPaymentAddress();
                List<JSOutput> outputs_new = new List<JSOutput>(info.vjsout);
                foreach (UInt256 asset_id in input_sum.Keys)
                {
                    if (input_sum[asset_id].Value > pay_total[asset_id].Value)
                    {
                        outputs_new.Add(new JSOutput
                        {
                            AssetID = asset_id,
                            value = input_sum[asset_id].Value - pay_total[asset_id].Value,
                            addr = change_address
                        });
                    }
                }

                var vin = pay_coins.Values.SelectMany(p => p.Unspents).ToArray();

                List<JSInput> input_new = new List<JSInput>(info.vjsin);
                List<Note> input_notes = new List<Note>(info.notes);
                foreach (JSCoin coin in vin)
                {
                    Note lnote = new Note(coin.Output.addr.a_pk, coin.Output.Value, coin.Output.rho, coin.Output.r, coin.Output.AssetId);
                    
                    KeyPair key = (KeyPair)GetKeyByScriptHash(coin.Output.ScriptHash);
                    SpendingKey lkey;

                    using (key.Decrypt())
                    {
                        byte[] pk = new byte[32];
                        Buffer.BlockCopy(key.PrivateKey, 0, pk, 0, 32);
                        lkey = new SpendingKey(new Pure.UInt256(pk));
                        string addr = ToAnonymousAddress(lkey.address());
                    }

                    input_new.Add(new JSInput
                    {
                        note = lnote,
                        key = lkey,
                        witness = coin.Output.witness,
                        AssetID = coin.Output.AssetId
                    });

                    input_notes.Add(lnote);
                }
                info.notes = input_notes;
                info.vjsin = input_new;
                info.vjsout = outputs_new;

                if (qrgAssetFee < tx.SystemFee) return null;
            }
            return tx;
        }

        public Transaction MakeTransaction(List<TransactionAttribute> attributes, IEnumerable<TransferOutput> outputs, UInt160 change_address = null, Fixed8 fee = default(Fixed8))
        {
            var cOutputs = outputs.Where(p => !p.IsGlobalAsset).GroupBy(p => new
            {
                AssetId = (UInt160)p.AssetId,
                Account = p.ScriptHash
            }, (k, g) => new
            {
                AssetId = k.AssetId,
                Value = g.Aggregate(BigInteger.Zero, (x, y) => x + y.Value.Value),
                Account = k.Account
            }).ToArray();
            Transaction tx;
            if (attributes == null) attributes = new List<TransactionAttribute>();
            if (cOutputs.Length == 0)
            {
                tx = new ContractTransaction();
            }
            else
            {
                UInt160[] addresses = GetAddresses().ToArray();
                HashSet<UInt160> sAttributes = new HashSet<UInt160>();
                using (ScriptBuilder sb = new ScriptBuilder())
                {
                    foreach (var output in cOutputs)
                    {
                        byte[] script;
                        using (ScriptBuilder sb2 = new ScriptBuilder())
                        {
                            foreach (UInt160 address in addresses)
                                sb2.EmitAppCall(output.AssetId, "balanceOf", address);
                            sb2.Emit(OpCode.DEPTH, OpCode.PACK);
                            script = sb2.ToArray();
                        }
                        ApplicationEngine engine = ApplicationEngine.Run(script);
                        if (engine.State.HasFlag(VMState.FAULT)) return null;
                        var balances = engine.EvaluationStack.Pop().GetArray().Reverse().Zip(addresses, (i, a) => new
                        {
                            Account = a,
                            Value = i.GetBigInteger()
                        }).ToArray();
                        BigInteger sum = balances.Aggregate(BigInteger.Zero, (x, y) => x + y.Value);
                        if (sum < output.Value) return null;
                        if (sum != output.Value)
                        {
                            balances = balances.OrderByDescending(p => p.Value).ToArray();
                            BigInteger amount = output.Value;
                            int i = 0;
                            while (balances[i].Value <= amount)
                                amount -= balances[i++].Value;
                            if (amount == BigInteger.Zero)
                                balances = balances.Take(i).ToArray();
                            else
                                balances = balances.Take(i).Concat(new[] { balances.Last(p => p.Value >= amount) }).ToArray();
                            sum = balances.Aggregate(BigInteger.Zero, (x, y) => x + y.Value);
                        }
                        sAttributes.UnionWith(balances.Select(p => p.Account));
                        for (int i = 0; i < balances.Length; i++)
                        {
                            BigInteger value = balances[i].Value;
                            if (i == 0)
                            {
                                BigInteger change = sum - output.Value;
                                if (change > 0) value -= change;
                            }
                            sb.EmitAppCall(output.AssetId, "transfer", balances[i].Account, output.Account, value);
                            sb.Emit(OpCode.THROWIFNOT);
                        }
                    }
                    byte[] nonce = new byte[8];
                    rand.NextBytes(nonce);
                    sb.Emit(OpCode.RET, nonce);
                    tx = new InvocationTransaction
                    {
                        Version = 1,
                        Script = sb.ToArray()
                    };
                }
                attributes.AddRange(sAttributes.Select(p => new TransactionAttribute
                {
                    Usage = TransactionAttributeUsage.Script,
                    Data = p.ToArray()
                }));
            }
            tx.Attributes = attributes.ToArray();
            tx.Inputs = new CoinReference[0];
            tx.Outputs = outputs.Where(p => p.IsGlobalAsset).Select(p => p.ToTxOutput()).ToArray();
            tx.Scripts = new Witness[0];
            if (tx is InvocationTransaction itx)
            {
                ApplicationEngine engine = ApplicationEngine.Run(itx.Script, itx);
                if (engine.State.HasFlag(VMState.FAULT)) return null;
                tx = new InvocationTransaction
                {
                    Version = itx.Version,
                    Script = itx.Script,
                    Gas = InvocationTransaction.GetGas(engine.GasConsumed),
                    Attributes = itx.Attributes,
                    Inputs = itx.Inputs,
                    Outputs = itx.Outputs
                };
            }
            tx = MakeTransaction(tx, change_address, fee);
            return tx;
        }

        protected abstract void OnProcessNewBlock(Block block, 
                                IEnumerable<Coin> added, 
                                IEnumerable<Coin> changed, 
                                IEnumerable<Coin> deleted,
                                IEnumerable<JSCoin> jsadded,
                                IEnumerable<JSCoin> jschanged,
                                IEnumerable<JSCoin> jsdeleted,
                                IEnumerable<JSCoin> jswitnesschanged,
                                IEnumerable<RCTCoin> rctadded,
                                IEnumerable<RCTCoin> rctchanged,
                                IEnumerable<RCTCoin> rctdeleted);
        protected abstract void OnSaveTransaction(Transaction tx, 
                                IEnumerable<Coin> added, 
                                IEnumerable<Coin> changed, 
                                IEnumerable<JSCoin> jsadded, 
                                IEnumerable<JSCoin> jschanged, 
                                IEnumerable<JSCoin> jsdeleted, 
                                IEnumerable<JSCoin> jswitnesschanged,
                                IEnumerable<RCTCoin> rctadded,
                                IEnumerable<RCTCoin> rctchanged,
                                IEnumerable<RCTCoin> rctdeleted);

        private void ProcessBlocks()
        {
            while (isrunning)
            {
                #region Test
                if (Blockchain.IsTestRingCT == true)
                {
                    //Block block = Blockchain.Default.GetBlock(0);
                    //if (block != null) ProcessNewBlock(block);
                }
                #endregion
                while (current_height <= Blockchain.Default?.Height && isrunning)
                {
                    lock (SyncRoot)
                    {
                        Block block = Blockchain.Default.GetBlock(current_height);
                        if (block != null) ProcessNewBlock(block);
                    }
                }
                for (int i = 0; i < 20 && isrunning; i++)
                {
                    Thread.Sleep(100);
                }
            }
        }

        public byte[] GetWitnessInByte(IntPtr witness)
        {
            int[] out_len = new int[1];
            IntPtr pWitness = SnarkDllApi.GetCMWitnessInBinary(witness, out_len);

            byte[] byWitness = new byte[out_len[0]];

            System.Runtime.InteropServices.Marshal.Copy(pWitness, byWitness, 0, out_len[0]);

            return byWitness;
        }

        private void ProcessNewBlock(Block block)
        {
            List<UInt256> blocks_commitments = new List<UInt256>();
            Coin[] changeset;
            JSCoin[] jschangeset;
            JSCoin[] jswitnesschangedset;
            RCTCoin[] rctchangeset;

            bool changedWitness = false;
            bool changedCMTree = false;
            
            lock (contracts)
                lock (coins)
                {
                    lock (jscoins)
                    {
                        foreach (Transaction tx in block.Transactions)
                        {
                            #region TEST
                            
                            if (tx.Type == TransactionType.MinerTransaction)
                            {
                                if (tx.Outputs.Length > 0)
                                {
                                    int i = 0;
                                    i++;

                                    Consensus.ConsensusService service = new Consensus.ConsensusService(null, this);

                                    MinerTransaction mtx = service.CreateMinerTransaction(block.Transactions.Where(p => p.Type != TransactionType.MinerTransaction), block.Index, 0);
                                }
                            }
                            
                            #endregion

                            for (ushort index = 0; index < tx.Outputs.Length; index++)
                            {
                                TransactionOutput output = tx.Outputs[index];
                                AddressState state = CheckAddressState(output.ScriptHash);
                                if (state.HasFlag(AddressState.InWallet))
                                {
                                    CoinReference key = new CoinReference
                                    {
                                        PrevHash = tx.Hash,
                                        PrevIndex = index
                                    };
                                    if (coins.Contains(key))
                                        coins[key].State |= CoinState.Confirmed;
                                    else
                                        coins.Add(new Coin
                                        {
                                            Reference = key,
                                            Output = output,
                                            State = CoinState.Confirmed
                                        });
                                    if (state.HasFlag(AddressState.WatchOnly))
                                        coins[key].State |= CoinState.WatchOnly;
                                }
                            }
                        }
                        foreach (Transaction tx in block.Transactions)
                        {
                            foreach (CoinReference input in tx.Inputs)
                            {
                                if (coins.Contains(input))
                                {
                                    if (coins[input].Output.AssetId.Equals(Blockchain.GoverningToken.Hash))
                                        coins[input].State |= CoinState.Spent | CoinState.Confirmed;
                                    else
                                        coins.Remove(input);
                                }
                            }
                        }
                        foreach (ClaimTransaction tx in block.Transactions.OfType<ClaimTransaction>())
                        {
                            foreach (CoinReference claim in tx.Claims)
                            {
                                if (coins.Contains(claim))
                                {
                                    coins.Remove(claim);
                                }
                            }
                        }

                        foreach (RingConfidentialTransaction rtx in block.Transactions.OfType<RingConfidentialTransaction>())
                        {
                            for (int i = 0; i < rtx.RingCTSig.Count; i++)
                            {
                                foreach (KeyPairBase key in GetKeys())
                                {
                                    if (key is StealthKeyPair rctKey)
                                    {
                                        for (int j = 0; j < rtx.RingCTSig[i].outPK.Count; j++)
                                        {
                                            if (rtx.RingCTSig[i].outPK[j].dest.ToString() == Cryptography.ECC.ECPoint.DecodePoint(rctKey.GetPaymentPubKeyFromR(rtx.RHashKey), Cryptography.ECC.ECCurve.Secp256r1).ToString())
                                            {
                                                RCTCoinReference reference = new RCTCoinReference
                                                {
                                                    PrevHash = rtx.Hash,
                                                    TxRCTHash = rtx.RHashKey,
                                                    PrevRCTSigId = (ushort)i,
                                                    PrevRCTSigIndex = (ushort)j
                                                };
                                                
                                                byte[] privKey = rctKey.GenOneTimePrivKey(rtx.RHashKey);
                                                string strPrivKey = privKey.ToHexString();
                                                Fixed8 amount = Fixed8.Zero;
                                                byte[] mask;

                                                try
                                                {
                                                    amount = RingCTSignature.DecodeRct(rtx.RingCTSig[i], privKey, j, out mask);
                                                }
                                                catch (Exception ex)
                                                {
                                                    amount = Fixed8.Zero;
                                                }

                                                if (amount > Fixed8.Zero)
                                                {
                                                    bool is_rtc_contains = false;

                                                    RCTransactionOutput output = new RCTransactionOutput
                                                    {
                                                        AssetId = rtx.RingCTSig[i].AssetID,
                                                        Value = amount,
                                                        PubKey = rtx.RingCTSig[i].outPK[j].dest,
                                                        ScriptHash = Contract.CreateRingSignatureRedeemScript(rctKey.PayloadPubKey, rctKey.ViewPubKey).ToScriptHash()
                                                    };

                                                    int k = -1;

                                                    for (k = 0; k < rtx.RingCTSig[i].mixRing.Count; k++)
                                                    {
                                                        for (int l = 0; l < rtx.RingCTSig[i].mixRing[k].Count; l++)
                                                        {
                                                            foreach (RCTCoin coins in rctcoins)
                                                            {
                                                                if ((coins.State & CoinState.Spent) == 0 && coins.Reference.PrevHash == rtx.RingCTSig[i].mixRing[k][l].txHash && coins.Output.AssetId == rtx.RingCTSig[i].AssetID)
                                                                {
                                                                    coins.State |= CoinState.Spent; l = rtx.RingCTSig[i].mixRing[k].Count - 1; k = rtx.RingCTSig[i].mixRing.Count - 1;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }

                                                    k = -1;
                                                    
                                                    for (k = 0; k < rctcoinCache.Count; k ++)
                                                    {
                                                        if (rctcoinCache[k].Reference.TxRCTHash.ToString() == reference.TxRCTHash.ToString()
                                                             && rctcoinCache[k].Output.AssetId.ToString() == output.AssetId.ToString() 
                                                             && (rctcoinCache[k].State & CoinState.Spent) == 0)
                                                        {
                                                            rctcoinCache.RemoveAt(k);
                                                            k--;
                                                        }
                                                    }

                                                    rctcoins.Add(new RCTCoin
                                                    {
                                                        Reference = reference,
                                                        Output = output,
                                                        State = CoinState.Confirmed
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        foreach (AnonymousContractTransaction atx in block.Transactions.OfType<AnonymousContractTransaction>())
                        {
                            for (int jsIndex = 0; jsIndex < atx.byJoinSplit.Count; jsIndex++)
                            {
                                blocks_commitments.Add(atx.Commitments(jsIndex)[0]);
                                blocks_commitments.Add(atx.Commitments(jsIndex)[1]);

                                foreach (KeyPairBase basekey in GetKeys())
                                {
                                    if (basekey is StealthKeyPair)
                                        continue;
                                    KeyPair key = (KeyPair)basekey;
                                    if (key.nVersion == KeyType.Anonymous)
                                    {
                                        using (key.Decrypt())
                                        {
                                            IntPtr byRet = SnarkDllApi.Snark_FindMyNotes(atx.byJoinSplit[jsIndex], key.PrivateKey, atx.joinSplitPubKey.ToArray());

                                            byte[] byCount = new byte[1];
                                            System.Runtime.InteropServices.Marshal.Copy(byRet, byCount, 0, 1);
                                            if (byCount[0] > 0)
                                            {
                                                int bodySize = byCount[0] * 169;
                                                byte[] byBody = new byte[bodySize + 1];
                                                System.Runtime.InteropServices.Marshal.Copy(byRet, byBody, 0, bodySize + 1);

                                                for (int i = 0; i < byCount[0]; i++)
                                                {
                                                    byte index = byBody[0 + 1];

                                                    JSTransactionOutput outItem = new JSTransactionOutput();

                                                    outItem.AssetId = atx.Asset_ID(jsIndex);
                                                    outItem.Value = new Fixed8(BitConverter.ToInt64(byBody, 33 + 1));

                                                    byte[] buffer = new byte[32];
                                                    System.Runtime.InteropServices.Marshal.Copy(byRet + 41 + 1, buffer, 0, 32);
                                                    UInt256 a_pk = new UInt256(buffer);

                                                    byte[] buffer1 = new byte[32];
                                                    System.Runtime.InteropServices.Marshal.Copy(byRet + 73 + 1, buffer1, 0, 32);
                                                    UInt256 pk_enc = new UInt256(buffer1);

                                                    outItem.addr = new PaymentAddress(a_pk, pk_enc);

                                                    outItem.ScriptHash = outItem.addr.ToArray().ToScriptHash();

                                                    byte[] buffer2 = new byte[32];
                                                    System.Runtime.InteropServices.Marshal.Copy(byRet + 105 + 1, buffer2, 0, 32);
                                                    outItem.rho = new UInt256(buffer2);

                                                    byte[] buffer3 = new byte[32];
                                                    System.Runtime.InteropServices.Marshal.Copy(byRet + 137 + 1, buffer3, 0, 32);
                                                    outItem.r = new UInt256(buffer3);

                                                    IntPtr noteMerkleTree = SnarkDllApi.CmMerkleTree_Create();
                                                    IntPtr noteWitness = SnarkDllApi.CmWitness_Create();

                                                    SnarkDllApi.SetCMTreeFromOthers(noteMerkleTree, cmMerkleTree);

                                                    if (index == 0)
                                                    {
                                                        for (int j = 0; j < blocks_commitments.Count - 1; j++)
                                                        {
                                                            SnarkDllApi.AppendCommitment(noteMerkleTree, blocks_commitments[j].ToArray());
                                                        }
                                                        SnarkDllApi.GetWitnessFromMerkleTree(noteWitness, noteMerkleTree);
                                                        SnarkDllApi.AppendCommitmentInWitness(noteWitness, atx.Commitments(jsIndex)[1].ToArray());
                                                    }
                                                    else
                                                    {
                                                        for (int j = 0; j < blocks_commitments.Count; j++)
                                                        {
                                                            SnarkDllApi.AppendCommitment(noteMerkleTree, blocks_commitments[j].ToArray());
                                                        }
                                                        SnarkDllApi.GetWitnessFromMerkleTree(noteWitness, noteMerkleTree);
                                                    }

                                                    outItem.witness_height = block.Index;
                                                    outItem.cmtree_height = block.Index;
                                                    outItem.witness = GetWitnessInByte(noteWitness);

                                                    JSCoinReference jsKey = new JSCoinReference
                                                    {
                                                        PrevHash = atx.Hash,
                                                        PrevJsId = (ushort)jsIndex,
                                                        PrevIndex = index
                                                    };

                                                    for (int k = 0; k < jscoins.Count; k++)
                                                    {
                                                        IntPtr witness = SnarkDllApi.CmWitness_Create();

                                                        if (jscoins[k].Output.witness.Length == 0)
                                                        {
                                                            continue;
                                                        }

                                                        if (jscoins[k].Output.cmtree_height > block.Index)
                                                        {
                                                            continue;
                                                        }
                                                        SnarkDllApi.SetCMWitnessFromBinary(witness, jscoins[k].Output.witness, jscoins[k].Output.witness.Length);

                                                        SnarkDllApi.AppendCommitmentInWitness(witness, atx.Commitments(jsIndex)[0].ToArray());
                                                        SnarkDllApi.AppendCommitmentInWitness(witness, atx.Commitments(jsIndex)[1].ToArray());

                                                        jscoins[k].Output.witness = GetWitnessInByte(witness);

                                                        changedWitness = true;

                                                        byte[] byNullifier = new byte[32];

                                                        SnarkDllApi.GetNullifier(jscoins[k].Output.addr.a_pk.ToArray(),
                                                            jscoins[k].Output.rho.ToArray(),
                                                            jscoins[k].Output.r.ToArray(),
                                                            jscoins[k].Output.Value.GetData(),
                                                            key.PrivateKey,
                                                            byNullifier);

                                                        byte[] byConvNullifier = new byte[32];
                                                        for (int nuIn = 0; nuIn < 32; nuIn ++)
                                                        {
                                                            byConvNullifier[nuIn] = byNullifier[31 - nuIn];
                                                        }
                                                        UInt256 nullifier = new UInt256(byConvNullifier);
                                                        if (Blockchain.Default.IsNullifierAdded(nullifier))
                                                        {
                                                            jscoins[k].State |= CoinState.Spent | CoinState.Confirmed;
                                                        }
                                                    }

                                                    if (jscoins.Contains(jsKey))
                                                    {
                                                        jscoins[jsKey].State |= CoinState.Confirmed;
                                                        jscoins[jsKey].Output = outItem;
                                                        jscoins[jsKey].Reference = jsKey;
                                                    }
                                                    else
                                                        jscoins.Add(new JSCoin
                                                        {
                                                            Reference = jsKey,
                                                            Output = outItem,
                                                            State = CoinState.Confirmed
                                                        });
                                                }
                                            }
                                            else
                                            {
                                                for (int i = 0; i < jscoins.Count; i++)
                                                {
                                                    IntPtr witness = SnarkDllApi.CmWitness_Create();
                                                    if (jscoins[i].Output.witness.Length == 0)
                                                    {
                                                        continue;
                                                    }

                                                    if (jscoins[i].Output.cmtree_height > block.Index)
                                                    {
                                                        continue;
                                                    }

                                                    SnarkDllApi.SetCMWitnessFromBinary(witness, jscoins[i].Output.witness, jscoins[i].Output.witness.Length);
                                                    jscoins[i].Output.cmtree_height = block.Index;

                                                    SnarkDllApi.AppendCommitmentInWitness(witness, atx.Commitments(jsIndex)[0].ToArray());
                                                    SnarkDllApi.AppendCommitmentInWitness(witness, atx.Commitments(jsIndex)[1].ToArray());

                                                    jscoins[i].Output.witness = GetWitnessInByte(witness);

                                                    changedWitness = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        for (int i = 0; i < blocks_commitments.Count; i ++)
                        {
                            SnarkDllApi.AppendCommitment(cmMerkleTree, blocks_commitments[i].ToArray());
                            changedCMTree = true;
                        }

                        //Witness Verify
                        IntPtr root = SnarkDllApi.GetCMRoot(cmMerkleTree);

                        byte[] byRoot = new byte[32];
                        System.Runtime.InteropServices.Marshal.Copy(root, byRoot, 0, 32);

                        for (int windex = 0; windex < jscoins.Count; windex ++)
                        {
                            if (jscoins[windex].Output.witness.Length == 0)
                            {
                                continue;
                            }

                            if (jscoins[windex].Output.cmtree_height > block.Index)
                            {
                                continue;
                            }

                            IntPtr witness = SnarkDllApi.CmWitness_Create();

                            SnarkDllApi.SetCMWitnessFromBinary(witness, jscoins[windex].Output.witness, jscoins[windex].Output.witness.Length);
                            jscoins[windex].Output.cmtree_height = block.Index;

                            IntPtr wRoot = SnarkDllApi.GetCMRootFromWitness(witness);

                            byte[] byWRoot = new byte[32];
                            System.Runtime.InteropServices.Marshal.Copy(wRoot, byWRoot, 0, 32);

                            if (!byRoot.UnsafeCompare(byWRoot))
                            {
                                ErrorsOccured?.Invoke(this, "Commitment Root Error");
                                // throw new System.ArgumentException("Commitment Root Error", "Plese initialize the blockchain DB.");
                            }
                        }
                        //Witness Verify End

                        current_height++;
                        changeset = coins.GetChangeSet();
                        jschangeset = jscoins.GetChangeSet();
                        rctchangeset = rctcoins.GetChangeSet();


                        if (changedWitness)
                            jswitnesschangedset = jscoins.ToArray();
                        else
                            jswitnesschangedset = new JSCoin[0];

                        OnProcessNewBlock(block, 
                             changeset.Where(p => ((ITrackable<CoinReference>)p).TrackState == TrackState.Added), 
                             changeset.Where(p => ((ITrackable<CoinReference>)p).TrackState == TrackState.Changed), 
                             changeset.Where(p => ((ITrackable<CoinReference>)p).TrackState == TrackState.Deleted),
                             jschangeset.Where(p => ((ITrackable<JSCoinReference>)p).TrackState == TrackState.Added),
                             jschangeset.Where(p => ((ITrackable<JSCoinReference>)p).TrackState == TrackState.Changed),
                             jschangeset.Where(p => ((ITrackable<JSCoinReference>)p).TrackState == TrackState.Deleted),
                             jswitnesschangedset.Where(p => ((ITrackable<JSCoinReference>)p).TrackState == TrackState.Deleted || ((ITrackable<JSCoinReference>)p).TrackState == TrackState.None || ((ITrackable<JSCoinReference>)p).TrackState == TrackState.Added || ((ITrackable<JSCoinReference>)p).TrackState == TrackState.Changed),
                             rctchangeset.Where(p => ((ITrackable<RCTCoinReference>)p).TrackState == TrackState.Added),
                             rctchangeset.Where(p => ((ITrackable<RCTCoinReference>)p).TrackState == TrackState.Changed),
                             rctchangeset.Where(p => ((ITrackable<RCTCoinReference>)p).TrackState == TrackState.Deleted)
                             );
                        if (changedCMTree)
                            SaveCmMerkleTree();
                        coins.Commit();
                        jscoins.Commit();
                        rctcoins.Commit();
                    }
                }
            if (changeset.Length > 0 || jschangeset.Length > 0 || rctchangeset.Length > 0)
                BalanceChanged?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Rebuild()
        {
            lock (SyncRoot)
                lock (coins)
                {
                    coins.Clear();
                    coins.Commit();
                    jscoins.Clear();
                    jscoins.Commit();
                    rctcoins.Clear();
                    rctcoins.Commit();

                    byte[] byMerkletree = { 0x00, 0x00, 0x00 };
                    SnarkDllApi.SetCMTreeFromBinary(cmMerkleTree, byMerkletree, byMerkletree.Length);

                    current_height = 0;
                }
        }

        protected abstract void SaveStoredData(string name, byte[] value);

        public bool SaveTransaction(Transaction tx)
        {
            Coin[] changeset;
            JSCoin[] jschangeset;
            RCTCoin[] rctchangeset = new RCTCoin[0];
            lock (contracts)
            {
                lock (coins)
                {
                    if (tx.Inputs.Length > 0)
                    {
                        if (tx.Inputs.Any(p => !coins.Contains(p) || coins[p].State.HasFlag(CoinState.Spent) || !coins[p].State.HasFlag(CoinState.Confirmed)))
                            return false;
                        foreach (CoinReference input in tx.Inputs)
                        {
                            coins[input].State |= CoinState.Spent;
                            coins[input].State &= ~CoinState.Confirmed;
                        }
                        for (ushort i = 0; i < tx.Outputs.Length; i++)
                        {
                            AddressState state = CheckAddressState(tx.Outputs[i].ScriptHash);
                            if (state.HasFlag(AddressState.InWallet))
                            {
                                Coin coin = new Coin
                                {
                                    Reference = new CoinReference
                                    {
                                        PrevHash = tx.Hash,
                                        PrevIndex = i
                                    },
                                    Output = tx.Outputs[i],
                                    State = CoinState.Unconfirmed
                                };
                                if (state.HasFlag(AddressState.WatchOnly))
                                    coin.State |= CoinState.WatchOnly;
                                coins.Add(coin);
                            }
                        }
                        if (tx is ClaimTransaction transaction)
                        {
                            foreach (CoinReference claim in transaction.Claims)
                            {

                                coins[claim].State |= CoinState.Claimed;
                                coins[claim].State &= ~CoinState.Confirmed;
                            }
                        }
                        changeset = coins.GetChangeSet();
                        OnSaveTransaction(tx, changeset.Where(p =>
                        ((ITrackable<CoinReference>)p).TrackState == TrackState.Added),
                        changeset.Where(p =>
                        ((ITrackable<CoinReference>)p).TrackState == TrackState.Changed),
                        Enumerable.Empty<JSCoin>(),
                        Enumerable.Empty<JSCoin>(),
                        Enumerable.Empty<JSCoin>(),
                        Enumerable.Empty<JSCoin>(),
                        Enumerable.Empty<RCTCoin>(),
                        Enumerable.Empty<RCTCoin>(),
                        Enumerable.Empty<RCTCoin>());

                        coins.Commit();
                    }
                    else
                    {
                        changeset = new Coin[0];
                    }
                }

                lock(jscoins)
                {
                    if (tx is AnonymousContractTransaction atx)
                    {
                        for (int i = 0; i < atx.byJoinSplit.Count; i++)
                        {
                            for (int k = 0; k < jscoins.Count; k++)
                            {
                                foreach (KeyPair key in GetKeys())
                                {
                                    if (key.nVersion == KeyType.Anonymous)
                                    {
                                        using (key.Decrypt())
                                        {
                                            byte[] byNullifier = new byte[32];

                                            SnarkDllApi.GetNullifier(jscoins[k].Output.addr.a_pk.ToArray(),
                                                jscoins[k].Output.rho.ToArray(),
                                                jscoins[k].Output.r.ToArray(),
                                                jscoins[k].Output.Value.GetData(),
                                                key.PrivateKey,
                                                byNullifier);

                                            byte[] byConvNullifier = new byte[32];
                                            for (int nuIn = 0; nuIn < 32; nuIn++)
                                            {
                                                byConvNullifier[nuIn] = byNullifier[31 - nuIn];
                                            }
                                            UInt256 nullifier = new UInt256(byConvNullifier);
                                            if (atx.Nullifiers(i)[0] == nullifier || atx.Nullifiers(i)[1] == nullifier)
                                            {
                                                if (jscoins[k].State.HasFlag(CoinState.Spent) && jscoins[k].State.HasFlag(CoinState.Confirmed))
                                                    return false;
                                                jscoins[k].State |= CoinState.Spent;
                                                jscoins[k].State &= ~CoinState.Confirmed;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        for (int jsIndex = 0; jsIndex < atx.byJoinSplit.Count; jsIndex++)
                        {
                            foreach (KeyPair key in GetKeys())
                            {
                                if (key.nVersion == KeyType.Anonymous)
                                {
                                    using (key.Decrypt())
                                    {
                                        IntPtr byRet = SnarkDllApi.Snark_FindMyNotes(atx.byJoinSplit[jsIndex], key.PrivateKey, atx.joinSplitPubKey.ToArray());

                                        byte[] byCount = new byte[1];
                                        System.Runtime.InteropServices.Marshal.Copy(byRet, byCount, 0, 1);
                                        if (byCount[0] > 0)
                                        {
                                            int bodySize = byCount[0] * 169;
                                            byte[] byBody = new byte[bodySize + 1];
                                            System.Runtime.InteropServices.Marshal.Copy(byRet, byBody, 0, bodySize + 1);

                                            for (int i = 0; i < byCount[0]; i++)
                                            {
                                                byte index = byBody[0 + 1];

                                                JSTransactionOutput outItem = new JSTransactionOutput();

                                                outItem.AssetId = atx.Asset_ID(jsIndex);
                                                outItem.Value = new Fixed8(BitConverter.ToInt64(byBody, 33 + 1));

                                                byte[] buffer = new byte[32];
                                                System.Runtime.InteropServices.Marshal.Copy(byRet + 41 + 1, buffer, 0, 32);
                                                UInt256 a_pk = new UInt256(buffer);

                                                byte[] buffer1 = new byte[32];
                                                System.Runtime.InteropServices.Marshal.Copy(byRet + 73 + 1, buffer1, 0, 32);
                                                UInt256 pk_enc = new UInt256(buffer1);

                                                outItem.addr = new PaymentAddress(a_pk, pk_enc);

                                                outItem.ScriptHash = outItem.addr.ToArray().ToScriptHash();

                                                byte[] buffer2 = new byte[32];
                                                System.Runtime.InteropServices.Marshal.Copy(byRet + 105 + 1, buffer2, 0, 32);
                                                outItem.rho = new UInt256(buffer2);

                                                byte[] buffer3 = new byte[32];
                                                System.Runtime.InteropServices.Marshal.Copy(byRet + 137 + 1, buffer3, 0, 32);
                                                outItem.r = new UInt256(buffer3);

                                                outItem.witness_height = -1;
                                                outItem.witness = new byte[0];

                                                JSCoinReference jsKey = new JSCoinReference
                                                {
                                                    PrevHash = atx.Hash,
                                                    PrevJsId = (ushort)jsIndex,
                                                    PrevIndex = index
                                                };

                                                if (jscoins.Contains(jsKey))
                                                    jscoins[jsKey].State |= CoinState.Unconfirmed;
                                                else
                                                    jscoins.Add(new JSCoin
                                                    {
                                                        Reference = jsKey,
                                                        Output = outItem,
                                                        State = CoinState.Unconfirmed
                                                    });


                                            }
                                        }
                                    }
                                }
                            }
                        }
                        
                        jschangeset = jscoins.GetChangeSet();
                        OnSaveTransaction(tx,
                        Enumerable.Empty<Coin>(),
                        Enumerable.Empty<Coin>(),
                        jschangeset.Where(p =>
                        ((ITrackable<JSCoinReference>)p).TrackState == TrackState.Added),
                        jschangeset.Where(p =>
                        ((ITrackable<JSCoinReference>)p).TrackState == TrackState.Changed),
                        Enumerable.Empty<JSCoin>(),
                        Enumerable.Empty<JSCoin>(),
                        Enumerable.Empty<RCTCoin>(),
                        Enumerable.Empty<RCTCoin>(),
                        Enumerable.Empty<RCTCoin>());
                    }
                    else
                    {
                        jschangeset = new JSCoin[0];
                    }
                }

                lock(rctcoins)
                {
                    if (tx is RingConfidentialTransaction rtx)
                    {
                        for (int i = 0; i < rtx.RingCTSig.Count; i++)
                        {
                            foreach (KeyPairBase key in GetKeys())
                            {
                                if (key is StealthKeyPair rctKey)
                                {
                                    for (int j = 0; j < rtx.RingCTSig[i].outPK.Count; j++)
                                    {
                                        if (rtx.RingCTSig[i].outPK[j].dest.ToString() == Cryptography.ECC.ECPoint.DecodePoint(rctKey.GetPaymentPubKeyFromR(rtx.RHashKey), Cryptography.ECC.ECCurve.Secp256r1).ToString())
                                        {
                                            RCTCoinReference reference = new RCTCoinReference
                                            {
                                                PrevHash = rtx.Hash,
                                                TxRCTHash = rtx.RHashKey,
                                                PrevRCTSigId = (ushort)i,
                                                PrevRCTSigIndex = (ushort)j
                                            };

                                            byte[] privKey = rctKey.GenOneTimePrivKey(rtx.RHashKey);
                                            string strPrivKey = privKey.ToHexString();
                                            Fixed8 amount = Fixed8.Zero;
                                            byte[] mask;
                                            try
                                            {
                                                amount = RingCTSignature.DecodeRct(rtx.RingCTSig[i], privKey, j, out mask);
                                            }
                                            catch (Exception ex)
                                            {
                                                amount = Fixed8.Zero;
                                            }

                                            if (amount > Fixed8.Zero)
                                            {
                                                bool is_rtc_contains = false;
                                                RCTransactionOutput output = new RCTransactionOutput
                                                {
                                                    AssetId = rtx.RingCTSig[i].AssetID,
                                                    Value = amount,
                                                    PubKey = rtx.RingCTSig[i].outPK[j].dest,
                                                    ScriptHash = Contract.CreateRingSignatureRedeemScript(rctKey.PayloadPubKey, rctKey.ViewPubKey).ToScriptHash()
                                                };

                                                
                                                for (int k = 0; k < rtx.RingCTSig[i].mixRing.Count; k ++)
                                                {
                                                    for (int l = 0; l < rtx.RingCTSig[i].mixRing[k].Count; l ++)
                                                    {
                                                        foreach (RCTCoin coins in rctcoins)
                                                        {
                                                            if ((coins.State & CoinState.Spent) == 0 && coins.Reference.PrevHash == rtx.RingCTSig[i].mixRing[k][l].txHash && coins.Output.AssetId == rtx.RingCTSig[i].AssetID)
                                                            {
                                                                coins.State |= CoinState.Spent; l = rtx.RingCTSig[i].mixRing[k].Count - 1; k = rtx.RingCTSig[i].mixRing.Count - 1;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }

                                                if (rctcoins.Contains(reference))
                                                {
                                                    rctcoins[reference].State |= CoinState.Confirmed;
                                                }
                                                else
                                                {
                                                    rctcoinCache.Add(new RCTCoin
                                                    {
                                                        Reference = reference,
                                                        Output = output,
                                                        State = CoinState.Confirmed
                                                    }); 
                                                }

                                            }
                                            else
                                            {
                                                rctchangeset = new RCTCoin[0];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        rctchangeset = rctcoins.GetChangeSet();
                        OnSaveTransaction(tx,
                        Enumerable.Empty<Coin>(),
                        Enumerable.Empty<Coin>(),
                        Enumerable.Empty<JSCoin>(),
                        Enumerable.Empty<JSCoin>(),
                        Enumerable.Empty<JSCoin>(),
                        Enumerable.Empty<JSCoin>(),
                        rctchangeset.Where(p =>
                        ((ITrackable<RCTCoinReference>)p).TrackState == TrackState.Added),
                        rctchangeset.Where(p =>
                        ((ITrackable<RCTCoinReference>)p).TrackState == TrackState.Changed),
                        Enumerable.Empty<RCTCoin>());
                    }
                }
            }
            
            if (changeset.Length > 0 || jschangeset.Length > 0 || rctchangeset.Length > 0)
                BalanceChanged?.Invoke(this, EventArgs.Empty);

            return true;
        }

        public bool Sign(ContractParametersContext context)
        {
            bool fSuccess = false;
            foreach (UInt160 scriptHash in context.ScriptHashes)
            {
                VerificationContract contract = GetContract(scriptHash);
                if (contract == null) continue;
                KeyPair key = (KeyPair)GetKeyByScriptHash(scriptHash);
                if (key == null) continue;
                byte[] signature = context.Verifiable.Sign(key);
                fSuccess |= context.AddSignature(contract, key.PublicKey, signature);
            }
            return fSuccess;
        }

        public static string ToAddress(UInt160 scriptHash)
        {
            byte[] data = new byte[21];
            data[0] = AddressVersion;
            Buffer.BlockCopy(scriptHash.ToArray(), 0, data, 1, 20);
            return data.Base58CheckEncode();
        }

        public static string ToAnonymousAddress(PaymentAddress addr)
        {
            byte[] data = new byte[65];
            data[0] = AnonymouseAddressVersion;
            Buffer.BlockCopy(addr.a_pk.ToArray(), 0, data, 1, 32);
            Buffer.BlockCopy(addr.pk_enc.ToArray(), 0, data, 33, 32);
            return data.Base58CheckEncode();
        }

        public static string ToStealthAddress(StealthKeyPair keys)
        {
            byte[] data = new byte[67];
            data[0] = StealthAddressVersion;
            Buffer.BlockCopy(keys.PayloadPubKey.ToString().HexToBytes(), 0, data, 1, 33);
            Buffer.BlockCopy(keys.ViewPubKey.ToString().HexToBytes(), 0, data, 34, 33);
            return data.Base58CheckEncode();
        }

        public static string ToStealthAddress(StealthPubKeys keys)
        {
            byte[] data = new byte[67];
            data[0] = StealthAddressVersion;
            Buffer.BlockCopy(keys.PayloadPubKey.ToString().HexToBytes(), 0, data, 1, 33);
            Buffer.BlockCopy(keys.ViewPubKey.ToString().HexToBytes(), 0, data, 34, 33);
            return data.Base58CheckEncode();
        }

        public static byte GetAddressVersion(string addr)
        {
            byte[] data = addr.Base58CheckDecode();

            if (!(data.Length == 21 || data.Length == 65 || data.Length == 67))
                throw new FormatException();
            if (!(data[0] == AddressVersion || data[0] == AnonymouseAddressVersion || data[0] == StealthAddressVersion))
                throw new FormatException();

            return data[0];
        }

        public static PaymentAddress ToPaymentAddress(string address)
        {
            byte[] data = address.Base58CheckDecode();
            if (data.Length != 65)
                throw new FormatException();
            if (!(data[0] == AnonymouseAddressVersion))
                throw new FormatException();

            byte[] p_a_pk = new byte[32];
            byte[] pk_enc = new byte[32];
            Buffer.BlockCopy(data, 1, p_a_pk, 0, 32);
            Buffer.BlockCopy(data, 33, pk_enc, 0, 32);

            return new PaymentAddress(new UInt256(p_a_pk), new UInt256(pk_enc));
        }

        public static StealthKeyPair ToStealthKeyPair(string address)
        {
            byte[] data = address.Base58CheckDecode();
            if (data.Length != 67)
                throw new FormatException();
            if (!(data[0] == StealthAddressVersion))
                throw new FormatException();

            byte[] payloadKeyHash = new byte[33];
            byte[] viewKeyHash = new byte[33];
            Buffer.BlockCopy(data, 1, payloadKeyHash, 0, 33);
            Buffer.BlockCopy(data, 34, viewKeyHash, 0, 33);

            return new StealthKeyPair(new byte[0], new byte[0], payloadKeyHash, viewKeyHash);
        }

        public static UInt160 ToScriptHash(string address)
        {
            byte[] data = address.Base58CheckDecode();
            if (!(data.Length == 21 || data.Length == 65))
                throw new FormatException();
            if (!(data[0] == AddressVersion || data[0] == AnonymouseAddressVersion))
                throw new FormatException();
            if (data[0] == AddressVersion)
            {
                return new UInt160(data.Skip(1).ToArray());
            }
            else
            {
                return new UInt160();
            }
        }

        public bool VerifyPassword(string password)
        {
            return password.ToAesKey().Sha256().SequenceEqual(LoadStoredData("PasswordHash"));
        }

        public bool VerifyPassword(SecureString password)
        {
            return password.ToAesKey().Sha256().SequenceEqual(LoadStoredData("PasswordHash"));
        }

        private static byte[] XOR(byte[] x, byte[] y)
        {
            if (x.Length != y.Length) throw new ArgumentException();
            return x.Zip(y, (a, b) => (byte)(a ^ b)).ToArray();
        }

        public static List<JSTransactionOutput> DecryptJS(AnonymousContractTransaction tx, int id, byte[] privKey, UInt256 jsPubKey)
        {
            IntPtr byRet = SnarkDllApi.Snark_FindMyNotes(tx.byJoinSplit[id], privKey, jsPubKey.ToArray());

            List<JSTransactionOutput> ret = new List<JSTransactionOutput>();

            byte[] byCount = new byte[1];
            System.Runtime.InteropServices.Marshal.Copy(byRet, byCount, 0, 1);
            if (byCount[0] > 0)
            {
                int bodySize = byCount[0] * 169;
                byte[] byBody = new byte[bodySize + 1];
                System.Runtime.InteropServices.Marshal.Copy(byRet, byBody, 0, bodySize + 1);

                for (int i = 0; i < byCount[0]; i++)
                {
                    byte index = byBody[0 + 1];

                    JSTransactionOutput outItem = new JSTransactionOutput();

                    outItem.AssetId = tx.Asset_ID(id);
                    outItem.Value = new Fixed8(BitConverter.ToInt64(byBody, 33 + 1));

                    byte[] buffer = new byte[32];
                    System.Runtime.InteropServices.Marshal.Copy(byRet + 41 + 1, buffer, 0, 32);
                    UInt256 a_pk = new UInt256(buffer);

                    byte[] buffer1 = new byte[32];
                    System.Runtime.InteropServices.Marshal.Copy(byRet + 73 + 1, buffer1, 0, 32);
                    UInt256 pk_enc = new UInt256(buffer1);

                    outItem.addr = new PaymentAddress(a_pk, pk_enc);

                    outItem.ScriptHash = outItem.addr.ToArray().ToScriptHash();

                    byte[] buffer2 = new byte[32];
                    System.Runtime.InteropServices.Marshal.Copy(byRet + 105 + 1, buffer2, 0, 32);
                    outItem.rho = new UInt256(buffer2);

                    byte[] buffer3 = new byte[32];
                    System.Runtime.InteropServices.Marshal.Copy(byRet + 137 + 1, buffer3, 0, 32);
                    outItem.r = new UInt256(buffer3);

                    ret.Add(outItem);
                }
            }
            return ret;
        }
    }
}
