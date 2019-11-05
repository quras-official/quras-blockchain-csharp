using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Pure.IO;
using Pure.Core.RingCT.Types;
using Pure.Core.RingCT.Impls;
using Pure.Cryptography.ECC;

namespace Pure.Core
{
    public class RingConfidentialTransaction : Transaction
    {
        public List<RingCTSignatureType> RingCTSig;
        public ECPoint RHashKey;

        public override int Size => base.Size + RingCTSig.Count * RingCTSig[0].Size + RHashKey.Size;

        public override Fixed8 SystemFee
        {
            get
            {
                bool isQrgAsset = false;
                for (int i = 0; i < Outputs.Length; i++)
                {
                    if (Outputs[i].AssetId != Blockchain.GoverningToken.Hash)
                    {
                        isQrgAsset = true;
                    }
                }

                for (int i = 0; i < RingCTSig.Count; i++)
                {
                    if (RingCTSig[i].AssetID != Blockchain.GoverningToken.Hash)
                    {
                        isQrgAsset = true;
                    }
                }
                
                if (isQrgAsset == false)
                {
                    return Fixed8.Zero;
                }

                switch (GetTxType())
                {
                    case RingConfidentialTransactionType.S_S_Transaction:
                    case RingConfidentialTransactionType.S_T_Transaction:
                    case RingConfidentialTransactionType.S_ST_Transaction:
                        return Fixed8.Satoshi * 10000000;
                    case RingConfidentialTransactionType.T_S_Transaction:
                    case RingConfidentialTransactionType.T_ST_Transaction:
                        return Fixed8.Zero;
                    default:
                        return Fixed8.Zero;
                }
            }
        }

        public override Fixed8 QrsSystemFee
        {
            get
            {
                bool isQrsAsset = false;
                for (int i = 0; i < Outputs.Length; i++)
                {
                    if (Outputs[i].AssetId == Blockchain.GoverningToken.Hash)
                    {
                        isQrsAsset = true;
                    }
                }

                for (int i = 0; i < RingCTSig.Count; i++)
                {
                    if (RingCTSig[i].AssetID == Blockchain.GoverningToken.Hash)
                    {
                        isQrsAsset = true;
                    }
                }

                if (isQrsAsset == false)
                {
                    return Fixed8.Zero;
                }

                switch (GetTxType())
                {
                    case RingConfidentialTransactionType.S_S_Transaction:
                    case RingConfidentialTransactionType.S_T_Transaction:
                    case RingConfidentialTransactionType.S_ST_Transaction:
                        return Fixed8.Satoshi * 10000000;
                    case RingConfidentialTransactionType.T_S_Transaction:
                    case RingConfidentialTransactionType.T_ST_Transaction:
                        return Fixed8.Zero;
                    default:
                        return Fixed8.Zero;
                }
            }
        }

        public RingConfidentialTransactionType GetTxType()
        {
            if (Inputs.Length == 0 && Outputs.Length == 0)
            {
                return RingConfidentialTransactionType.S_S_Transaction;
            }
            else if (Inputs.Length == 0)
            {
                return RingConfidentialTransactionType.S_T_Transaction;
            }
            else
            {
                return RingConfidentialTransactionType.T_S_Transaction;
            }
        }

        public RingConfidentialTransaction()
            : base(TransactionType.RingConfidentialTransaction)
        {
            this.RingCTSig = new List<RingCTSignatureType>();
        }

        public RingConfidentialTransaction(RingCTSignatureType sig)
            : base(TransactionType.RingConfidentialTransaction)
        {
            this.RingCTSig = new List<RingCTSignatureType>();
            this.RingCTSig.Add(sig);
        }

        protected override void SerializeExclusiveData(BinaryWriter writer)
        {
            writer.Write(RingCTSig.ToArray());
            writer.Write(RHashKey);
        }

        protected override void DeserializeExclusiveData(BinaryReader reader)
        {
            RingCTSig = reader.ReadSerializableArray<RingCTSignatureType>().ToList();
            RHashKey = reader.ReadSerializable<ECPoint>();
        }

