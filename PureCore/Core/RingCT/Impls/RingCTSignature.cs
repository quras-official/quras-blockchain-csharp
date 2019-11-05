using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

using Pure.Core.RingCT.Types;
using Pure.Cryptography.ECC;

namespace Pure.Core.RingCT.Impls
{
    public static class RingCTSignature
    {
        /// <summary>
        /// C_in = xG + aH : Format
        /// x => mask, a => amount
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static ECPoint GetCommitment(byte[] mask, byte[] amount)
        {
            ECPoint ret = ECCurve.Secp256r1.G * mask + RangeSignature.H * amount;
            return ret;
        }

        /// <summary>
        /// Tag-Linkable Ring-CT with Multiple Inputs and One-time Keys
        /// c.f. http://eprint.iacr.org/2015/1098 section 4. definition 10. 
        /// This does the MG sig on the "dest" part of the given key matrix, and 
        /// the last row is the sum of input commitments from that column - sum output commitments
        /// this shows that sum inputs = sum outputs
        /// </summary>
        /// <param name="pubs">Public keys using from Ring CT -> {P(i,j), C(i,j)}</param>
        /// <param name="inSK">Private keys for public keys</param>
        /// <param name="outSK">C_out sk</param>
        /// <param name="outPK">C_out PK</param>
        /// <param name="index">Signer's index</param>
        /// <returns></returns>
        public static MLSAGSignatureType ProveRctMG(List<List<CTKey>> pubs, List<CTCommitment> inSK, List<CTCommitment> outSK, List<CTKey> outPK, Fixed8 vPub, int index)
        {
            int rows = pubs[0].Count;
            int cols = pubs.Count;

            List<byte[]> sk = new List<byte[]>();
            List<byte[]> tmp = new List<byte[]>();

            for (int i = 0; i < rows + 1; i++)
            {
                byte[] sk_i = new byte[32];
                sk.Add(sk_i);
            }

            List<List<ECPoint>> M = new List<List<ECPoint>>();

            for (int i = 0; i < cols; i++)
            {
                List<ECPoint> M_i = new List<ECPoint>();
                ECPoint M_row = new ECPoint();
                for (int j = 0; j < rows; j++)
                {
                    M_i.Add(pubs[i][j].dest);
                    M_row = M_row + pubs[i][j].mask;
                }
                M_i.Add(M_row);
                M.Add(M_i);
            }

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < outPK.Count; j++)
                {
                    M[i][rows] = M[i][rows] - outPK[j].mask;
                }

                if (vPub > Fixed8.Zero)
                {
                    byte[] b_vPub = vPub.ToBinaryFormat().ToBinary();
                    M[i][rows] = M[i][rows] - RangeSignature.H * b_vPub;
                }
                else if (vPub < Fixed8.Zero)
                {
                    byte[] b_vPub = (-vPub).ToBinaryFormat().ToBinary();
                    M[i][rows] = M[i][rows] + RangeSignature.H * b_vPub;
                }
            }

            byte[] sk_row = inSK[0].mask;
            for (int i = 0; i < rows; i++)
            {
                sk[i] = inSK[i].dest;
                if (i > 0)
                {
                    sk_row = ScalarFunctions.Add(sk_row, inSK[i].mask);
                }
            }
            sk[rows] = sk_row;

            for (int i = 0; i < outPK.Count; i++)
            {
                sk[rows] = ScalarFunctions.Sub(sk[rows], outSK[i].mask);
            }

