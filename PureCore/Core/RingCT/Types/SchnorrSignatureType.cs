using System;
using System.Collections.Generic;
using System.Text;

using Pure.Cryptography.ECC;

namespace Pure.Core.RingCT.Types
{
    public class SchnorrSignatureType
    {
        public ECPoint L1;
        public byte[] s1;
        public byte[] s2;

        public SchnorrSignatureType()
        {
            L1 = new ECPoint();
            s1 = new byte[32];
            s2 = new byte[32];
        }

        public SchnorrSignatureType(ECPoint L1, byte[] s1, byte[] s2)
        {
            this.s1 = new byte[32];
            this.s2 = new byte[32];

            this.L1 = L1;
            Buffer.BlockCopy(s1, 0, this.s1, 0, 32);
            Buffer.BlockCopy(s2, 0, this.s2, 0, 32);
        }

    }
}
