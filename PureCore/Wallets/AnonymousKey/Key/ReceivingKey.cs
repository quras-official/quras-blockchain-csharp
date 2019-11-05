using System;
using System.Collections.Generic;
using System.Text;
using Pure;
using PureCore.Wallets.AnonymousKey.Note;
using Pure.Core.Anonoymous;

namespace PureCore.Wallets.AnonymousKey.Key
{
    public class ReceivingKey : UInt256
    {
        public ReceivingKey()
            : base()
        {

        }

        public ReceivingKey(UInt256 sk_enc)
            : base(sk_enc)
        {

        }

        public UInt256 pk_enc()
        {
            IntPtr ptr_pk_enc =  SnarkDllApi.Key_ReceivingKey_Pk_enc(this.ToArray());

            byte[] by_pk_enc = new byte[32];
            System.Runtime.InteropServices.Marshal.Copy(ptr_pk_enc, by_pk_enc, 0, 32);

            return new UInt256(by_pk_enc);
            //return NoteEncryption.generate_pubkey(this);
        }
    }
}
