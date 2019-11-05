using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

using Pure;
using PureCore.Wallets.AnonymousKey.Key;
using PureCore.Wallets.AnonymousKey.PRF;

namespace Pure.Core.Anonoymous
{
    public class JSOutput
    {
        public PaymentAddress addr;
        public Fixed8 value;
        public Fixed8 fee;
        public byte[] memo;
        public UInt256 AssetID;

        public JSOutput()
        {
            byte[] random_byte256 = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random_byte256);
            }

            SpendingKey a_sk = new SpendingKey(new UInt256(random_byte256));
            addr = a_sk.address();

            AssetID = Blockchain.GoverningToken.Hash;

            memo = new byte[0];
        }

        public JSOutput(PaymentAddress addr, Fixed8 value, Fixed8 fee, UInt256 Asset_ID)
        {
            this.addr = addr;
            this.value = value;
            this.fee = fee;
            this.memo = new byte[0];
            this.AssetID = Asset_ID;
        }

        public JSOutput(PaymentAddress addr, Fixed8 value, UInt256 Asset_ID)
        {
            this.addr = addr;
            this.value = value;
            this.fee = Fixed8.Zero;
            this.memo = new byte[0];
            this.AssetID = Asset_ID;
        }

        public Note note(UInt252 phi, UInt256 r, Fixed8 i, UInt256 h_sig)
        {
            UInt256 rho = PRFClass.PRF_rho(phi, i, h_sig);

            return new Note(addr.a_pk, value, rho, r, AssetID);
        }
    }
}
