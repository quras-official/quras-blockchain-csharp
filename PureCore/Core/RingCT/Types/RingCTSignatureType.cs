using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Pure.IO;
using Pure.IO.Json;
using Pure.VM;
using Pure.Cryptography.ECC;
using System.IO;

namespace Pure.Core.RingCT.Types
{
    public class CTKey : IInteropInterface, ISerializable
    {
        public ECPoint dest;    // Public Key
        public ECPoint mask;    // Commitments

        public CTKey()
        {
            dest = new ECPoint();
            mask = new ECPoint();
        }

        public CTKey(ECPoint dest, ECPoint mask)
        {
            this.dest = dest;
            this.mask = mask;
        }

        public int Size => 33 + 33;

        public void Deserialize(BinaryReader reader)
        {
            dest = reader.ReadSerializable<ECPoint>();
            mask = reader.ReadSerializable<ECPoint>();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(dest);
            writer.Write(mask);
        }
    }

    public class MixRingCTKey : IInteropInterface, ISerializable
    {
        public UInt256 txHash;  // Transaction Hash
        public byte RingCTIndex;
        public byte RingCTOutPKIndex;

        public MixRingCTKey()
        {

        }

        public MixRingCTKey(UInt256 hash, byte index, byte outPkIndex)
        {
            this.txHash = hash;
            this.RingCTIndex = index;
            this.RingCTOutPKIndex = outPkIndex;
        }

        public int Size => txHash.Size + sizeof(byte) * 3;

        public void Deserialize(BinaryReader reader)
        {
            txHash = reader.ReadSerializable<UInt256>();
            RingCTIndex = reader.ReadByte();
            RingCTOutPKIndex = reader.ReadByte();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(txHash);
            writer.Write(RingCTIndex);
            writer.Write(RingCTOutPKIndex);
        }
    }
    

    public class CTCommitment
    {
        public byte[] dest;    // Private Key
        public byte[] mask;    // Mask

        public CTCommitment()
        {

        }

        public CTCommitment(byte[] dest, byte[] mask)
        {
            this.dest = new byte[32];
            this.mask = new byte[32];

            Buffer.BlockCopy(dest, 0, this.dest, 0, dest.Length);
            Buffer.BlockCopy(mask, 0, this.mask, 0, mask.Length);
        }
    }

    public class RingInfo
    {
        public List<List<CTKey>> mixRing;
        public int index;
        public List<List<MixRingCTKey>> mixRingIndex;

        public RingInfo()
        {

        }

        public RingInfo(List<List<CTKey>> mixRing, int index, List<List<MixRingCTKey>> mixRingIndex = null)
        {
            this.mixRing = mixRing;
            this.index = index;
            this.mixRingIndex = mixRingIndex;
        }
    }

    public class EcdhTuple : IInteropInterface, ISerializable
    {
        public byte[] mask;
        public byte[] amount;
        public ECPoint senderPK;

        public EcdhTuple()
        {
            mask = new byte[32];
            amount = new byte[32];
            senderPK = new ECPoint();
        }

        public EcdhTuple(byte[] mask, byte[] amount, ECPoint senderPK)
        {
            this.mask = new byte[mask.Length];
            this.amount = new byte[amount.Length];
            this.senderPK = senderPK;

            Buffer.BlockCopy(amount, 0, this.amount, 0, amount.Length);
            Buffer.BlockCopy(mask, 0, this.mask, 0, mask.Length);
        }

        public int Size => 32 + 32 + 33;

        public void Deserialize(BinaryReader reader)
        {
            mask = reader.ReadVarBytes();
            amount = reader.ReadVarBytes();
            senderPK = reader.ReadSerializable<ECPoint>();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteVarBytes(mask);
            writer.WriteVarBytes(amount);
            writer.Write(senderPK);
        }
    }

    public class RingCTSignatureType : IInteropInterface, ISerializable
    {
        public UInt256 AssetID;
        public List<RangeProveType> rangeSigs;   // Range Signature
        public MLSAGSignatureType MG;   // MLSAG Signature
        public List<List<MixRingCTKey>> mixRing; // the set of all pubkeys / copy pairs that you mix with

        public List<EcdhTuple> ecdhInfo;
        public List<CTKey> outPK;
        public Fixed8 vPub;

        public RingCTSignatureType()
        {
            AssetID = new UInt256();
            rangeSigs = new List<RangeProveType>();
            MG = new MLSAGSignatureType();
            mixRing = new List<List<MixRingCTKey>>();
            ecdhInfo = new List<EcdhTuple>();
            outPK = new List<CTKey>();
            vPub = Fixed8.Zero;
        }

        public int Size => 32 + rangeSigs.Count * rangeSigs[0].Size + MG.Size + mixRing.Count * mixRing[0].Count * mixRing[0][0].Size + ecdhInfo.Count * ecdhInfo[0].Size + outPK.Count * outPK[0].Size + vPub.Size;

        public void Deserialize(BinaryReader reader)
        {
            AssetID = reader.ReadSerializable<UInt256>();
            rangeSigs = reader.ReadSerializableArray<RangeProveType>().ToList();
            MG = reader.ReadSerializable<MLSAGSignatureType>();

            int row = reader.ReadInt32();
            int col = reader.ReadInt32();

            for (int i = 0; i < row; i++)
            {
                List<MixRingCTKey> mixRing_i = new List<MixRingCTKey>();
                for (int j = 0; j < col; j++)
                {
                    mixRing_i.Add(reader.ReadSerializable<MixRingCTKey>());
                }

                mixRing.Add(mixRing_i);
            }

            ecdhInfo = reader.ReadSerializableArray<EcdhTuple>().ToList();
            outPK = reader.ReadSerializableArray<CTKey>().ToList();
            vPub = reader.ReadSerializable<Fixed8>();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AssetID);
            writer.Write(rangeSigs.ToArray());
            writer.Write(MG);

            writer.Write(mixRing.Count);
            writer.Write(mixRing[0].Count);

            for (int i = 0; i < mixRing.Count; i++)
            {
                for (int j = 0; j < mixRing[0].Count; j++)
                {
                    writer.Write(mixRing[i][j]);
                }
            }

            writer.Write(ecdhInfo.ToArray());
            writer.Write(outPK.ToArray());
            writer.Write(vPub);
        }

        public JObject ToJson(ushort index)
        {
            JObject json = new JObject();
            json["n"] = index;
            //json["asset"] = AssetId.ToString();
            //json["value"] = Value.ToString();
            //json["address"] = Wallet.ToAddress(ScriptHash);
            return json;
        }
    }
}
