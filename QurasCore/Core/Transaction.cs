using Quras.Cryptography;
using Quras.Implementations.Wallets.EntityFramework;
using Quras.IO;
using Quras.IO.Caching;
using Quras.IO.Json;
using Quras.Network;
using Quras.VM;
using Quras.Wallets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Quras.Core
{
    public abstract class Transaction : IEquatable<Transaction>, IInventory
    {
        private const int MaxTransactionAttributes = 16;

        private static ReflectionCache<byte> ReflectionCache = ReflectionCache<byte>.CreateFromEnum<TransactionType>();

        public readonly TransactionType Type;

        public byte Version;

        public TransactionAttribute[] Attributes;

        public CoinReference[] Inputs;

        public TransactionOutput[] Outputs;

        public Witness[] Scripts { get; set; }

        public bool is_consensus_mempool = false;

        private UInt256 _hash = null;

        public UInt256 Hash
        {
            get
            {
                if (_hash == null)
                {
                    _hash = new UInt256(Crypto.Default.Hash256(this.GetHashData(true)));
                }
                return _hash;
            }
        }

        InventoryType IInventory.InventoryType => InventoryType.TX;

        private Fixed8 _network_fee = -Fixed8.Satoshi;
        public virtual Fixed8 NetworkFee
        {
            get
            {
                if (_network_fee == -Fixed8.Satoshi)
                {
                    Fixed8 input = References.Values.Where(p => p.AssetId.Equals(Blockchain.UtilityToken.Hash)).Sum(p => p.Value);
                    Fixed8 output = Outputs.Where(p => p.AssetId.Equals(Blockchain.UtilityToken.Hash)).Sum(p => p.Value);
                    _network_fee = input - output - SystemFee;
                }
                return _network_fee;
            }
        }

        private IReadOnlyDictionary<CoinReference, TransactionOutput> _references;
 
        public IReadOnlyDictionary<CoinReference, TransactionOutput> References
        {
            get
            {
                if (_references == null)
                {
                    Dictionary<CoinReference, TransactionOutput> dictionary = new Dictionary<CoinReference, TransactionOutput>();

                    if (Inputs == null) return _references;

                    foreach (var group in Inputs.GroupBy(p => p.PrevHash))
                    {
                        Transaction tx = Blockchain.Default.GetTransaction(group.Key);

                        if (tx == null)
                        {
                            if (UserWallet.Default == null)
                                return null;
                            foreach (TransactionInfo info in UserWallet.Default.LoadTransactions())
                            {
                                if (info.Transaction.Hash == group.Key)
                                {
                                    tx = info.Transaction;
                                    break;
                                }
                            }
                        }
                        if (tx == null)
                        {
                            foreach (Transaction info in LocalNode.GetMemoryPool())
                            {
                                if (info.Hash == group.Key)
                                {
                                    tx = info;
                                    break;
                                }
                            }
                        }
                        if (tx == null) return null;
                        foreach (var reference in group.Select(p => new
                        {
                            Input = p,
                            Output = tx.Outputs[p.PrevIndex]
                        }))
                        {
                            dictionary.Add(reference.Input, reference.Output);
                        }
                    }
                    _references = dictionary;
                }
                    
                return _references;
            }
        }

        public virtual int Size => sizeof(TransactionType) + 
                                    sizeof(byte) + 
                                    Attributes.GetVarSize() + 
                                    Inputs.GetVarSize() + 
                                    Outputs.GetVarSize() + 
                                    Scripts.GetVarSize();

        public virtual Fixed8 SystemFee => Settings.Default.SystemFee.TryGetValue(Type, out Fixed8 fee) ? fee : Fixed8.Zero;
        public virtual Fixed8 QrsSystemFee => Fixed8.Zero;

        protected Transaction(TransactionType type)
        {
            this.Type = type;
        }

        void ISerializable.Deserialize(BinaryReader reader)
        {
            ((IVerifiable)this).DeserializeUnsigned(reader);
            Scripts = reader.ReadSerializableArray<Witness>();
            OnDeserialized();
        }

        protected virtual void DeserializeExclusiveData(BinaryReader reader)
        {
        }

        public static Transaction DeserializeFrom(byte[] value, int offset = 0)
        {
            using (MemoryStream ms = new MemoryStream(value, offset, value.Length - offset, false))
            using (BinaryReader reader = new BinaryReader(ms, Encoding.UTF8))
            {
                return DeserializeFrom(reader);
            }
        }

        internal static Transaction DeserializeFrom(BinaryReader reader)
        {
            // Looking for type in reflection cache
            Transaction transaction = ReflectionCache.CreateInstance<Transaction>(reader.ReadByte());
            if (transaction == null) throw new FormatException();

            transaction.DeserializeUnsignedWithoutType(reader);
            transaction.Scripts = reader.ReadSerializableArray<Witness>();
            transaction.OnDeserialized();
            return transaction;
        }

        void IVerifiable.DeserializeUnsigned(BinaryReader reader)
        {
            if ((TransactionType)reader.ReadByte() != Type)
                throw new FormatException();
            DeserializeUnsignedWithoutType(reader);
        }

        private void DeserializeUnsignedWithoutType(BinaryReader reader)
        {
            Version = reader.ReadByte();
            DeserializeExclusiveData(reader);
            Attributes = reader.ReadSerializableArray<TransactionAttribute>(MaxTransactionAttributes);
            Inputs = reader.ReadSerializableArray<CoinReference>();
            Outputs = reader.ReadSerializableArray<TransactionOutput>(ushort.MaxValue + 1);
        }

        public bool Equals(Transaction other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Hash.Equals(other.Hash);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Transaction);
        }

        public override int GetHashCode()
        {
            return Hash.GetHashCode();
        }

        byte[] IScriptContainer.GetMessage()
        {
            return this.GetHashData();
        }

        public virtual UInt160[] GetScriptHashesForVerifying()
        {
            if (References == null) throw new InvalidOperationException();
            HashSet<UInt160> hashes = new HashSet<UInt160>(Inputs.Select(p => References[p].ScriptHash));
            hashes.UnionWith(Attributes.Where(p => p.Usage == TransactionAttributeUsage.Script).Select(p => new UInt160(p.Data)));
            foreach (var group in Outputs.GroupBy(p => p.AssetId))
            {
                AssetState asset = Blockchain.Default.GetAssetState(group.Key);
                if (asset == null) throw new InvalidOperationException();
                if (asset.AssetType.HasFlag(AssetType.DutyFlag))
                {
                    hashes.UnionWith(group.Select(p => p.ScriptHash));
                }
            }
            return hashes.OrderBy(p => p).ToArray();
        }

        public IEnumerable<TransactionResult> GetTransactionResults()
        {
            if (References == null) return null;
            return References.Values.Select(p => new
            {
                AssetId = p.AssetId,
                Value = p.Value
            }).Concat(Outputs.Select(p => new
            {
                AssetId = p.AssetId,
                Value = -p.Value
            })).GroupBy(p => p.AssetId, (k, g) => new TransactionResult
            {
                AssetId = k,
                Amount = g.Sum(p => p.Value)
            }).Where(p => p.Amount != Fixed8.Zero);
        }

        protected virtual void OnDeserialized()
        {
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            ((IVerifiable)this).SerializeUnsigned(writer);
            writer.Write(Scripts);
        }

        protected virtual void SerializeExclusiveData(BinaryWriter writer)
        {
        }

        void IVerifiable.SerializeUnsigned(BinaryWriter writer)
        {
            writer.Write((byte)Type);
            writer.Write(Version);
            SerializeExclusiveData(writer);
            writer.Write(Attributes);
            writer.Write(Inputs);
            writer.Write(Outputs);
        }

        public virtual JObject ToJson()
        {
            JObject json = new JObject();
            json["txid"] = Hash.ToString();
            json["size"] = Size;
            json["type"] = Type;
            json["version"] = Version;
            json["attributes"] = Attributes.Select(p => p.ToJson()).ToArray();
            json["vin"] = Inputs.Select(p => p.ToJson()).ToArray();
            json["vout"] = Outputs.Select((p, i) => p.ToJson((ushort)i)).ToArray();
            json["sys_fee"] = SystemFee.ToString();
            json["net_fee"] = NetworkFee.ToString();
            json["scripts"] = Scripts.Select(p => p.ToJson()).ToArray();
            return json;
        }

        public static Transaction FromJson(JObject jobj)
        {
            Transaction tx = ReflectionCache.CreateInstance<Transaction>((byte)jobj["type"].AsNumber());

            tx._hash = UInt256.Parse(jobj["txid"].AsString());
            tx.Version = (byte)jobj["version"].AsNumber();
            tx.Attributes = ((JArray)jobj["attributes"]).Select(p => TransactionAttribute.FromJson(p)).ToArray();
            tx.Inputs = ((JArray)jobj["vin"]).Select(p => CoinReference.FromJson(p)).ToArray();
            tx.Outputs = ((JArray)jobj["vout"]).Select(p => TransactionOutput.FromJson(p)).ToArray();
            tx.Scripts = ((JArray)jobj["scripts"]).Select(p => Witness.FromJson(p)).ToArray();

            return tx;
        }

        public virtual JObject ToJsonString()
        {
            JObject json = new JObject();
            json["type"] = (byte)Type;
            json["version"] = Version;
            json["attributes"] = Attributes.Select(p => p.ToJson()).ToArray();
            json["inputs"] = Inputs.Select(p => p.ToJsonString()).ToArray();
            json["outputs"] = Outputs.Select(p => p.ToJsonString()).ToArray();
            json["scripts"] = Scripts.Select(p => p.ToJsonString()).ToArray();
            return json;
        }
        public static Transaction FromJsonString(JObject jobj)
        {
            Transaction tx = ReflectionCache.CreateInstance<Transaction>((byte)jobj["type"].AsNumber());

            tx.Version = (byte)jobj["version"].AsNumber();
            tx.Attributes = ((JArray)jobj["attributes"]).Select(p => TransactionAttribute.FromJson(p)).ToArray();
            tx.Inputs = ((JArray)jobj["inputs"]).Select(p => CoinReference.FromJsonString(p)).ToArray();
            tx.Outputs = ((JArray)jobj["outputs"]).Select(p => TransactionOutput.FromJsonString(p)).ToArray();
            tx.Scripts = ((JArray)jobj["scripts"]).Select(p => Witness.FromJsonString(p)).ToArray();

            return tx;
        }
        public virtual void FromJsonObject(JObject jobj)
        {
            Version = (byte)jobj["version"].AsNumber();
            Attributes = ((JArray)jobj["attributes"]).Select(p => TransactionAttribute.FromJson(p)).ToArray();
            Inputs = ((JArray)jobj["inputs"]).Select(p => CoinReference.FromJsonString(p)).ToArray();
            Outputs = ((JArray)jobj["outputs"]).Select(p => TransactionOutput.FromJsonString(p)).ToArray();
            Scripts = ((JArray)jobj["scripts"]).Select(p => Witness.FromJsonString(p)).ToArray();
        }

        public Fixed8 GetFee()
        {
            if (this is InvocationTransaction itx)
            {
                return itx.Gas;
            }
            return this.NetworkFee;
        }

        bool IInventory.Verify()
        {
            return Verify(Enumerable.Empty<Transaction>());
        }

        public virtual bool Verify(IEnumerable<Transaction> mempool)
        {
            if (Type == TransactionType.ContractTransaction)
            {
                UInt160 toAddress = Outputs[0].ScriptHash;
                bool isSelf = true;
                for (int i = 1; i < Outputs.Length; i++)
                {
                    if (toAddress != Outputs[i].ScriptHash)
                    {
                        isSelf = false;
                        break;
                    }
                }

                if (isSelf)
                {
                    if (toAddress == References[Inputs[0]].ScriptHash)
                    {
                        return false;
                    }
                }
            }

            for (int i = 1; i < Inputs.Length; i++)
                for (int j = 0; j < i; j++)
                    if (Inputs[i].PrevHash == Inputs[j].PrevHash && Inputs[i].PrevIndex == Inputs[j].PrevIndex)
                        return false;
            if (mempool.Where(p => p != this).SelectMany(p => p.Inputs).Intersect(Inputs).Count() > 0) { 
                return false;
            }

            if (is_consensus_mempool == false)
            {
                if (Blockchain.Default.IsDoubleSpend(this))
                { Console.WriteLine("double spent error"); return false; }

                Fixed8 assetFee = Fixed8.Zero;

	            foreach (var group in Outputs.GroupBy(p => p.AssetId))
	            {
	                AssetState asset = Blockchain.Default.GetAssetState(group.Key);
	                if (asset == null) return false;
	                if (asset.Expiration <= Blockchain.Default.Height + 1 && asset.AssetType != AssetType.GoverningToken && asset.AssetType != AssetType.UtilityToken)
	                    return false;
	                foreach (TransactionOutput output in group)
	                    if (output.Value.GetData() % (long)Math.Pow(10, 8 - asset.Precision) != 0)
	                        return false;

	                if (Type == TransactionType.ContractTransaction)
	                {
	                    foreach (TransactionOutput output in group)
	                    {
	                        bool isOwn = false;
	                        foreach (var refOutput in References.Values)
	                        {
	                            if (output.ScriptHash == refOutput.ScriptHash)
	                            {
	                                isOwn = true;
	                                break;
	                            }
	                        }

	                        if (isOwn == false)
	                        {
	                            assetFee += asset.Fee;
	                        }
	                    }
	                }
	            }

	            TransactionResult[] results = GetTransactionResults()?.ToArray();
	            if (results == null) return false;
	            TransactionResult[] results_destroy = results.Where(p => p.Amount > Fixed8.Zero).ToArray();
	            if (results_destroy.Length > 1) return false;
	            if (results_destroy.Length == 1 && results_destroy[0].AssetId != Blockchain.UtilityToken.Hash)
	                return false;
	            if (SystemFee + assetFee > Fixed8.Zero && (results_destroy.Length == 0 || results_destroy[0].Amount < SystemFee))
	                return false;
            
	            TransactionResult[] results_issue = results.Where(p => p.Amount < Fixed8.Zero).ToArray();
	            switch (Type)
	            {
	                case TransactionType.MinerTransaction:
	                    if (results_issue.Any(p => p.AssetId != Blockchain.UtilityToken.Hash && p.AssetId != Blockchain.GoverningToken.Hash))
	                        return false;
	                    break;
	                case TransactionType.ClaimTransaction:
	                    if (results_issue.Any(p => p.AssetId != Blockchain.UtilityToken.Hash))
	                        return false;
	                    break;
	                case TransactionType.IssueTransaction:
	                    if (results_issue.Any(p => p.AssetId == Blockchain.UtilityToken.Hash))
	                        return false;
	                    break;
	                case TransactionType.InvocationTransaction:
	                    if (Outputs.Length <= 0)
	                        return false;
	                    break;
	                default:
	                    if (results_issue.Length > 0)
	                        return false;
	                    break;
	            }
            
	            if (Attributes.Count(p => p.Usage == TransactionAttributeUsage.ECDH02 || p.Usage == TransactionAttributeUsage.ECDH03) > 1)
	                return false;
                Console.WriteLine("Verifying Scripts");
	            return this.VerifyScripts();
            }
            return true;
        }
    }
}
