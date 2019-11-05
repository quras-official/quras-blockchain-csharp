using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

using Pure.Cryptography;
using Pure.Cryptography.ECC;

namespace Pure.Core.RingCT.Impls
{
    public static class ScalarFunctions
    {
        public static BigInteger l = BigInteger.Pow(2, 252) + BigInteger.Parse("27742317777372353535851937790883648493", System.Globalization.NumberStyles.AllowDecimalPoint);
        /// <summary>
        /// (c - ab) mod l
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static byte[] MulSub(byte[] a, byte[] b, byte[] c)
        {
            BigInteger b_a = new BigInteger(a.Reverse().Concat(new byte[1]).ToArray());
            BigInteger b_b = new BigInteger(b.Reverse().Concat(new byte[1]).ToArray());
            BigInteger b_c = new BigInteger(c.Reverse().Concat(new byte[1]).ToArray());

            BigInteger b_ret = (b_c - b_a * b_b).Mod(Pure.Cryptography.ECC.ECCurve.Secp256r1.N);

            return b_ret.ToByteArray().Reverse().ToArray().FixLength();
        }

        public static byte[] Add(byte[] a, byte[] b)
        {
            BigInteger b_a = new BigInteger(a.Reverse().Concat(new byte[1]).ToArray());
            BigInteger b_b = new BigInteger(b.Reverse().Concat(new byte[1]).ToArray());

            BigInteger b_ret = (b_a + b_b).Mod(Pure.Cryptography.ECC.ECCurve.Secp256r1.N);

            return b_ret.ToByteArray().Reverse().ToArray().FixLength();
        }

        public static byte[] Sub(byte[] a, byte[] b)
        {
            BigInteger b_a = new BigInteger(a.Reverse().Concat(new byte[1]).ToArray());
            BigInteger b_b = new BigInteger(b.Reverse().Concat(new byte[1]).ToArray());

            BigInteger b_ret = (b_a - b_b).Mod(Pure.Cryptography.ECC.ECCurve.Secp256r1.N);

            return b_ret.ToByteArray().Reverse().ToArray().FixLength();
        }

        public static byte[] Mul(byte[] a, byte[] b)
        {
            BigInteger b_a = new BigInteger(a.Reverse().Concat(new byte[1]).ToArray());
            BigInteger b_b = new BigInteger(b.Reverse().Concat(new byte[1]).ToArray());

            BigInteger b_ret = (b_a * b_b).Mod(Pure.Cryptography.ECC.ECCurve.Secp256r1.N);

            return b_ret.ToByteArray().Reverse().ToArray().FixLength();
        }

        public static byte[] MulWithPoint(ECPoint P, byte[] xx)
        {
            //BigInteger x = new BigInteger(xx.Reverse().Concat(new byte[1]).ToArray());
            ECPoint Pxx = P * xx;

            return Pxx.ToString().HexToBytes();

            //BigInteger ii = x * hashPub;

            //return ii.ToByteArray().Reverse().ToArray();
        }
    }
}
