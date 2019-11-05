using System;
using System.Collections.Generic;
using System.Text;
using Pure;

namespace PureCore.Wallets.AnonymousKey.Key
{
    public class ViewingKey
    {
        public UInt256 a_pk;
        public ReceivingKey sk_enc;

        public ViewingKey()
        {
            a_pk = new UInt256();
            sk_enc = new ReceivingKey();
        }

        public ViewingKey(UInt256 p_a_pk, ReceivingKey p_sk_enc)
        {
            a_pk = new UInt256(p_a_pk);
            sk_enc = new ReceivingKey(p_sk_enc);
        }

        public PaymentAddress address()
        {
            return new PaymentAddress(a_pk, sk_enc.pk_enc());
        }
    }
}
