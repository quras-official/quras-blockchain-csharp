using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Pure;

namespace PureCore.Wallets.AnonymousKey.PRF
{
    public class PRFClass
    {
        public PRFClass()
        {

        }

        public static UInt256 PRF(bool a, bool b, bool c, bool d, UInt252 x, UInt256 y)
        {
            UInt256 res;
            byte[] blob = new byte[64];

            x.ToArray().ToHexString().HexToBytesInverse().CopyTo(blob, 0);
            y.ToArray().ToHexString().HexToBytesInverse().CopyTo(blob, 32);

            blob[0] &= 0x0F;
            blob[0] |= (byte)((a ? 1 << 7 : 0) | (b ? 1 << 6 : 0) | (c ? 1 << 5 : 0) | (d ? 1 << 4 : 0));

            using (SHA256 hasher = SHA256.Create())
            {
                byte[] hashValue;
                hashValue = hasher.ComputeHash(blob);
                res = new UInt256(hashValue);
            }
            return res;
        }

        public static UInt256 PRF_addr(UInt252 a_sk, byte t)
        {
            UInt256 temp = new UInt256();
            byte[] byTemp = temp.ToArray();
            byTemp[0] = t;

            UInt256 y = new UInt256(byTemp);

            return PRF(true, true, false, false, a_sk, y);
        }

        public static UInt256 PRF_nf(UInt252 a_sk, UInt256 rho)
        {
            return PRF(true, true, true, false, a_sk, rho);
        }

        public static UInt256 PRF_addr_a_pk(UInt252 a_sk)
        {
            return PRF_addr(a_sk, 0);
        }

        public static UInt256 PRF_addr_sk_enc(UInt252 a_sk)
        {
            return PRF_addr(a_sk, 1);
        }

        public static UInt256 PRF_rho(UInt252 phi, Fixed8 i0, UInt256 h_sig)
        {
            if ((i0.GetData() != 0) && (i0.GetData() != 1))
            {
                throw new FormatException("i0 is not correct");
            }

            if (i0.GetData() == 0)
            {
                return PRF(false, false, true, false, phi, h_sig);
            }
            else
            {
                return PRF(false, true, true, false, phi, h_sig);
            }
            
        }

        public static UInt256 PRF_pk(UInt252 a_sk, Fixed8 i0, UInt256 h_sig)
        {
            if ((i0.GetData() != 0) && (i0.GetData() != 1))
            {
                throw new FormatException("i0 is not correct");
            }

            if (i0.GetData() == 0)
            {
                return PRF(false, false, false, false, a_sk, h_sig);
            }
            else
            {
                return PRF(false, true, false, false, a_sk, h_sig);
            }
        }
    }
}
