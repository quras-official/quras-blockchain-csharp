using Pure.Cryptography;
using Pure.SmartContract;
using Pure.VM;
using Pure.Wallets;
using Pure.Core.RingCT.Types;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Pure.Core
{
    public static class Helper
    {
        public static byte[] GetHashData(this IVerifiable verifiable)
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                verifiable.SerializeUnsigned(writer);
                writer.Flush();
                return ms.ToArray();
            }
        }

        public static byte[] Sign(this IVerifiable verifiable, KeyPair key)
        {
            using (key.Decrypt())
            {
                return Crypto.Default.Sign(verifiable.GetHashData(), key.PrivateKey, key.PublicKey.EncodePoint(false).Skip(1).ToArray());
            }
        }

        public static UInt160 ToScriptHash(this byte[] script)
        {
            return new UInt160(Crypto.Default.Hash160(script));
        }

        internal static bool VerifyScripts(this IVerifiable verifiable)
        {
            UInt160[] hashes;
            try
            {
                hashes = verifiable.GetScriptHashesForVerifying();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            if (hashes.Length != verifiable.Scripts.Length) return false;
            for (int i = 0; i < hashes.Length; i++)
            {
                byte[] verification = verifiable.Scripts[i].VerificationScript;
                if (verification.Length == 0)
                {
                    using (ScriptBuilder sb = new ScriptBuilder())
                    {
                        sb.EmitAppCall(hashes[i].ToArray());
                        verification = sb.ToArray();
                    }
                }
                else
                {
                    if (hashes[i] != verification.ToScriptHash()) return false;
                }
                ApplicationEngine engine = new ApplicationEngine(TriggerType.Verification, verifiable, Blockchain.Default, StateReader.Default, Fixed8.Zero);
                engine.LoadScript(verification, false);
                engine.LoadScript(verifiable.Scripts[i].InvocationScript, true);
                if (!engine.Execute()) return false;
                if (engine.EvaluationStack.Count != 1 || !engine.EvaluationStack.Pop().GetBoolean()) return false;
            }
            return true;
        }

        public static byte[] FixLength(this byte[] org)
        {
            byte[] ret = new byte[32];

            for (int i = 0; i < 32; i++)
            {
                if (org.Length - 1 - i >= 0)
                {
                    ret[31 - i] = org[org.Length - 1 - i];
                }
            }

            return ret;
        }

        public static RingSignature RingSign(this byte[] message, List<Cryptography.ECC.ECPoint> pubKeys, byte[] privKey, byte[] keyImage, int index)
        {
            #region Step 1.   Generate Q{i}, W{i}
            List<byte[]> Q = new List<byte[]>();
            List<byte[]> W = new List<byte[]>();

            for (int i = 0; i < RingSignature.RING_SIZE; i++)
            {
                byte[] key = new byte[32];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(key);
                }

                Q.Add(key);

                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(key);
                }

                W.Add(key);
            }
            #endregion

            #region Step 2.   Calculate L{i}, R{i}
            List<byte[]> L = new List<byte[]>();
            List<byte[]> R = new List<byte[]>();

            for (int i = 0; i < RingSignature.RING_SIZE; i++)
            {
                if (i == index)
                {
                    byte[] L_i = (Cryptography.ECC.ECCurve.Secp256r1.G * Q[i]).ToString().HexToBytes();
                    L.Add(L_i);

                    BigInteger bR_i = (new BigInteger(Q[i].Reverse().Concat(new byte[1]).ToArray()) * new BigInteger(Crypto.Default.Hash256(pubKeys[i].ToString().HexToBytes()).Reverse().Concat(new byte[1]).ToArray())).Mod(Cryptography.ECC.ECCurve.Secp256r1.N);
                    byte[] R_i = bR_i.ToByteArray().Reverse().ToArray();
                    R.Add(R_i);
                }
                else
                {
                    byte[] L_i = (Cryptography.ECC.ECCurve.Secp256r1.G * Q[i] + pubKeys[i] * W[i]).ToString().HexToBytes();
                    L.Add(L_i);

                    BigInteger R_i = ((new BigInteger(Q[i].Reverse().Concat(new byte[1]).ToArray()) * new BigInteger(Crypto.Default.Hash256(pubKeys[i].ToString().HexToBytes()).Reverse().Concat(new byte[1]).ToArray())) + (new BigInteger(W[i].Reverse().Concat(new byte[1]).ToArray()) * new BigInteger(keyImage.Reverse().Concat(new byte[1]).ToArray())).Mod(Cryptography.ECC.ECCurve.Secp256r1.N)).Mod(Cryptography.ECC.ECCurve.Secp256r1.N);
                    R.Add(R_i.ToByteArray().Reverse().ToArray());
                }
            }
            #endregion

            #region Step 3.   Generating non-interactive challenge
            // calculate size of message, L and R
            int totalSize = message.Length + L.Select(p => p.Length).Sum() + R.Select(p => p.Length).Sum();
            byte[] data = new byte[totalSize];
            int dstOff = 0;
            Buffer.BlockCopy(message, 0, data, dstOff, message.Length);
            dstOff += message.Length;

            for (int i = 0; i < RingSignature.RING_SIZE; i++)
            {
                Buffer.BlockCopy(L[i], 0, data, dstOff, L[i].Length);
                dstOff += L[i].Length;
            }

            for (int i = 0; i < RingSignature.RING_SIZE; i++)
            {
                Buffer.BlockCopy(R[i], 0, data, dstOff, R[i].Length);
                dstOff += R[i].Length;
            }

            byte[] c = Crypto.Default.Hash256(data);
            #endregion

            #region Step 4.   Calculate C{i}, R{i} => Final Stage.
            List<byte[]> C = new List<byte[]>();
            List<byte[]> T_R = new List<byte[]>();

            for (int i = 0; i < RingSignature.RING_SIZE; i++)
            {
                if (i != index)
                {
                    C.Add(W[i].FixLength());
                    T_R.Add(Q[i].FixLength());
                }
                else
                {
                    C.Add(new byte[0]);
                    T_R.Add(new byte[0]);
                }
            }

            // Calculate C{s} & T_R{s}
            BigInteger SumC = BigInteger.Zero;
            for (int i = 0; i < RingSignature.RING_SIZE; i++)
            {
                if (i != index)
                {
                    SumC += new BigInteger(C[i].Reverse().Concat(new byte[1]).ToArray());
                }
            }

            BigInteger C_s = (new BigInteger(c.Reverse().Concat(new byte[1]).ToArray()) - SumC).Mod(Cryptography.ECC.ECCurve.Secp256r1.N);
            C[index] = C_s.ToByteArray().Reverse().ToArray().FixLength();

            BigInteger R_s = (new BigInteger(Q[index].Reverse().Concat(new byte[1]).ToArray()) - (C_s * new BigInteger(privKey.Reverse().Concat(new byte[1]).ToArray()))).Mod(Cryptography.ECC.ECCurve.Secp256r1.N);
            T_R[index] = R_s.ToByteArray().Reverse().ToArray().FixLength();
            #endregion

            #region TEST
            BigInteger HP_P0 = new BigInteger(Crypto.Default.Hash256(pubKeys[0].ToString().HexToBytes()).Reverse().Concat(new byte[1]).ToArray());
            BigInteger Q0 = new BigInteger(Q[0].Reverse().Concat(new byte[1]).ToArray());
            BigInteger C0_X = C_s * new BigInteger(privKey.Reverse().Concat(new byte[1]).ToArray());
            BigInteger C0_I = C_s * new BigInteger(keyImage.Reverse().Concat(new byte[1]).ToArray());
            BigInteger C0_XHP = C0_X * HP_P0;
            #endregion

            return new RingSignature(keyImage, C, T_R);
        }

        public static bool RingVerify(this RingSignature signature, byte[] message, List<Cryptography.ECC.ECPoint> pubKeys)
        {
            #region Calculate L{i} & R{i}
            List<byte[]> L = new List<byte[]>();
            List<byte[]> R = new List<byte[]>();

            for (int i = 0; i < RingSignature.RING_SIZE; i++)
            {
                byte[] L_i = (Cryptography.ECC.ECCurve.Secp256r1.G * signature.R[i] + pubKeys[i] * signature.C[i]).ToString().HexToBytes();
                L.Add(L_i);

                BigInteger R_i = ((new BigInteger(signature.R[i].Reverse().Concat(new byte[1]).ToArray()) * new BigInteger(Crypto.Default.Hash256(pubKeys[i].ToString().HexToBytes()).Reverse().Concat(new byte[1]).ToArray())) + (new BigInteger(signature.C[i].Reverse().Concat(new byte[1]).ToArray()) * new BigInteger(signature.KeyImage.Reverse().Concat(new byte[1]).ToArray())).Mod(Cryptography.ECC.ECCurve.Secp256r1.N)).Mod(Cryptography.ECC.ECCurve.Secp256r1.N);
                R.Add(R_i.ToByteArray().Reverse().ToArray());
            }
            #endregion

            #region Calculate Sigma C{i}
            BigInteger SigmaC = BigInteger.Zero;
            for (int i = 0; i < RingSignature.RING_SIZE; i++)
            {
                SigmaC += new BigInteger(signature.C[i].Reverse().Concat(new byte[1]).ToArray());
            }

            SigmaC = SigmaC.Mod(Cryptography.ECC.ECCurve.Secp256r1.N);
            #endregion

            #region Calcuate Hash
            int totalSize = message.Length + L.Select(p => p.Length).Sum() + R.Select(p => p.Length).Sum();
            byte[] data = new byte[totalSize];
            int dstOff = 0;
            Buffer.BlockCopy(message, 0, data, dstOff, message.Length);
            dstOff += message.Length;

            for (int i = 0; i < RingSignature.RING_SIZE; i++)
            {
                Buffer.BlockCopy(L[i], 0, data, dstOff, L[i].Length);
                dstOff += L[i].Length;
            }

            for (int i = 0; i < RingSignature.RING_SIZE; i++)
            {
                Buffer.BlockCopy(R[i], 0, data, dstOff, R[i].Length);
                dstOff += R[i].Length;
            }

            byte[] c = Crypto.Default.Hash256(data);
            BigInteger iC = (new BigInteger(c.Reverse().Concat(new byte[1]).ToArray())).Mod(Cryptography.ECC.ECCurve.Secp256r1.N);

            #endregion

            if (SigmaC != iC)
            {
                return false;
            }
            return true;
        }
    }
}