        public override bool Verify(IEnumerable<Transaction> mempool)
        {
            switch (GetTxType())
            {
                case RingConfidentialTransactionType.S_S_Transaction:
                    for (int i = 0; i < RingCTSig.Count; i++)
                    {
                        if (!RingCT.Impls.RingCTSignature.Verify(RingCTSig[i], Fixed8.Zero))
                        {
                            return false;
                        }

                        // Double Spending Check
                        if (Blockchain.Default.IsDoubleRingCTCommitment(this))
                        {
                            return false;
                        }

                        // Check Mix Rings Link & Asset
                        UInt256 assetID = RingCTSig[i].AssetID;
                        for (int j = 0; j < RingCTSig[i].mixRing.Count; j++)
                        {
                            for (int k = 0; k < RingCTSig[i].mixRing[j].Count; k++)
                            {
                                Transaction tx = Blockchain.Default.GetTransaction(RingCTSig[i].mixRing[j][k].txHash);

                                if (tx is RingConfidentialTransaction rtx)
                                {
                                    if (rtx.RingCTSig[RingCTSig[i].mixRing[j][k].RingCTIndex].AssetID != assetID)
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    
                    break;
                case RingConfidentialTransactionType.S_T_Transaction:
                    // Check Amount
                    try
                    {
                        Dictionary<UInt256, Fixed8> amount = new Dictionary<UInt256, Fixed8>();
                        for (int i = 0; i < Outputs.Length; i++)
                        {
                            if (amount.ContainsKey(Outputs[i].AssetId))
                            {
                                amount[Outputs[i].AssetId] += Outputs[i].Value;
                            }
                            else
                            {
                                amount[Outputs[i].AssetId] = Outputs[i].Value;
                            }
                        }

                        for (int i = 0; i < RingCTSig.Count; i++)
                        {
                            if (!RingCT.Impls.RingCTSignature.Verify(RingCTSig[i], Fixed8.Zero))
                            {
                                return false;
                            }

                            // Double Spending Check
                            if (Blockchain.Default.IsDoubleRingCTCommitment(this))
                            {
                                return false;
                            }

                            if (amount.ContainsKey(RingCTSig[i].AssetID))
                            {
                                amount[RingCTSig[i].AssetID] -= RingCTSig[i].vPub;
                            }
                            else if(i == RingCTSig.Count - 1 && RingCTSig[i].AssetID == Blockchain.UtilityToken.Hash && RingCTSig[i].vPub != Fixed8.Zero)
                            {
                                amount[RingCTSig[i].AssetID] = Fixed8.Zero - RingCTSig[i].vPub;
                            }
                            else
                            {
                                return false;
                            }
                        }

                        if (SystemFee > Fixed8.Zero)
                        {
                            amount[Blockchain.UtilityToken.Hash] += Blockchain.UtilityToken.A_Fee;
                        }

                        foreach (var key in amount.Keys)
                        {
                            if (amount[key] > Fixed8.Zero)
                            {
                                return false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                    break;
                case RingConfidentialTransactionType.T_S_Transaction:
                    for (int i = 0; i < RingCTSig.Count; i++)
                    {
                        Fixed8 vPubOld = Fixed8.Zero;

                        for (int j = 0; j < Inputs.Length; j++)
                        {
                            Transaction tx = Blockchain.Default.GetTransaction(Inputs[j].PrevHash);
                            if (tx.Outputs[Inputs[j].PrevIndex].AssetId.ToString() == RingCTSig[i].AssetID.ToString())
                            {
                                vPubOld += tx.Outputs[Inputs[j].PrevIndex].Value;
                            }
                        }

                        if (!RingCT.Impls.RingCTSignature.Verify(RingCTSig[i], vPubOld))
                        {
                            return false;
                        }

                        // Check the amount
                        Fixed8 vPubOut = Fixed8.Zero;
                        for (int j = 0; j < Outputs.Length; j++)
                        {
                            if (Outputs[j].AssetId == RingCTSig[i].AssetID)
                            {
                                vPubOut += Outputs[j].Value;
                            }
                        }

                        Fixed8 vPrivOut = Fixed8.Zero;

                        for (int j = 0; j < RingCTSig[i].ecdhInfo.Count; j++)
                        {
                            vPrivOut += RingCTSig[i].ecdhInfo[j].amount.ToFixed8();
                        }

                        if (RingCTSig[i].AssetID == Blockchain.GoverningToken.Hash)
                        {
                            if (vPubOld < vPrivOut + vPubOut + QrsSystemFee)
                            {
                                return false;
                            }
                        }
                        else if (RingCTSig[i].AssetID == Blockchain.UtilityToken.Hash)
                        {
                            if (vPubOld < vPrivOut + vPubOut + SystemFee)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (vPubOld < vPrivOut + vPubOut)
                            {
                                return false;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            return true;
        }
    }

    public enum RingConfidentialTransactionType : byte
    {
        NONE = 0x00,
        T_S_Transaction = 0x01,
        S_S_Transaction = 0x02,
        S_T_Transaction = 0x03,
        S_ST_Transaction = 0x04,
        T_ST_Transaction = 0x05
    }
}
