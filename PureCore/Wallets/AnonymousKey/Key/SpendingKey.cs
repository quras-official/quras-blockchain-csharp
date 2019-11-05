using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Pure;
using PureCore.Wallets.AnonymousKey.Note;
using PureCore.Wallets.AnonymousKey.PRF;
using Pure.Core.Anonoymous;

namespace PureCore.Wallets.AnonymousKey.Key
{
    public class SpendingKey : UInt252
    {
        public SpendingKey() : base()
        {

        }

        public SpendingKey(UInt256 a_sk) : base(a_sk)
        {

        }

        public static SpendingKey random()
        {
            byte[] random_byte256 = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random_byte256);
            }

            UInt256 a_sk = new UInt256(random_byte256);
            SpendingKey spendingKey = new SpendingKey(a_sk);

            return spendingKey;
        }

        public ReceivingKey receiving_key()
        {
            IntPtr ptr_rk = SnarkDllApi.Key_SpendingKey_ReceivingKey(this.ToArray());

            byte[] by_rk = new byte[32];
            System.Runtime.InteropServices.Marshal.Copy(ptr_rk, by_rk, 0, 32);

            return new ReceivingKey(new UInt256(by_rk));
            
            //return new ReceivingKey(NoteEncryption.generate_privkey(this));
        }

        public ViewingKey viewing_key()
        {
            IntPtr ptr_vk = SnarkDllApi.Key_SpendingKey_ReceivingKey(this.ToArray());

            byte[] by_a_pk = new byte[32];
            byte[] by_sk_enc = new byte[32];
            System.Runtime.InteropServices.Marshal.Copy(ptr_vk, by_a_pk, 0, 32);
            System.Runtime.InteropServices.Marshal.Copy(ptr_vk + 32, by_sk_enc, 0, 32);

            return new ViewingKey(new UInt256(by_a_pk), new ReceivingKey(new UInt256(by_sk_enc)));
            //return new ViewingKey(PRFClass.PRF_addr_a_pk(this), receiving_key());
        }

        public PaymentAddress address()
        {
            IntPtr ptr_pa = SnarkDllApi.Key_SpendingKey_Address(this.ToArray());

            byte[] by_a_pk = new byte[32];
            byte[] by_pk_enc = new byte[32];
            System.Runtime.InteropServices.Marshal.Copy(ptr_pa, by_a_pk, 0, 32);
            System.Runtime.InteropServices.Marshal.Copy(ptr_pa + 32, by_pk_enc, 0, 32);

            return new PaymentAddress(new UInt256(by_a_pk), new UInt256(by_pk_enc));
            //return viewing_key().address();
        }
    }
}
