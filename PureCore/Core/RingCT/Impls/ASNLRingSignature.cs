using System;
using System.Collections.Generic;
using System.Text;

using Pure.Core.RingCT.Types;
using Pure.Cryptography;
using Pure.Cryptography.ECC;

namespace Pure.Core.RingCT.Impls
{
    /// <summary>
    /// Aggregate Schnorr Non-Linkable Ring Signature (ASNL)
    /// c.f. http://eprint.iacr.org/2015/1098 section 5.
    /// These are used in range proofs (alternatively Borromean could be used)
    /// Generate gives a signature which proves the signer knows, for each i, an x[i] such that x[i]G = one of P1[i] or P2[i]
    /// Verify verifies the signer knows a key for one of P1[i], P2[i] at each i
    /// </summary>
    public static class ASNLRingSignature
    {
        public static int AMOUNT_SIZE = 64;
        public static ASNLSignatureType Generate(List<byte[]> x, List<Cryptography.ECC.ECPoint> P1, List<Cryptography.ECC.ECPoint> P2, List<int> indices)
        {
            List<byte[]> s1 = new List<byte[]>();

            ASNLSignatureType asnlSig = new ASNLSignatureType();
            asnlSig.InitSField();

            for (int i = 0; i < AMOUNT_SIZE; i++)
            {
                SchnorrSignatureType schnorrSig = SchnorrNonLinkable.Generate(x[i], P1[i], P2[i], indices[i]);

                if (!SchnorrNonLinkable.Verify(P1[i], P2[i], schnorrSig))
                    throw new Exception("Schnorr Sign Error!");

                asnlSig.L1.Add(schnorrSig.L1);
                s1.Add(schnorrSig.s1);
                asnlSig.s2.Add(schnorrSig.s2);

                asnlSig.s = ScalarFunctions.Add(asnlSig.s, s1[i]);
            }

            return asnlSig.Exports();
        }

        public static bool Verify(List<ECPoint> P1, List<ECPoint> P2, ASNLSignatureType sig)
        {
            ECPoint LHS = sig.L1[0];
            ECPoint RHS = ECCurve.Secp256r1.G * sig.s;

            for (int i = 0; i < AMOUNT_SIZE; i++)
            {
                byte[] c2 = Crypto.Default.Hash256(sig.L1[i].EncodePoint(true));
                ECPoint L2 = ECCurve.Secp256r1.G * sig.s2[i] + P2[i] * c2;
                byte[] c1 = Crypto.Default.Hash256(L2.EncodePoint(true));

                if (i > 0)
                {
                    LHS = LHS + sig.L1[i];
                }

                RHS = RHS + P1[i] * c1;
            }

            return LHS.ToString() == RHS.ToString();
        }
    }
}
