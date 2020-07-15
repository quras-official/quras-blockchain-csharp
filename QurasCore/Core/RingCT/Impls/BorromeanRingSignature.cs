using System;
using System.Collections.Generic;
using System.Text;

using Quras.Core.RingCT.Types;
using Quras.Cryptography;
using Quras.Cryptography.ECC;

namespace Quras.Core.RingCT.Impls
{
    public static class BorromeanRingSignature
    {
        public static int AMOUNT_SIZE = 64;
        public static BorromeanSignatureType Generate(List<byte[]> x, List<Cryptography.ECC.ECPoint> P1, List<Cryptography.ECC.ECPoint> P2, List<int> indices)
        {
            List<byte[]> s1 = new List<byte[]>();
            List<byte[]> alpha = new List<byte[]>();
            List<ECPoint>[] L = new List<ECPoint> [2];
            L[0] = new List<ECPoint>();
            L[1] = new List<ECPoint>();

            BorromeanSignatureType boroSig = new BorromeanSignatureType();
            boroSig.InitSField();

            for (int i = 0; i < AMOUNT_SIZE; i++)
            {
                int naught = indices[i];
                int prime = (naught + 1) % 2;

                byte[] a = SchnorrNonLinkable.GenerateRandomScalar();
                ECPoint L1 = Cryptography.ECC.ECCurve.Secp256r1.G * a;

                L[naught].Add(L1);
                alpha.Add(a);
                if (naught == 0)
                {
                    byte[] s2 = SchnorrNonLinkable.GenerateRandomScalar();
                    byte[] c2 = Crypto.Default.Hash256(L1.EncodePoint(true));
                    ECPoint L2 = Cryptography.ECC.ECCurve.Secp256r1.G * s2 + P2[i] * c2;
                    L[prime].Add(L2);
                    boroSig.s1.Add(s2);
                }
                else
                    boroSig.s1.Add(new byte[32]);

                boroSig.ee = ScalarFunctions.Add(boroSig.ee, Crypto.Default.Hash256(L[1][i].EncodePoint(true))); //Check This Part
            }

            for (int i = 0; i < AMOUNT_SIZE; i ++)
            {
                if (indices[i] == 0)
                {
                    boroSig.s0.Add(ScalarFunctions.MulSub(boroSig.ee, x[i], alpha[i]));
                }
                else
                {
                    byte[] s2 = SchnorrNonLinkable.GenerateRandomScalar();
                    ECPoint LL = Cryptography.ECC.ECCurve.Secp256r1.G * s2 + P1[i] * boroSig.ee;
                    byte[] cc = Crypto.Default.Hash256(LL.EncodePoint(true));
                    boroSig.s1[i] = ScalarFunctions.MulSub(cc, x[i], alpha[i]);
                    boroSig.s0.Add(s2);
                }
            }

            return boroSig.Exports();
        }

        public static bool Verify(List<ECPoint> P1, List<ECPoint> P2, BorromeanSignatureType sig)
        {
            byte[] ee = new byte [32];
            ECPoint LHS;
            ECPoint RHS = ECCurve.Secp256r1.G * sig.ee;

            for (int i = 0; i < AMOUNT_SIZE; i++)
            {
                ECPoint L2 = ECCurve.Secp256r1.G * sig.s0[i]  + P1[i] * sig.ee;
                byte[] c1 = Crypto.Default.Hash256(L2.EncodePoint(true));
                ECPoint L1 = ECCurve.Secp256r1.G * sig.s1[i] + P2[i] * c1;
                ee = ScalarFunctions.Add(ee, Crypto.Default.Hash256(L1.EncodePoint(true)));
            }
            return sig.ee.ToHexString().Equals(ee.ToHexString());
        }
    }
}
