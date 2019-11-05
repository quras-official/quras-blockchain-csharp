using System;
using System.Collections.Generic;
using System.Text;

using Pure;
using PureCore.Wallets.AnonymousKey.Const;

namespace PureCore.Wallets.AnonymousKey.Note
{
    public class NoteDecryption
    {
        protected static int NOTEENCRYPTION_AUTH_BYTES = 16;

        protected static int MLEN = GConst.QR_AN_NOTEPLAINTEXT_LEADING +
                                    GConst.QR_AN_V_SIZE +
                                    GConst.QR_AN_RHO_SIZE +
                                    GConst.QR_AN_R_SIZE +
                                    GConst.QR_AN_MEMO_SIZE;

        protected static int CLEN = MLEN + NOTEENCRYPTION_AUTH_BYTES;

        protected UInt256 sk_enc;
        protected UInt256 pk_enc;

        public NoteDecryption()
        {

        }
        public NoteDecryption(UInt256 sk_enc)
        {
            this.sk_enc = sk_enc;
            this.pk_enc = NoteEncryption.generate_pubkey(sk_enc);
        }

        public byte[] Decrypt(byte[] ciphertext, UInt256 epk, UInt256 hSig, char nonce) 
        {
            byte[] dhsecret;
            dhsecret = Sodium.ScalarMult.Mult(sk_enc.ToArray(), epk.ToArray());

            byte[] K = Common.KDF(new UInt256(dhsecret), epk, pk_enc, hSig, nonce);

            nonce++;

            byte[] cipher_nonce = new byte[Common.crypto_aead_chacha20poly1305_IETF_NPUBBYTES];

            byte[] plain_text = Sodium.SecretAead.Decrypt(ciphertext, cipher_nonce, K);
            return plain_text;
        }
    }
}
