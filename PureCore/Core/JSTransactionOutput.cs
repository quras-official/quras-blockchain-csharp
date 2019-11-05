using Pure.IO;
using Pure.IO.Json;
using Pure.VM;
using Pure.Wallets;
using PureCore.Wallets.AnonymousKey.Key;
using System;
using System.IO;

namespace Pure.Core
{
    public class JSTransactionOutput : IInteropInterface, ISerializable
    {
        public UInt256 AssetId;

        public Fixed8 Value;

        public UInt160 ScriptHash;

        public PaymentAddress addr;

        public UInt256 rho;

        public UInt256 r;

        public byte[] witness;

        public long witness_height;
        public long cmtree_height;

        public int Size => AssetId.Size + Value.Size + ScriptHash.Size + addr.Size + rho.Size + r.Size + witness.GetVarSize() + sizeof(long) + sizeof(long);

        void ISerializable.Deserialize(BinaryReader reader)
        {
            this.AssetId = reader.ReadSerializable<UInt256>();
            this.Value = reader.ReadSerializable<Fixed8>();
            if (Value <= Fixed8.Zero) throw new FormatException();
            this.ScriptHash = reader.ReadSerializable<UInt160>();
            this.addr = reader.ReadSerializable<PaymentAddress>();
            this.rho = reader.ReadSerializable<UInt256>();
            this.r = reader.ReadSerializable<UInt256>();
            this.witness = reader.ReadVarBytes();
            this.witness_height = reader.ReadInt64();
            this.cmtree_height = reader.ReadInt64();
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(Value);
            writer.Write(ScriptHash);
            writer.Write(addr);
            writer.Write(rho);
            writer.Write(r);
            writer.WriteVarBytes(witness);
            writer.Write(witness_height);
            writer.Write(cmtree_height);
        }

        public JObject ToJson(ushort index)
        {
            JObject json = new JObject();
            json["n"] = index;
            json["asset"] = AssetId.ToString();
            json["value"] = Value.ToString();
            json["address"] = Wallet.ToAnonymousAddress(addr);
            json["rho"] = rho.ToString();
            json["r"] = r.ToString();
            json["witness"] = "Witness";
            json["witness_height"] = witness_height.ToString();
            json["cmtree_height"] = cmtree_height.ToString();
            return json;
        }
    }
}
