using Pure.IO;
using Pure.IO.Json;
using Pure.VM;
using System;
using System.IO;

namespace Pure.Core
{
    public class JSCoinReference : IEquatable<JSCoinReference>, IInteropInterface, ISerializable
    {
        public UInt256 PrevHash;

        public ushort PrevJsId;
        public ushort PrevIndex;

        public int Size => PrevHash.Size + sizeof(ushort) + sizeof(ushort);

        void ISerializable.Deserialize(BinaryReader reader)
        {
            PrevHash = reader.ReadSerializable<UInt256>();
            PrevJsId = reader.ReadUInt16();
            PrevIndex = reader.ReadUInt16();
        }

        public bool Equals(JSCoinReference other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            return PrevHash.Equals(other.PrevHash) && PrevJsId.Equals(other.PrevJsId) && PrevIndex.Equals(other.PrevIndex);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(null, obj)) return false;
            if (!(obj is JSCoinReference)) return false;
            return Equals((JSCoinReference)obj);
        }

        public override int GetHashCode()
        {
            return PrevHash.GetHashCode() + PrevJsId.GetHashCode() + PrevIndex.GetHashCode();
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.Write(PrevHash);
            writer.Write(PrevJsId);
            writer.Write(PrevIndex);
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["txid"] = PrevHash.ToString();
            json["jsid"] = PrevJsId;
            json["vout"] = PrevIndex;
            return json;
        }
    }
}
