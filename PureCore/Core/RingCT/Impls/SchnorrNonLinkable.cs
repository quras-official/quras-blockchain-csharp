using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

using Pure.Cryptography;
using Pure.Core.RingCT.Types;
using Pure.Cryptography.ECC;

namespace Pure.Core.RingCT.Impls
{
    /// <summary>
    /// Schnorr Non-linkable
    /// Generate gives a signature (L1, s1, s2) proving that the sender knows "x" such that xG = one of P1 or P2
    /// Verify verifies that signer knows an "x" such that xG = one of P1 or P2
    /// </summary>
    public static class SchnorrNonLinkable
    {
        public static byte[] GenerateRandomScalar()
        {
            byte[] randomScalar = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomScalar);
            }

            return randomScalar;
        }

        public static Cryptography.ECC.ECPoint GenerateRandomPoint()
        {
            return Cryptography.ECC.ECCurve.Secp256r1.G * GenerateRandomScalar();
        }

        public static byte[] PointToScalar(Cryptography.ECC.ECPoint pt)
        {
            return new byte[0];
        }

        public static SchnorrSignatureType Generate(byte[] x, Cryptography.ECC.ECPoint P1, Cryptography.ECC.ECPoint P2, int index)
        {
            byte[] a = GenerateRandomScalar();

            if (index == 0)
            {
                Cryptography.ECC.ECPoint L1 = Cryptography.ECC.ECCurve.Secp256r1.G * a;

                byte[] s2 = GenerateRandomScalar();
                byte[] c2 = Crypto.Default.Hash256(L1.EncodePoint(true));

                Cryptography.ECC.ECPoint L2 = Cryptography.ECC.ECCurve.Secp256r1.G * s2 + P2 * c2;

                byte[] c1 = Crypto.Default.Hash256(L2.EncodePoint(true));
                byte[] s1 = ScalarFunctions.MulSub(c1, x, a);

                SchnorrSignatureType retSig = new SchnorrSignatureType(L1, s1, s2);

                return retSig;
            }
            else if (index == 1)
            {
                Cryptography.ECC.ECPoint L2 = Cryptography.ECC.ECCurve.Secp256r1.G * a;

                byte[] s1 = GenerateRandomScalar();
                byte[] c1 = Crypto.Default.Hash256(L2.EncodePoint(true));

                Cryptography.ECC.ECPoint L1 = Cryptography.ECC.ECCurve.Secp256r1.G * s1 + P1 * c1;

                byte[] c2 = Crypto.Default.Hash256(L1.EncodePoint(true));
                byte[] s2 = ScalarFunctions.MulSub(c2, x, a);

                SchnorrSignatureType retSig = new SchnorrSignatureType(L1, s1, s2);

                return retSig;
            }
            else
            {
                throw new Exception("SchnorrNonLinkable Index Overload Error!");
            }
        }

        public static bool Verify(Cryptography.ECC.ECPoint P1, Cryptography.ECC.ECPoint P2, SchnorrSignatureType sig)
        {
            byte[] c2 = Crypto.Default.Hash256(sig.L1.EncodePoint(true));

            Cryptography.ECC.ECPoint L2 = Cryptography.ECC.ECCurve.Secp256r1.G * sig.s2 + P2 * c2;

            byte[] c1 = Crypto.Default.Hash256(L2.EncodePoint(true));

            Cryptography.ECC.ECPoint L1P = Cryptography.ECC.ECCurve.Secp256r1.G * sig.s1 + P1 * c1;

            return sig.L1.ToString() == L1P.ToString();
        }
    }
}
