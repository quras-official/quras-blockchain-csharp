using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

using Pure;
using Pure.Cryptography;
using PureCore.Wallets.AnonymousKey.Key;
using PureCore.Wallets.AnonymousKey.PRF;

namespace Pure.Core.Anonoymous
{
    public class Note
    {
        public UInt256 a_pk;
        public Fixed8 value;
        public UInt256 rho;
        public UInt256 r;
        public UInt256 assetID;

        public Note()
        {
            byte[] random_byte256 = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random_byte256);
            }

            this.a_pk = new UInt256(random_byte256);

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random_byte256);
            }
            this.rho = new UInt256(random_byte256);

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random_byte256);
            }
            this.r = new UInt256(random_byte256);

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random_byte256);
            }
            this.assetID = new UInt256(random_byte256);

            value = new Fixed8(0);
        }

        public Note(UInt256 assetID)
        {
            byte[] random_byte256 = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random_byte256);
            }

            this.a_pk = new UInt256(random_byte256);

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random_byte256);
            }
            this.rho = new UInt256(random_byte256);

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random_byte256);
            }
            this.r = new UInt256(random_byte256);

            this.assetID = assetID;

            value = new Fixed8(0);
        }

        public Note(UInt256 a_pk, Fixed8 value, UInt256 rho, UInt256 r, UInt256 assetID)
        {
            this.a_pk = a_pk;
            this.value = value;
            this.rho = rho;
            this.r = r;
            this.assetID = assetID;
        }

        public UInt256 CM()
        {
            byte[] byValue = Encoding.ASCII.GetBytes(this.value.ToString());
            byte[] buffer = new byte[a_pk.Size + byValue.Length + rho.Size + r.Size + 1];

            buffer[0] = 0x48;
            Buffer.BlockCopy(a_pk.ToArray(), 0, buffer, 1, a_pk.Size);
            Buffer.BlockCopy(byValue, 0, buffer, 1 + a_pk.Size , byValue.Length);
            Buffer.BlockCopy(rho.ToArray(), 0, buffer, 1 + a_pk.Size + byValue.Length, rho.Size);
            Buffer.BlockCopy(r.ToArray(), 0, buffer, 1 + a_pk.Size + byValue.Length + rho.Size, r.Size);

            byte[] hash = buffer.Sha256(0, buffer.Length);

            return new UInt256(hash);
        }

        public UInt256 Nullifier(SpendingKey a_sk)
        {
            byte[] byNullifier = new byte[32];

            SnarkDllApi.GetNullifier(a_pk.ToArray(),
                rho.ToArray(),
                r.ToArray(),
                value.GetData(),
                a_sk.ToArray(),
                byNullifier);

            byte[] byConvNullifier = new byte[32];
            for (int nuIn = 0; nuIn < 32; nuIn++)
            {
                byConvNullifier[nuIn] = byNullifier[31 - nuIn];
            }
            UInt256 nullifier = new UInt256(byConvNullifier);

            return nullifier;
        }
    }
}
