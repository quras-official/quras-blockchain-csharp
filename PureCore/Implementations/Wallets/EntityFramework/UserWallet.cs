using Microsoft.EntityFrameworkCore;
using Pure.Core;
using Pure.IO;
using Pure.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;

using PureCore.Wallets.AnonymousKey.Key;
using CoreTransaction = Pure.Core.Transaction;
using WalletCoin = Pure.Wallets.Coin;
using JSWalletCoin = Pure.Wallets.JSCoin;
using RCTWalletCoin = Pure.Wallets.RCTCoin;
using WalletKeyPair = Pure.Wallets.KeyPair;
using WalletKeyPairBase = Pure.Wallets.KeyPairBase;
using StealthKeyPair = Pure.Wallets.StealthKey.StealthKeyPair;
using Pure.Wallets.StealthKey;

namespace Pure.Implementations.Wallets.EntityFramework
{
    public class UserWallet : Wallet
    {
        public event EventHandler<IEnumerable<TransactionInfo>> TransactionsChanged;

        protected override Version Version => GetType().GetTypeInfo().Assembly.GetName().Version;

        protected UserWallet(string path, string password, bool create, bool checkpwd = false)
            : base(path, password, create, checkpwd)
        {
        }

        protected UserWallet(string path, SecureString password, bool create, bool checkpwd = false)
            : base(path, password, create, checkpwd)
        {
        }

        public override void AddContract(VerificationContract contract)
        {
            base.AddContract(contract);
            using (WalletDataContext ctx = new WalletDataContext(DbPath))
            {
                Contract db_contract = ctx.Contracts.FirstOrDefault(p => p.ScriptHash.SequenceEqual(contract.ScriptHash.ToArray()));
                if (db_contract != null)
                {
                    db_contract.PublicKeyHash = contract.PublicKeyHash.ToArray();
                }
                else
                {
                    Address db_address = ctx.Addresses.FirstOrDefault(p => p.ScriptHash.SequenceEqual(contract.ScriptHash.ToArray()));
                    if (db_address == null)
                    {
                        ctx.Addresses.Add(new Address
                        {
                            ScriptHash = contract.ScriptHash.ToArray()
                        });
                    }
                    ctx.Contracts.Add(new Contract
                    {
                        RawData = contract.ToArray(),
                        ScriptHash = contract.ScriptHash.ToArray(),
                        PublicKeyHash = contract.PublicKeyHash.ToArray()
                    });
                }
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                }
            }
        }

        public override void AddWatchOnly(UInt160 scriptHash)
        {
            base.AddWatchOnly(scriptHash);
            using (WalletDataContext ctx = new WalletDataContext(DbPath))
            {
                if (ctx.Addresses.All(p => !p.ScriptHash.SequenceEqual(scriptHash.ToArray())))
                {
                    ctx.Addresses.Add(new Address
                    {
                        ScriptHash = scriptHash.ToArray()
                    });
                    try
                    {
                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string str = ex.Message;
                    }
                }
            }
        }

        protected override void BuildDatabase()
        {
            using (WalletDataContext ctx = new WalletDataContext(DbPath))
            {
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();
            }
        }

        public static UserWallet Create(string path, string password, KeyType nVersion = KeyType.Transparent)
        {
            UserWallet wallet = new UserWallet(path, password, true);
            wallet.CreateKey(nVersion);
            return wallet;
        }

        public static UserWallet Create(string path, SecureString password, KeyType nVersion = KeyType.Transparent)
        {
            UserWallet wallet = new UserWallet(path, password, true);
            wallet.CreateKey(nVersion);
            return wallet;
        }

        public override StealthKeyPair CreateKey(byte[] payloadPrivKey, byte[] viewPrivKey)
        {
            StealthKeyPair account = base.CreateKey(payloadPrivKey, viewPrivKey);
            OnCreateAccount(account);
            AddContract(VerificationContract.CreateRingSignatureContract(account.ToStelathPubKeys()));
            return account;
        }

