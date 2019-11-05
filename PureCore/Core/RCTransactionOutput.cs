using Pure.IO;
using Pure.IO.Json;
using Pure.VM;
using Pure.Wallets;
using Pure.Cryptography.ECC;
using System;
using System.IO;

namespace Pure.Core
{
    public class RCTransactionOutput : IInteropInterface, ISerializable
    {
        public UInt256 AssetId;

        public Fixed8 Value;

        public ECPoint PubKey;

        public UInt160 ScriptHash;

        public int Size => AssetId.Size + Value.Size + PubKey.Size + ScriptHash.Size;

        void ISerializable.Deserialize(BinaryReader reader)
        {
            this.AssetId = reader.ReadSerializable<UInt256>();
            this.Value = reader.ReadSerializable<Fixed8>();
            if (Value <= Fixed8.Zero) throw new FormatException();
            this.PubKey = reader.ReadSerializable<ECPoint>();
            this.ScriptHash = reader.ReadSerializable<UInt160>();
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(Value);
            writer.Write(PubKey);
            writer.Write(ScriptHash);
        }

        public JObject ToJson(ushort index)
        {
            JObject json = new JObject();
            json["n"] = index;
            json["asset"] = AssetId.ToString();
            json["value"] = Value.ToString();
            json["scripthash"] = ScriptHash.ToString();
            return json;
        }
    }
}
