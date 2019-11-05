using Pure.IO;
using Pure.IO.Json;
using Pure.VM;
using System;
using System.IO;

namespace Pure.Core
{
    public class CoinReference : IEquatable<CoinReference>, IInteropInterface, ISerializable
    {
        public UInt256 PrevHash;
        
        public ushort PrevIndex;

        public int Size => PrevHash.Size + sizeof(ushort);

        void ISerializable.Deserialize(BinaryReader reader)
        {
            PrevHash = reader.ReadSerializable<UInt256>();
            PrevIndex = reader.ReadUInt16();
        }
        
        public bool Equals(CoinReference other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            return PrevHash.Equals(other.PrevHash) && PrevIndex.Equals(other.PrevIndex);
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(null, obj)) return false;
            if (!(obj is CoinReference)) return false;
            return Equals((CoinReference)obj);
        }

        public override int GetHashCode()
        {
            return PrevHash.GetHashCode() + PrevIndex.GetHashCode();
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.Write(PrevHash);
            writer.Write(PrevIndex);
        }

        public static CoinReference FromJson(JObject json)
        {
            CoinReference coinReference = new CoinReference();

            coinReference.PrevHash = new UInt256(json["txid"].AsString().Substring(2).HexToBytesInverse());
            coinReference.PrevIndex = (ushort)json["vout"].AsNumber();

            return coinReference;
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["txid"] = PrevHash.ToString();
            json["vout"] = PrevIndex;
            return json;
        }
    }
}