            return MLSAGSignature.Generate(M, sk, index);
        }

        public static bool VerRctMG(MLSAGSignatureType sig, List<List<CTKey>> pubs, List<CTKey> outPK, Fixed8 vPub)
        {
            int rows = pubs[0].Count;
            int cols = pubs.Count;
            List<List<ECPoint>> M = new List<List<ECPoint>>();

            for (int i = 0; i < cols; i++)
            {
                List<ECPoint> M_i = new List<ECPoint>();
                ECPoint M_row = new ECPoint();
                for (int j = 0; j < rows; j++)
                {
                    M_i.Add(pubs[i][j].dest);
                    M_row = M_row + pubs[i][j].mask;
                }
                M_i.Add(M_row);
                M.Add(M_i);
            }

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < outPK.Count; j++)
                {
                    M[i][rows] = M[i][rows] - outPK[j].mask;
                }
                
                if (vPub > Fixed8.Zero)
                {
                    byte[] b_vPub = vPub.ToBinaryFormat().ToBinary();
                    M[i][rows] = M[i][rows] - RangeSignature.H * b_vPub;
                }
                else if (vPub < Fixed8.Zero)
                {
                    byte[] b_vPub = (-vPub).ToBinaryFormat().ToBinary();
                    M[i][rows] = M[i][rows] + RangeSignature.H * b_vPub;
                }
            }

            return MLSAGSignature.Verify(M, sig);
        }

        public static List<CTKey> GetKeyFromBlockchain(uint start, int count, UInt256 assetID, out List<MixRingCTKey> mixRingIndex)
        {
            List<CTKey> ret = new List<CTKey>();
            mixRingIndex = new List<MixRingCTKey>();

            uint endNumber = 0;
            if ((int)Blockchain.Default.Height - 1 < 0)
            {
                endNumber = 0;
            }
            else
            {
                endNumber = Blockchain.Default.Height - 1;
            }

            while (ret.Count < count)
            {
                if (Blockchain.Default != null)
                {
                    for (uint i = endNumber; i >= start; i--)
                    {
                        try
                        {
                            Block block = Blockchain.Default.GetBlock(i);

                            foreach (Transaction tx in block.Transactions)
                            {
                                if (tx is RingConfidentialTransaction rtx)
                                {
                                    for (byte j = 0; j < rtx.RingCTSig.Count; j++)
                                    {
                                        if (rtx.RingCTSig[j].AssetID == assetID)
                                        {
                                            for (byte k = 0; k < rtx.RingCTSig[j].outPK.Count; k++)
                                            {
                                                CTKey key = rtx.RingCTSig[j].outPK[k];
                                                MixRingCTKey keyIndex = new MixRingCTKey(rtx.Hash, j, k);

                                                ret.Add(key);
                                                mixRingIndex.Add(keyIndex);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if ((int)Blockchain.Default.Height - 1 < 0)
                            {
                                endNumber = 0;
                            }
                            else
                            {
                                endNumber = Blockchain.Default.Height - 1;
                            }

                            break;
                        }

                        if (ret.Count >= count)
                        {
                            if ((int)Blockchain.Default.Height - 1 < 0)
                            {
                                endNumber = 0;
                            }
                            else
                            {
                                endNumber = Blockchain.Default.Height - 1;
                            }

                            break;
                        }
                    }
                }
                else
                {
                    CTKey key = new CTKey(new ECPoint(), new ECPoint());
                    MixRingCTKey keyIndex = new MixRingCTKey(new UInt256(new byte[32]), 0xff, 0xff);

                    ret.Add(key);
                    mixRingIndex.Add(keyIndex);
                }
            }

            return ret;
        }

        public static List<CTKey> GetRingKeyFromIndex(List<MixRingCTKey> keyIndexs, Fixed8 vPubOld)
        {
            List<CTKey> ret = new List<CTKey>();
            for (int i = 0; i < keyIndexs.Count; i++)
            {
                if ((keyIndexs[i].txHash == new UInt256(new byte[32]) && keyIndexs[i].RingCTIndex == 0xff && keyIndexs[i].RingCTOutPKIndex == 0xff) ||
                    (keyIndexs[i].txHash == Blockchain.Default.GetBlock(0).Hash && keyIndexs[i].RingCTIndex == 0xff && keyIndexs[i].RingCTOutPKIndex == 0xff))
                {
                    byte[] mask = new byte[32];

                    Fixed8 amount = vPubOld;
                    byte[] b_amount = amount.ToBinaryFormat().ToBinary();

                    ECPoint C_i_in = GetCommitment(mask, b_amount);

                    CTKey key = new CTKey(new ECPoint(), C_i_in);
                    ret.Add(key);
                }
                else
                {
                    Transaction tx = Blockchain.Default.GetTransaction(keyIndexs[i].txHash);

                    if (tx is RingConfidentialTransaction rtx)
                    {
                        CTKey key = rtx.RingCTSig[keyIndexs[i].RingCTIndex].outPK[keyIndexs[i].RingCTOutPKIndex];
                        ret.Add(key);
                    }
                    else
                    {
                        throw new Exception("InPK Index is not correct!");
                    }
                }
            }
            
            return ret;
        }

        public static List<List<CTKey>> GetRingKeyFromIndex(List<List<MixRingCTKey>> keyIndexs, Fixed8 vPubOld)
        {
            List<List<CTKey>> ret = new List<List<CTKey>>();
            for (int i = 0; i < keyIndexs.Count; i++)
            {
                List<CTKey> ret_i = new List<CTKey>();
                for (int j = 0; j < keyIndexs[0].Count; j++)
                {
                    if (Blockchain.Default != null)
                    {
                        if (keyIndexs[i][j].txHash == new UInt256())
                        {
                            byte[] mask = new byte[32];

                            Fixed8 amount = vPubOld;
                            byte[] b_amount = amount.ToBinaryFormat().ToBinary();

                            ECPoint C_i_in = GetCommitment(mask, b_amount);

                            CTKey key = new CTKey(new ECPoint(), C_i_in);
                            ret_i.Add(key);
                        }
                        else
                        {
                            Transaction tx = Blockchain.Default.GetTransaction(keyIndexs[i][j].txHash);

                            if (tx is RingConfidentialTransaction rtx)
                            {
                                CTKey key = rtx.RingCTSig[keyIndexs[i][j].RingCTIndex].outPK[keyIndexs[i][j].RingCTOutPKIndex];
                                ret_i.Add(key);
                            }
                            else
                            {
                                throw new Exception("InPK Index is not correct!");
                            }
                        }
                    }
                    else
                    {
                        CTKey key = new CTKey(new ECPoint(), new ECPoint());
                        ret_i.Add(key);
                    }
                }

                ret.Add(ret_i);
            }

            return ret;
        }

        public static RingInfo PopulateFromBlockchain(List<MixRingCTKey> inPK, int mixin, UInt256 assetID, Fixed8 vPubOld, uint startBlockHeight = 0)
        {
            int rows = inPK.Count;
            List<List<CTKey>> mixRing = new List<List<CTKey>>();
            List<List<MixRingCTKey>> mixRingIndex = new List<List<MixRingCTKey>>();
            int index = mixin.ToRandomInt();

            List<CTKey> inPKContent = GetRingKeyFromIndex(inPK, vPubOld);
            List<MixRingCTKey> otherPKIndex;

            List<CTKey> otherPKContent = GetKeyFromBlockchain(startBlockHeight, (mixin - 1) * rows, assetID, out otherPKIndex);

            int other_i = 0;
            for (int i = 0; i < mixin; i++)
            {
                if (i != index)
                {
                    List<CTKey> ring_i = new List<CTKey>();
                    List<MixRingCTKey> ringIndex_i = new List<MixRingCTKey>();

                    for (int j = 0; j < rows; j++)
                    {
                        ring_i.Add(otherPKContent[other_i]);
                        ringIndex_i.Add(otherPKIndex[other_i]);

                        other_i++;
                    }
                    mixRing.Add(ring_i);
                    mixRingIndex.Add(ringIndex_i);
                }
                else
                {
                    mixRing.Add(inPKContent);
                    mixRingIndex.Add(inPK);
                }
            }

            return new RingInfo(mixRing, index, mixRingIndex);
        }

        public static RingCTSignatureType Generate(List<CTCommitment> inSK, List<MixRingCTKey> inPK, List<ECPoint> destinations, List<Fixed8> amounts, Fixed8 vPub, int mixin, UInt256 assetID, Fixed8 vPubOld)
        {
            RingCTSignatureType rctSig = new RingCTSignatureType();
            List<CTCommitment> outSK = new List<CTCommitment>();

            for (int i = 0; i < destinations.Count; i++)
            {
                rctSig.outPK.Add(new CTKey());
                rctSig.rangeSigs.Add(new RangeProveType());
                rctSig.ecdhInfo.Add(new EcdhTuple());
            }

            for (int i = 0; i < destinations.Count; i++)
            {
                rctSig.outPK[i].dest = destinations[i];

                rctSig.rangeSigs[i] = RangeSignature.Generate(amounts[i]);

                rctSig.outPK[i].mask = rctSig.rangeSigs[i].C;

                CTCommitment outSK_i = new CTCommitment(new byte[32], rctSig.rangeSigs[i].mask);
                outSK.Add(outSK_i);

                rctSig.ecdhInfo[i].mask = rctSig.rangeSigs[i].mask;
                rctSig.ecdhInfo[i].amount = amounts[i].ToBinaryFormat().ToBinary();

                bool isEncoded = true;

                if (rctSig.ecdhInfo.Count > 1)
                    isEncoded = false;

                for (int j = 0; j < inSK.Count; j++)
                {
                    byte[] dest_cmp = new byte[32];
                    if (inSK[j].dest.ToHexString() == dest_cmp.ToHexString() && inSK[j].mask.ToHexString() == dest_cmp.ToHexString())
                    {
                        isEncoded = false;
                    }
                }

                
                if (isEncoded)
                    rctSig.ecdhInfo[i] = rctSig.ecdhInfo[i].EcdhEncode(destinations[i]);
            }

            RingInfo ringInfo = PopulateFromBlockchain(inPK, mixin, assetID, vPubOld);
            rctSig.mixRing = ringInfo.mixRingIndex;

            rctSig.MG = ProveRctMG(ringInfo.mixRing, inSK, outSK, rctSig.outPK, vPub, ringInfo.index);
            rctSig.vPub = vPub;
            return rctSig;
        }

        public static bool Verify(RingCTSignatureType sig, Fixed8 vPubOld)
        {
            bool result = true;
            for (int i = 0; i < sig.outPK.Count; i++)
            {
                bool rangeVerify = RangeSignature.Verify(sig.outPK[i].mask, sig.rangeSigs[i].rangeSig);
                result = result && rangeVerify;
            }

            List<List<CTKey>> mixRing = GetRingKeyFromIndex(sig.mixRing, vPubOld);

            bool mgVer = VerRctMG(sig.MG, mixRing, sig.outPK, sig.vPub);

            return result && mgVer;
        }

        public static Fixed8 DecodeRct(RingCTSignatureType sig, byte[] sk, int i, out byte[] mask)
        {
            EcdhTuple unmask;
            if (sig.ecdhInfo[i].amount[0] == 0x00)
                unmask = sig.ecdhInfo[i];
            else
                unmask = sig.ecdhInfo[i].EcdhDecode(sk);

            ECPoint C = sig.outPK[i].mask;

            ECPoint C_Tmp = ECCurve.Secp256r1.G * unmask.mask + RangeSignature.H * unmask.amount;

            if (C.ToString() != C_Tmp.ToString())
            {
                throw new Exception("Amount decoded incorrectly");
            }

            mask = new byte[32];
            Buffer.BlockCopy(unmask.mask, 0, mask, 0, unmask.mask.Length);

            return unmask.amount.ToFixed8();
        }
    }
}
