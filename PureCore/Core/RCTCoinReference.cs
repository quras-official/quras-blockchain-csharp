using Pure.IO;
using Pure.IO.Json;
using Pure.VM;
using System;
using System.IO;

namespace Pure.Core
{
    public class RCTCoinReference : IEquatable<RCTCoinReference>, IInteropInterface, ISerializable
    {
        public UInt256 PrevHash;
        public Cryptography.ECC.ECPoint TxRCTHash;
        public ushort PrevRCTSigId;
        public ushort PrevRCTSigIndex;

        public int Size => PrevHash.Size + sizeof(ushort) + sizeof(ushort) + TxRCTHash.Size;

        public void Deserialize(BinaryReader reader)
        {
            PrevHash = reader.ReadSerializable<UInt256>();
            TxRCTHash = reader.ReadSerializable<Cryptography.ECC.ECPoint>();
            PrevRCTSigId = reader.ReadUInt16();
            PrevRCTSigIndex = reader.ReadUInt16();
        }

        public bool Equals(RCTCoinReference other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            if (!(other is RCTCoinReference)) return false;
            return other.PrevHash == this.PrevHash && other.TxRCTHash.ToString() == this.TxRCTHash.ToString() && other.PrevRCTSigId == this.PrevRCTSigId && other.PrevRCTSigIndex == this.PrevRCTSigIndex;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(PrevHash);
            writer.Write(TxRCTHash);
            writer.Write(PrevRCTSigId);
            writer.Write(PrevRCTSigIndex);
        }

        public override int GetHashCode()
        {
            return PrevHash.GetHashCode() + PrevRCTSigId.GetHashCode() + PrevRCTSigIndex.GetHashCode();
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["txid"] = PrevHash.ToString();
            json["rctid"] = PrevRCTSigId;
            json["rctindex"] = PrevRCTSigIndex;
            return json;
        }
    }
}
