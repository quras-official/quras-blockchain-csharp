using Quras.IO;
using Quras.IO.Json;
using Quras.VM;
using Quras.Wallets;
using System;
using System.IO;

namespace Quras.Core
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
            json["fee"] = Fee.ToString();
            return json;
        }

        public JObject ToJsonString()
        {
            JObject json = new JObject();
            json["assetId"] = AssetId.ToString().Substring(2);
            json["value"] = Value.ToString();
            json["scriptHash"] = ScriptHash.ToString().Substring(2);
            json["fee"] = Fee.ToString();
            return json;
        }

        public static TransactionOutput FromJson(JObject json)
        {
            TransactionOutput output = new TransactionOutput();
            output.AssetId = UInt256.Parse(json["asset"].AsString());
            output.Value = Fixed8.Parse(json["value"].AsString());
            output.ScriptHash = Wallet.ToScriptHash(json["address"].AsString());
            output.Fee = Fixed8.Parse(json["fee"].AsString());
            return output;
        }

        public static TransactionOutput FromJsonString(JObject json)
        {
            TransactionOutput output = new TransactionOutput();
            output.AssetId = UInt256.Parse("0x" + json["assetId"].AsString());
            output.Value = Fixed8.Parse(json["value"].AsString());
            output.ScriptHash = UInt160.Parse("0x" + json["scriptHash"].AsString());
            output.Fee = Fixed8.Parse(json["fee"].AsString());
            return output;
        }
    }
}
