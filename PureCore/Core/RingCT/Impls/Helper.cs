using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

using Pure.Cryptography.ECC;
using Pure.Core.RingCT.Types;

namespace Pure.Core.RingCT.Impls
{
    public static class Helper
    {
        public static int ToRandomInt(this int limit)
        {
            Random rand = new Random();
            return rand.Next() % limit;
        }

        public static List<int> ToBinaryFormat(this Fixed8 amount)
        {
            long lAmount = amount.GetData();
            List<int> binary = new List<int>();
            int i = 0;

            while (lAmount != 0)
            {
                binary.Add((int)(lAmount & 1));
                i++;
                lAmount >>= 1;
            }

            while (i < 64)
            {
                binary.Add(0);
                i++;
            }

            return binary;
        }

        public static Fixed8 ToFixed8(this byte[] binary)
        {
            BigInteger amount = new BigInteger(binary.Reverse().Concat(new byte[1]).ToArray());

            Fixed8 ret = new Fixed8((long)amount);
            return ret;
        }

        public static byte[] ToBinary(this List<int> amount)
        {
            BigInteger ret = BigInteger.Zero;

            for (int i = 0; i < amount.Count; i++)
            {
                if (amount[i] == 1)
                {
                    ret += BigInteger.Pow(2, i);
                }
            }

            return ret.ToByteArray().Reverse().ToArray().FixLength();
        }

        public static EcdhTuple EcdhEncode(this EcdhTuple unmask, ECPoint receiverPk)
        {
            EcdhTuple ret = new EcdhTuple();
            byte[] esk = SchnorrNonLinkable.GenerateRandomScalar();
            ret.senderPK = ECCurve.Secp256r1.G * esk;

            byte[] sharedSec1 = Cryptography.Crypto.Default.Hash256(ScalarFunctions.MulWithPoint(receiverPk, esk));
            byte[] sharedSec2 = Cryptography.Crypto.Default.Hash256(sharedSec1);

            ret.mask = ScalarFunctions.Add(unmask.mask, sharedSec1);
            ret.amount = ScalarFunctions.Add(unmask.amount, sharedSec2);
            return ret;
        }

        public static EcdhTuple EcdhDecode(this EcdhTuple mask, byte[] recieverSK)
        {
            EcdhTuple unmask = mask;
            byte[] sharedSec1 = Cryptography.Crypto.Default.Hash256(ScalarFunctions.MulWithPoint(mask.senderPK, recieverSK));
            byte[] sharedSec2 = Cryptography.Crypto.Default.Hash256(sharedSec1);

            unmask.mask = ScalarFunctions.Sub(mask.mask, sharedSec1);
            unmask.amount = ScalarFunctions.Sub(mask.amount, sharedSec2);

            return unmask;
        }
    }
}
