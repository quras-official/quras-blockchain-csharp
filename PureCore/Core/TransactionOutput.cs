using Pure.IO;
using Pure.IO.Json;
using Pure.VM;
using Pure.Wallets;
using System;
using System.IO;

namespace Pure.Core
{
    public class TransactionOutput : IInteropInterface, ISerializable
    {
        public UInt256 AssetId;

        public Fixed8 Value;

        public UInt160 ScriptHash;

        public Fixed8 Fee = Fixed8.Zero;

        public int Size => AssetId.Size + Value.Size + ScriptHash.Size + Fee.Size;

        void ISerializable.Deserialize(BinaryReader reader)
        {
            this.AssetId = reader.ReadSerializable<UInt256>();
            this.Value = reader.ReadSerializable<Fixed8>();
            if (Value <= Fixed8.Zero) throw new FormatException();
            this.ScriptHash = reader.ReadSerializable<UInt160>();
            this.Fee = reader.ReadSerializable<Fixed8>();
            if (Fee < Fixed8.Zero) throw new FormatException();
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(Value);
            writer.Write(ScriptHash);
            writer.Write(Fee);
        }

        public JObject ToJson(ushort index)
        {
            JObject json = new JObject();
            json["n"] = index;
            json["asset"] = AssetId.ToString();
            json["value"] = Value.ToString();
            json["address"] = Wallet.ToAddress(ScriptHash);
            json["fee"] = Value.ToString();
            return json;
        }
    }
}