        public override WalletKeyPair CreateKey(byte[] privateKey, KeyType nVersion = KeyType.Transparent)
        {
            if (nVersion == KeyType.Transparent)
            {
                WalletKeyPair account = base.CreateKey(privateKey, nVersion);
                OnCreateAccount(account);
                AddContract(VerificationContract.CreateSignatureContract(account.PublicKey));
                return account;
            }
            else if (nVersion == KeyType.Anonymous)
            {
                WalletKeyPair account = base.CreateKey(privateKey, nVersion);
                SpendingKey sKey;

                sKey = new SpendingKey(new UInt256(privateKey));
                OnCreateAccount(account);
                AddContract(VerificationContract.CreateSignatureAnonymousContract(account.PublicKey, sKey.address()));
                return account;
            }
            else
            {
                return null;
            }
        }

        public override bool DeleteKey(UInt160 publicKeyHash)
        {
            bool flag = base.DeleteKey(publicKeyHash);
            if (flag)
            {
                using (WalletDataContext ctx = new WalletDataContext(DbPath))
                {
                    Account account = ctx.Accounts.FirstOrDefault(p => p.PublicKeyHash.SequenceEqual(publicKeyHash.ToArray()));
                    if (account != null)
                    {
                        foreach (byte[] hash in ctx.Contracts.Where(p => p.PublicKeyHash.SequenceEqual(publicKeyHash.ToArray())).Select(p => p.ScriptHash))
                        {
                            Address address = ctx.Addresses.FirstOrDefault(p => p.ScriptHash.SequenceEqual(hash));
                            if (address != null) ctx.Addresses.Remove(address);
                        }
                        ctx.Accounts.Remove(account);
                        try
                        {
                            ctx.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }
                    }
                }
            }
            return flag;
        }

        public override bool DeleteAddress(UInt160 scriptHash)
        {
            bool flag = base.DeleteAddress(scriptHash);
            if (flag)
            {
                using (WalletDataContext ctx = new WalletDataContext(DbPath))
                {
                    Address address = ctx.Addresses.FirstOrDefault(p => p.ScriptHash.SequenceEqual(scriptHash.ToArray()));
                    if (address != null)
                    {
                        ctx.Addresses.Remove(address);
                        try
                        {
                            ctx.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }
                    }
                }
            }
            return flag;
        }

        public override WalletCoin[] FindUnspentCoins(UInt256 asset_id, Fixed8 amount, string from_addr = null)
        {
            if (from_addr == null)
            {
                return FindUnspentCoins(FindUnspentCoins().ToArray().Where(p => GetContract(p.Output.ScriptHash).IsStandard), asset_id, amount) ?? base.FindUnspentCoins(asset_id, amount);
            }
            else
            {
                return FindUnspentCoins(FindUnspentCoinsFrom(from_addr).ToArray().Where(p => GetContract(p.Output.ScriptHash).IsStandard), asset_id, amount) ?? base.FindUnspentCoins(asset_id, amount, from_addr);
            }
        }

        public override JSWalletCoin[] FindUnspentNotes(UInt256 asset_id, Fixed8 amount, string from_addr)
        {
            return FindUnspentNotes(FindUnspentNotesFrom(from_addr).ToArray().Where(p => GetContract(p.Output.ScriptHash).IsStandard), asset_id, amount) ?? base.FindUnspentNotes(asset_id, amount, from_addr);
        }

        private static IEnumerable<TransactionInfo> GetTransactionInfo(IEnumerable<Transaction> transactions)
        {
            return transactions.Select(p => new TransactionInfo
            {
                Transaction = CoreTransaction.DeserializeFrom(p.RawData),
                Height = p.Height,
                Time = p.Time
            });
        }

