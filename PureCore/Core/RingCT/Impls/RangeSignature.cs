using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

using Pure.Core.RingCT.Types;
using Pure.Cryptography.ECC;

namespace Pure.Core.RingCT.Impls
{
    /// <summary>
    /// Range Prove.
    /// </summary>
    public static class RangeSignature
    {
        //public static ECPoint H = ECPoint.DecodePoint(("04" + Cryptography.Crypto.Default.Hash256(("6B17D1F2E12C4247F8BCE6E563A440F277037D812DEB33A0F4A13945D898C296").HexToBytes()).ToHexString() + Cryptography.Crypto.Default.Hash256("4FE342E2FE1A7F9B8EE7EB4A7C0F9E162BCE33576B315ECECBB6406837BF51F5".HexToBytes()).ToHexString()).HexToBytes(), ECCurve.Secp256r1);
        public static ECPoint H = ECCurve.Secp256r1.G * ("6B17D1F2E12C4247F8BCE6E563A440F277037D812DEB33A0F4A13945D898C296").HexToBytes();
        public static List<ECPoint> H2 = new List<ECPoint>();

        public static void Initialize()
        {
            if (H2.Count == 64) return;

            H2.Add(H * BigInteger.Pow(new BigInteger(2), 0));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 1));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 2));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 3));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 4));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 5));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 6));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 7));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 8));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 9));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 10));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 11));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 12));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 13));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 14));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 15));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 16));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 17));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 18));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 19));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 20));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 21));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 22));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 23));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 24));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 25));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 26));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 27));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 28));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 29));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 30));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 31));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 32));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 33));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 34));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 35));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 36));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 37));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 38));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 39));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 40));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 41));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 42));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 43));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 44));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 45));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 46));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 47));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 48));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 49));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 50));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 51));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 52));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 53));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 54));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 55));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 56));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 57));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 58));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 59));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 60));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 61));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 62));
            H2.Add(H * BigInteger.Pow(new BigInteger(2), 63));
        }
        
        /// <summary>
        /// Generate gives C, and mask such that \sumCi = C
        /// c.f. http://eprint.iacr.org/2015/1098 section 5.1
        /// and Ci is a commitment to either 0 or 2^i, i=0,...,63
        /// thus this proves that "amount" is in [0, 2^64]
        /// mask is a such that C = aG + bH, and b = amount
        /// </summary>
        /// <returns></returns>
        public static RangeProveType Generate(Fixed8 amount)
        {
            Initialize();

            byte[] mask = new byte[32];
            ECPoint C = new ECPoint();

            List<int> binaryAmount = amount.ToBinaryFormat();
            List<byte[]> ai = new List<byte[]>();
            List<ECPoint> CiH = new List<ECPoint>();

            RangeProveType rangeProver = new RangeProveType();

            for (int i = 0; i < ASNLRingSignature.AMOUNT_SIZE; i++)
            {
                byte[] ai_i = new byte[32];
                ai.Add(ai_i);

                if (binaryAmount[i] == 0)
                {
                    rangeProver.rangeSig.Ci.Add(ECCurve.Secp256r1.G * ai_i);
                }
                else if (binaryAmount[i] == 1)
                {
                    rangeProver.rangeSig.Ci.Add(ECCurve.Secp256r1.G * ai_i + H2[i]);
                }
                else
                {
                    throw new Exception("Range Prove => Binary Format Error!");
                }

                CiH.Add(rangeProver.rangeSig.Ci[i] - H2[i]);
                mask = ScalarFunctions.Add(mask, ai[i]);

                if (i == 0)
                {
                    C = rangeProver.rangeSig.Ci[i];
                }
                else
                {
                    C = C + rangeProver.rangeSig.Ci[i];
                }
            }

            rangeProver.C = C;
            rangeProver.mask = mask;
            rangeProver.rangeSig.AsnlSig = ASNLRingSignature.Generate(ai, rangeProver.rangeSig.Ci, CiH, binaryAmount);

            if (!ASNLRingSignature.Verify(rangeProver.rangeSig.Ci, CiH, rangeProver.rangeSig.AsnlSig))
                throw new Exception("Range prove error => ASNL verify error!");

            return rangeProver.Export();
        }

        public static bool Verify(ECPoint C, RangeSigatureType rangeSig)
        {
            Initialize();

            List<ECPoint> CiH = new List<ECPoint>();
            ECPoint Ctmp = rangeSig.Ci[0];

            bool reb = false;
            bool rab = false;

            Console.WriteLine(rangeSig.Ci.Count);
            Console.WriteLine(H2.Count);

            for (int i = 0; i < ASNLRingSignature.AMOUNT_SIZE; i++)
            {
                CiH.Add(rangeSig.Ci[i] - H2[i]);
                if (i > 0)
                {
                    Ctmp = Ctmp + rangeSig.Ci[i];
                }
            }

            reb = C.ToString() == Ctmp.ToString();

            rab = ASNLRingSignature.Verify(rangeSig.Ci, CiH, rangeSig.AsnlSig);

            return reb && rab;
        }
    }
}
