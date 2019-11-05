using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

using Pure.Cryptography;
using Pure.Cryptography.ECC;
using Pure.Core.RingCT.Types;

namespace Pure.Core.RingCT.Impls
{
    /// <summary>
    /// Multilayered Spontaneous Anonymous Group Signatures (MLSAG signatures)
    /// This is a just slghtly more efficient version than the ones described below
    /// (will be explained in more detail in Ring Multisig paper)
    /// These are aka MG signatutes in earlier drafts of the ring ct paper
    /// c.f. http://eprint.iacr.org/2015/1098 section 2.
    /// </summary>
    public static class MLSAGSignature
    {
        /// <summary>
        /// keyImageV just does I[i] = xx[i] * Hash(xx[i] * G) for each i
        /// </summary>
        /// <param name="xx"></param>
        /// <returns></returns>
        public static List<byte[]> KeyImageV(List<byte[]> xx)
        {
            List<byte[]> I = new List<byte[]>();

            for (int i = 0; i < xx.Count; i++)
            {
                BigInteger x = new BigInteger(xx[i].Reverse().Concat(new byte[1]).ToArray());
                byte[] hashP = Crypto.Default.Hash256((ECCurve.Secp256r1.G * xx[i]).ToString().HexToBytes());
                BigInteger hashPub = new BigInteger(hashP.Reverse().Concat(new byte[1]).ToArray());

                BigInteger ii = x * hashPub;

                I.Add(ii.ToByteArray().Reverse().ToArray());
            }

            return I;
        }

        public static byte[] MakeHash(List<ECPoint> PK, List<ECPoint> L, List<byte[]> R)
        {
            if (PK.Count != L.Count || PK.Count != R.Count || L.Count != R.Count)
            {
                throw new Exception("Make Hash Parameter is not correct!");
            }

            byte[] msg = new byte[PK.Count * (33 + 33 + 32)];
            for (int i = 0; i < PK.Count; i++)
            {
                //Buffer.BlockCopy(msg, i * (0), PK[i].ToString().HexToBytes(), 0, PK[i].Size);
                //Buffer.BlockCopy(msg, i * (33), L[i].ToString().HexToBytes(), 0, L[i].Size);
                //Buffer.BlockCopy(msg, i * (33 + 33), R[i], 0, R[i].Length);

                Buffer.BlockCopy(PK[i].ToString().HexToBytes(), 0, msg, i * (0), PK[i].Size);
                Buffer.BlockCopy(L[i].ToString().HexToBytes(), 0, msg, i * (33), L[i].Size);
                Buffer.BlockCopy(R[i], 0, msg, i * (33 + 33), R[i].Length);
            }

            return Crypto.Default.Hash256(msg);
        }

        public static MLSAGSignatureType Generate(List<List<ECPoint>> PK, List<byte[]> X, int index)
        {
            MLSAGSignatureType sig = new MLSAGSignatureType();

            int rows = PK[0].Count;
            int cols = PK.Count;

            #region Initialize
            for (int j = 0; j < cols; j++)
            {
                sig.ss.Add(new List<byte[]>());
            }
            #endregion

            if (cols < 2)
            {
                throw new Exception("Error! What is c if cols = 1!");
            }

            List<byte[]> alpha = new List<byte[]>();
            List<ECPoint> aG = new List<ECPoint>();
            List<byte[]> aHP = new List<byte[]>();

            List<ECPoint> Li = new List<ECPoint>();
            List<byte[]> Ri = new List<byte[]>();

            for (int j = 0; j < rows; j++)
            {
                byte[] a = SchnorrNonLinkable.GenerateRandomScalar();
                alpha.Add(a);
                aG.Add(ECCurve.Secp256r1.G * a);

                byte[] Hi = Crypto.Default.Hash256(PK[index][j].ToString().HexToBytes());
                aHP.Add(ScalarFunctions.Mul(a, Hi));

                sig.II.Add(ScalarFunctions.Mul(X[j], Hi));
            }

            byte[] c_old = MakeHash(PK[index], aG, aHP);

            int i = (index + 1) % cols;

            if (i == 0)
            {
                Buffer.BlockCopy(c_old, 0, sig.cc, 0, c_old.Length);
            }

            while (i != index)
            {
                for (int j = 0; j < rows; j++)
                {
                    sig.ss[i].Add(SchnorrNonLinkable.GenerateRandomScalar());
                }

                byte[] c = new byte[32];

                Li.Clear();
                Ri.Clear();
                for (int j = 0; j < rows; j++)
                {
                    ECPoint L = ECCurve.Secp256r1.G * sig.ss[i][j] + PK[i][j] * c_old;
                    byte[] Hi = Crypto.Default.Hash256(PK[i][j].ToString().HexToBytes());
                    byte[] R = ScalarFunctions.Add(ScalarFunctions.Mul(sig.ss[i][j], Hi), ScalarFunctions.Mul(c_old, sig.II[j]));

                    Li.Add(L);
                    Ri.Add(R);
                }

                c_old = MakeHash(PK[i], Li, Ri);

                i = (i + 1) % cols;

                if (i == 0)
                {
                    Buffer.BlockCopy(c_old, 0, sig.cc, 0, c_old.Length);
                }
            }

            for (int j = 0; j < rows; j++)
            {
                sig.ss[index].Add(ScalarFunctions.MulSub(c_old, X[j], alpha[j]));
            }

            return sig;
        }

        public static bool Verify(List<List<ECPoint>> PK, MLSAGSignatureType sig)
        {
            int rows = PK[0].Count;
            int cols = PK.Count;

            if (cols < 2)
            {
                throw new Exception("Error! What is c if cols = 1!");
            }

            int i = 0;
            byte[] c_old = new byte[32];

            Buffer.BlockCopy(sig.cc, 0, c_old, 0, sig.cc.Length);

            List<ECPoint> Li = new List<ECPoint>();
            List<byte[]> Ri = new List<byte[]>();

            while (i < cols)
            {
                Li.Clear();
                Ri.Clear();

                for (int j = 0; j < rows; j++)
                {
                    ECPoint L = ECCurve.Secp256r1.G * sig.ss[i][j] + PK[i][j] * c_old;
                    byte[] Hi = Crypto.Default.Hash256(PK[i][j].ToString().HexToBytes());
                    byte[] R = ScalarFunctions.Add(ScalarFunctions.Mul(sig.ss[i][j], Hi), ScalarFunctions.Mul(c_old, sig.II[j]));

                    Li.Add(L);
                    Ri.Add(R);
                }

                c_old = MakeHash(PK[i], Li, Ri);

                i = i + 1;
            }

            return sig.cc.ToHexString() == c_old.ToHexString();
        }
    }
}