        public override IEnumerable<T> GetTransactions<T>()
        {
            using (WalletDataContext ctx = new WalletDataContext(DbPath))
            {
                IQueryable<Transaction> transactions = ctx.Transactions;
                if (typeof(T).GetTypeInfo().IsSubclassOf(typeof(CoreTransaction)))
                {
                    TransactionType type = (TransactionType)Enum.Parse(typeof(TransactionType), typeof(T).Name);
                    transactions = transactions.Where(p => p.Type == type);
                }
                return transactions.Select(p => p.RawData).ToArray().Select(p => (T)CoreTransaction.DeserializeFrom(p));
            }
        }

        public static Version GetVersion(string path)
        {
            byte[] buffer;
            using (WalletDataContext ctx = new WalletDataContext(path))
            {
                buffer = ctx.Keys.FirstOrDefault(p => p.Name == "Version")?.Value;
            }
            if (buffer == null) return new Version(0, 0);
            int major = buffer.ToInt32(0);
            int minor = buffer.ToInt32(4);
            int build = buffer.ToInt32(8);
            int revision = buffer.ToInt32(12);
            return new Version(major, minor, build, revision);
        }

        protected override IEnumerable<WalletKeyPairBase> LoadKeyPairs()
        {
            using (WalletDataContext ctx = new WalletDataContext(DbPath))
            {
                foreach (Account item in ctx.Accounts.Select(p => p))
                {
                    if ((KeyType)item.nVersion == KeyType.Anonymous || (KeyType)item.nVersion ==KeyType.Transparent)
                    {
                        byte[] decryptedPrivateKey = DecryptPrivateKey(item.PrivateKeyEncrypted);
                        WalletKeyPair account = new WalletKeyPair(decryptedPrivateKey, (KeyType)item.nVersion);
                        Array.Clear(decryptedPrivateKey, 0, decryptedPrivateKey.Length);
                        yield return account;
                    }
                    else if ((KeyType)item.nVersion == KeyType.Stealth)
                    {
                        byte[] decryptedPrivateKey = DecryptPrivateKey(item.PrivateKeyEncrypted);
                        byte[] decryptedViewKey = DecryptPrivateKey(item.PrivateViewKeyEncrypted);
                        StealthKeyPair account = new StealthKeyPair(decryptedPrivateKey, decryptedViewKey);
                        Array.Clear(decryptedPrivateKey, 0, decryptedPrivateKey.Length);
                        Array.Clear(decryptedViewKey, 0, decryptedViewKey.Length);
                        yield return account;
                    }
                }
            }
        }

        protected override IEnumerable<WalletCoin> LoadCoins()
        {
            using (WalletDataContext ctx = new WalletDataContext(DbPath))
            {
                foreach (Coin coin in ctx.Coins)
                {
                    yield return new WalletCoin
                    {
                        Reference = new CoinReference
                        {
                            PrevHash = new UInt256(coin.TxId),
                            PrevIndex = coin.Index
                        },
                        Output = new TransactionOutput
                        {
                            AssetId = new UInt256(coin.AssetId),
                            Value = new Fixed8(coin.Value),
                            ScriptHash = new UInt160(coin.ScriptHash),
                        },
                        State = coin.State
                    };
                }
            }
        }

        protected override IEnumerable<JSWalletCoin> LoadJSCoins()
        {
            using (WalletDataContext ctx = new WalletDataContext(DbPath))
            {
                foreach (JSCoin coin in ctx.JSCoins)
                {
                    yield return new JSWalletCoin
                    {
                        Reference = new JSCoinReference
                        {
                            PrevHash = new UInt256(coin.TxId),
                            PrevJsId = coin.JsId,
                            PrevIndex = coin.Index
                        },
                        Output = new JSTransactionOutput
                        {
                            AssetId = new UInt256(coin.AssetId),
                            Value = new Fixed8(coin.Value),
                            ScriptHash = new UInt160(coin.ScriptHash),
                            addr = GetContract(new UInt160(coin.ScriptHash)).paymentAddress,
                            r = new UInt256(coin.r),
                            rho = new UInt256(coin.rho),
                            witness = coin.Witness,
                            witness_height = coin.WitnessHeight
                        },
                        State = coin.State
                    };
                }
            }
        }

        protected override IEnumerable<RCTWalletCoin> LoadRCTCoins()
        {
            using (WalletDataContext ctx = new WalletDataContext(DbPath))
            {
                foreach (RCTCoin coin in ctx.RCTCoins)
                {
                    yield return new RCTWalletCoin
                    {
                        Reference = new RCTCoinReference
                        {
                            PrevHash = new UInt256(coin.TxId),
                            TxRCTHash = Cryptography.ECC.ECPoint.DecodePoint(coin.TxRCTHash, Cryptography.ECC.ECCurve.Secp256r1),
                            PrevRCTSigId = coin.RctID,
                            PrevRCTSigIndex = coin.Index
                        },
                        Output = new RCTransactionOutput
                        {
                            AssetId = new UInt256(coin.AssetId),
                            Value = new Fixed8(coin.Value),
                            PubKey = Cryptography.ECC.ECPoint.DecodePoint(coin.PubKey, Cryptography.ECC.ECCurve.Secp256r1),
                            ScriptHash = new UInt160(coin.ScriptHash)
                        },
                        State = coin.State
                    };
                }
            }
        }

        protected override IEnumerable<VerificationContract> LoadContracts()
        {
            using (WalletDataContext ctx = new WalletDataContext(DbPath))
            {
                foreach (Contract contract in ctx.Contracts)
                {
                    yield return contract.RawData.AsSerializable<VerificationContract>();
                }
            }
        }

        protected override byte[] LoadStoredData(string name)
        {
            using (WalletDataContext ctx = new WalletDataContext(DbPath))
            {
                try
                {
                    return ctx.Keys.FirstOrDefault(p => p.Name == name)?.Value;
                }
                catch
                {
                    throw new FormatException("Wallet is not correct");
                }
            }
        }

        public IEnumerable<TransactionInfo> LoadTransactions()
        {
            using (WalletDataContext ctx = new WalletDataContext(DbPath))
            {
                return GetTransactionInfo(ctx.Transactions.ToArray());
            }
        }

        protected override IEnumerable<UInt160> LoadWatchOnly()
        {
            using (WalletDataContext ctx = new WalletDataContext(DbPath))
            {
                foreach (byte[] hash in ctx.Addresses.Where(p => !ctx.Contracts.Select(q => q.ScriptHash).Contains(p.ScriptHash)).Select(p => p.ScriptHash))
                    yield return new UInt160(hash);
            }
        }

        public static void Migrate(string path_old, string path_new)
        {
            Version current_version = typeof(UserWallet).GetTypeInfo().Assembly.GetName().Version;
            using (WalletDataContext ctx_old = new WalletDataContext(path_old))
            using (WalletDataContext ctx_new = new WalletDataContext(path_new))
            {
                ctx_new.Database.EnsureCreated();
                ctx_new.Accounts.AddRange(ctx_old.Accounts);
                ctx_new.Addresses.AddRange(ctx_old.Contracts.Select(p => new Address { ScriptHash = p.ScriptHash }));
                ctx_new.Contracts.AddRange(ctx_old.Contracts);
                ctx_new.Keys.AddRange(ctx_old.Keys.Where(p => p.Name != "Height" && p.Name != "Version"));
                SaveStoredData(ctx_new, "Height", new byte[sizeof(int)]);
                SaveStoredData(ctx_new, "Version", new[] { current_version.Major, current_version.Minor, current_version.Build, current_version.Revision }.Select(p => BitConverter.GetBytes(p)).SelectMany(p => p).ToArray());
                ctx_new.SaveChanges();
            }
        }

        private void OnCoinsChanged(WalletDataContext ctx, 
                    IEnumerable<WalletCoin> added, 
                    IEnumerable<WalletCoin> changed, 
                    IEnumerable<WalletCoin> deleted,
                    IEnumerable<JSWalletCoin> jsadded,
                    IEnumerable<JSWalletCoin> jschanged,
                    IEnumerable<JSWalletCoin> jsdeleted,
                    IEnumerable<JSWalletCoin> jswitnesschanged,
                    IEnumerable<RCTWalletCoin> rctadded,
                    IEnumerable<RCTWalletCoin> rctchanged,
                    IEnumerable<RCTWalletCoin> rctdeleted)
        {
            #region Transparent Coin
            foreach (WalletCoin coin in added)
            {
                try
                {
                    ctx.Coins.Add(new Coin
                    {
                        TxId = coin.Reference.PrevHash.ToArray(),
                        Index = coin.Reference.PrevIndex,
                        AssetId = coin.Output.AssetId.ToArray(),
                        Value = coin.Output.Value.GetData(),
                        ScriptHash = coin.Output.ScriptHash.ToArray(),
                        State = coin.State
                    });
                    ctx.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
            foreach (WalletCoin coin in changed)
            {
                ctx.Coins.First(p => p.TxId.SequenceEqual(coin.Reference.PrevHash.ToArray()) && p.Index == coin.Reference.PrevIndex).State = coin.State;
                try
                {
                    ctx.Coins.Update(ctx.Coins.First(p => p.TxId.SequenceEqual(coin.Reference.PrevHash.ToArray()) && p.Index == coin.Reference.PrevIndex));
                    ctx.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
            foreach (WalletCoin coin in deleted)
            {
                try
                {
                    Coin unspent_coin = ctx.Coins.FirstOrDefault(p => p.TxId.SequenceEqual(coin.Reference.PrevHash.ToArray()) && p.Index == coin.Reference.PrevIndex);
                    ctx.Coins.Remove(unspent_coin);
                    ctx.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
            #endregion

            #region ZK-SNARKS Coin
            foreach (JSWalletCoin coin in jsadded)
            {
                ctx.JSCoins.Add(new JSCoin
                {
                    TxId = coin.Reference.PrevHash.ToArray(),
                    JsId = coin.Reference.PrevJsId,
                    Index = coin.Reference.PrevIndex,
                    AssetId = coin.Output.AssetId.ToArray(),
                    Value = coin.Output.Value.GetData(),
                    ScriptHash = coin.Output.ScriptHash.ToArray(),
                    State = coin.State,
                    r = coin.Output.r.ToArray(),
                    rho = coin.Output.rho.ToArray(),
                    Witness = coin.Output.witness,
                    WitnessHeight = coin.Output.witness_height,
                    CMTreeHeight = coin.Output.cmtree_height
                });

                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
            foreach (JSWalletCoin coin in jschanged)
            {
                ctx.JSCoins.First(p => p.TxId.SequenceEqual(coin.Reference.PrevHash.ToArray()) && p.Index == coin.Reference.PrevIndex && p.JsId == coin.Reference.PrevJsId).State = coin.State;
                ctx.JSCoins.First(p => p.TxId.SequenceEqual(coin.Reference.PrevHash.ToArray()) && p.Index == coin.Reference.PrevIndex && p.JsId == coin.Reference.PrevJsId).Witness = coin.Output.witness;
                ctx.JSCoins.First(p => p.TxId.SequenceEqual(coin.Reference.PrevHash.ToArray()) && p.Index == coin.Reference.PrevIndex && p.JsId == coin.Reference.PrevJsId).WitnessHeight = coin.Output.witness_height;
                ctx.JSCoins.First(p => p.TxId.SequenceEqual(coin.Reference.PrevHash.ToArray()) && p.Index == coin.Reference.PrevIndex && p.JsId == coin.Reference.PrevJsId).CMTreeHeight = coin.Output.cmtree_height;
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
            foreach (JSWalletCoin coin in jsdeleted)
            {
                JSCoin unspent_coin = ctx.JSCoins.FirstOrDefault(p => p.TxId.SequenceEqual(coin.Reference.PrevHash.ToArray()) && p.Index == coin.Reference.PrevIndex && p.JsId == coin.Reference.PrevJsId);
                ctx.JSCoins.Remove(unspent_coin);
            }
            foreach (JSWalletCoin coin in jswitnesschanged)
            {
                JSCoin update_coin = ctx.JSCoins.FirstOrDefault(p => p.TxId.SequenceEqual(coin.Reference.PrevHash.ToArray()) && p.Index == coin.Reference.PrevIndex && p.JsId == coin.Reference.PrevJsId);
                if (update_coin != null)
                {  
                    try
                    {
                        update_coin.Witness = coin.Output.witness;
                        update_coin.WitnessHeight = coin.Output.witness_height;
                        ctx.JSCoins.Update(update_coin);
                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                    }
                }
            }
            #endregion

            #region RingCT Coin
            foreach (RCTWalletCoin coin in rctadded)
            {
                ctx.RCTCoins.Add(new RCTCoin
                {
                    TxId = coin.Reference.PrevHash.ToArray(),
                    TxRCTHash = coin.Reference.TxRCTHash.ToArray(),
                    RctID = coin.Reference.PrevRCTSigId,
                    Index = coin.Reference.PrevRCTSigIndex,
                    AssetId = coin.Output.AssetId.ToArray(),
                    Value = coin.Output.Value.GetData(),
                    PubKey = coin.Output.PubKey.ToArray(),
                    ScriptHash = coin.Output.ScriptHash.ToArray(),
                    State = coin.State
                });

                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
            
            foreach (RCTWalletCoin coin in rctchanged)
            {
                ctx.RCTCoins.First(p => p.TxRCTHash.SequenceEqual(coin.Reference.TxRCTHash.ToArray()) && p.RctID == coin.Reference.PrevRCTSigId && p.Index == coin.Reference.PrevRCTSigIndex).State = coin.State;
                
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }

            foreach (RCTWalletCoin coin in rctdeleted)
            {
                RCTCoin unspent_coin = ctx.RCTCoins.FirstOrDefault(p => p.TxId.SequenceEqual(coin.Reference.PrevHash.ToArray()) && p.RctID == coin.Reference.PrevRCTSigId && p.Index == coin.Reference.PrevRCTSigIndex);
                ctx.RCTCoins.Remove(unspent_coin);
            }
            #endregion
        }

        private void OnCreateAccount(StealthKeyPair account)
        {
            byte[] decryptedPayloadPrivKey = new byte[96];
            Buffer.BlockCopy(account.PayloadPubKey.EncodePoint(false), 1, decryptedPayloadPrivKey, 0, 64);

            using (account.DecryptPayloadKey())
            {
                Buffer.BlockCopy(account.PayloadPrivKey, 0, decryptedPayloadPrivKey, 64, 32);
            }
            byte[] encryptedPayloadPrivKey = EncryptPrivateKey(decryptedPayloadPrivKey);
            Array.Clear(decryptedPayloadPrivKey, 0, decryptedPayloadPrivKey.Length);

            byte[] decryptedViewPrivKey = new byte[96];
            Buffer.BlockCopy(account.ViewPubKey.EncodePoint(false), 1, decryptedViewPrivKey, 0, 64);

            using (account.DecryptViewKey())
            {
                Buffer.BlockCopy(account.ViewPrivKey, 0, decryptedViewPrivKey, 64, 32);
            }
            byte[] encryptedViewPrivKey = EncryptPrivateKey(decryptedViewPrivKey);
            Array.Clear(decryptedViewPrivKey, 0, decryptedViewPrivKey.Length);

            using (WalletDataContext ctx = new WalletDataContext(DbPath))
            {
                Account db_account = ctx.Accounts.FirstOrDefault(p => p.PublicKeyHash.SequenceEqual(account.PublicKeyHash.ToArray()));
                if (db_account == null)
                {
                    db_account = ctx.Accounts.Add(new Account
                    {
                        nVersion = (int)account.nVersion,
                        PrivateKeyEncrypted = encryptedPayloadPrivKey,
                        PublicKeyHash = account.PublicKeyHash.ToArray(),
                        PrivateViewKeyEncrypted = encryptedViewPrivKey
                    }).Entity;
                }
                else
                {
                    db_account.nVersion = (int)account.nVersion;
                    db_account.PrivateKeyEncrypted = encryptedPayloadPrivKey;
                    db_account.PrivateViewKeyEncrypted = encryptedViewPrivKey;
                }

                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                }
            }
        }

        private void OnCreateAccount(WalletKeyPair account)
        {
            byte[] decryptedPrivateKey = new byte[96];
            Buffer.BlockCopy(account.PublicKey.EncodePoint(false), 1, decryptedPrivateKey, 0, 64);
            using (account.Decrypt())
            {
                Buffer.BlockCopy(account.PrivateKey, 0, decryptedPrivateKey, 64, 32);
            }
            byte[] encryptedPrivateKey = EncryptPrivateKey(decryptedPrivateKey);
            Array.Clear(decryptedPrivateKey, 0, decryptedPrivateKey.Length);
            using (WalletDataContext ctx = new WalletDataContext(DbPath))
            {
                Account db_account = ctx.Accounts.FirstOrDefault(p => p.PublicKeyHash.SequenceEqual(account.PublicKeyHash.ToArray()));
                if (db_account == null)
                {
                    db_account = ctx.Accounts.Add(new Account
                    {
                        nVersion = (int)account.nVersion,
                        PrivateKeyEncrypted = encryptedPrivateKey,
                        PublicKeyHash = account.PublicKeyHash.ToArray()
                    }).Entity;
                }
                else
                {
                    db_account.nVersion = (int)account.nVersion;
                    db_account.PrivateKeyEncrypted = encryptedPrivateKey;
                }
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                }
            }
        }

        protected override void OnProcessNewBlock(Block block,
                                            IEnumerable<WalletCoin> added,
                                            IEnumerable<WalletCoin> changed,
                                            IEnumerable<WalletCoin> deleted,
                                            IEnumerable<JSWalletCoin> jsadded,
                                            IEnumerable<JSWalletCoin> jschanged,
                                            IEnumerable<JSWalletCoin> jsdeleted,
                                            IEnumerable<JSWalletCoin> jswitnesschanged,
                                            IEnumerable<RCTWalletCoin> rctadded,
                                            IEnumerable<RCTWalletCoin> rctchanged,
                                            IEnumerable<RCTWalletCoin> rctdeleted)
        {
            Transaction[] tx_changed = null;
            using (WalletDataContext ctx = new WalletDataContext(DbPath))
            {
                foreach (CoreTransaction tx in block.Transactions.Where(p => IsWalletTransaction(p)))
                {
                    Transaction db_tx = ctx.Transactions.FirstOrDefault(p => p.Hash.SequenceEqual(tx.Hash.ToArray()));
                    if (db_tx == null)
                    {
                        ctx.Transactions.Add(new Transaction
                        {
                            Hash = tx.Hash.ToArray(),
                            Type = tx.Type,
                            RawData = tx.ToArray(),
                            Height = block.Index,
                            Time = block.Timestamp.ToDateTime()
                        });
                    }
                    else
                    {
                        db_tx.Height = block.Index;
                    }
                }

                tx_changed = ctx.ChangeTracker.Entries<Transaction>().Where(p => p.State != EntityState.Unchanged).Select(p => p.Entity).ToArray();

                OnCoinsChanged(ctx, added, changed, deleted, jsadded, jschanged, jsdeleted, jswitnesschanged, rctadded, rctchanged, rctdeleted);
                if (block.Index == Blockchain.Default.Height || ctx.ChangeTracker.Entries().Any())
                {
                    foreach (Transaction db_tx in ctx.Transactions.Where(p => !p.Height.HasValue))
                        if (block.Transactions.Any(p => p.Hash == new UInt256(db_tx.Hash)))
                        {
                            db_tx.Height = block.Index;
                        }
                            
                    ctx.Keys.First(p => p.Name == "Height").Value = BitConverter.GetBytes(WalletHeight);
                    ctx.Keys.First(p => p.Name == "CmMerkleTree").Value = GetCmMerkleTreeInBytes();

                    try
                    {
                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string str = ex.Message;
                    } 
                }
            }
            if (tx_changed?.Length > 0)
                TransactionsChanged?.Invoke(this, GetTransactionInfo(tx_changed));
        }

        protected override void OnSaveTransaction(CoreTransaction tx, 
                                                    IEnumerable<WalletCoin> added, 
                                                    IEnumerable<WalletCoin> changed, 
                                                    IEnumerable<JSWalletCoin> jsadded, 
                                                    IEnumerable<JSWalletCoin> jschanged, 
                                                    IEnumerable<JSWalletCoin> jsdeleted, 
                                                    IEnumerable<JSWalletCoin> jswitnesschanged,
                                                    IEnumerable<RCTWalletCoin> rctadded,
                                                    IEnumerable<RCTWalletCoin> rctchanged,
                                                    IEnumerable<RCTWalletCoin> rctdeleted)
        {
            Transaction tx_changed = null;
            using (WalletDataContext ctx = new WalletDataContext(DbPath))
            {
                if (IsWalletTransaction(tx))
                {
                    tx_changed = ctx.Transactions.Add(new Transaction
                    {
                        Hash = tx.Hash.ToArray(),
                        Type = tx.Type,
                        RawData = tx.ToArray(),
                        Height = null,
                        Time = DateTime.Now
                    }).Entity;
                }
                OnCoinsChanged(ctx, added, changed, Enumerable.Empty<WalletCoin>(),
                                jsadded,
                                jschanged, 
                                jsdeleted,
                                jswitnesschanged,
                                rctadded,
                                rctchanged,
                                rctdeleted);

                try
                {
                    ctx.SaveChanges();
                }
                catch(Exception ex)
                {
                    string str = ex.Message;
                }
            }
            if (tx_changed != null)
                TransactionsChanged?.Invoke(this, GetTransactionInfo(new[] { tx_changed }));
        }

        public static UserWallet Open(string path, string password)
        {
            return new UserWallet(path, password, false);
        }

        public static UserWallet Open(string path, SecureString password)
        {
            return new UserWallet(path, password, false);
        }

        public static UserWallet CheckPassword(string path, string password)
        {
            return new UserWallet(path, password, false, true);
        }

        public override void Rebuild()
        {
            lock (SyncRoot)
            {
                base.Rebuild();
                using (WalletDataContext ctx = new WalletDataContext(DbPath))
                {
                    ctx.Keys.First(p => p.Name == "Height").Value = BitConverter.GetBytes(0);

                    ctx.Database.ExecuteSqlCommand($"DELETE FROM [Transaction]");//{nameof(Transaction)}
                    ctx.Database.ExecuteSqlCommand($"DELETE FROM [Coin]");//{nameof(Coin)}
                    ctx.Database.ExecuteSqlCommand($"DELETE FROM [JSCoin]");//{nameof(JSCoin)}
                    ctx.Database.ExecuteSqlCommand($"DELETE FROM [RCTCoin]");

                    byte[] cmTree = { 0x00, 0x00, 0x00 };   // Commitment Merkle Tree Initial bytes.
                    ctx.Keys.First(p => p.Name == "CmMerkleTree").Value = cmTree;   // Initialize the CmMerkleTree.

                    try
                    {
                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string strEx = ex.Message;
                    }
                }
            }
        }

        protected override void SaveStoredData(string name, byte[] value)
        {
            using (WalletDataContext ctx = new WalletDataContext(DbPath))
            {
                SaveStoredData(ctx, name, value);
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                }
            }
        }

        private static void SaveStoredData(WalletDataContext ctx, string name, byte[] value)
        {
            Key key = ctx.Keys.FirstOrDefault(p => p.Name == name);
            if (key == null)
            {
                ctx.Keys.Add(new Key
                {
                    Name = name,
                    Value = value
                });
            }
            else
            {
                key.Value = value;
            }
        }
    }
}
