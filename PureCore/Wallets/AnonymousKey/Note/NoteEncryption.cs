using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

using Pure;
using PureCore.Wallets.AnonymousKey.PRF;
using PureCore.Wallets.AnonymousKey.Const;

namespace PureCore.Wallets.AnonymousKey.Note
{
    public class NoteEncryption
    {
        protected static int MLEN = GConst.QR_AN_NOTEPLAINTEXT_LEADING +
                                    GConst.QR_AN_V_SIZE +
                                    GConst.QR_AN_RHO_SIZE +
                                    GConst.QR_AN_R_SIZE +
                                    GConst.QR_AN_MEMO_SIZE;

        protected static int CLEN = MLEN + Common.NOTEENCRYPTION_AUTH_BYTES;

        protected UInt256 epk;
        protected UInt256 esk;
        protected char nonce;
        protected UInt256 hSig;

        //public Char[] Ciphertext = new Char[CLEN];
        //public Char[] Plaintext = new Char[MLEN];
        
        // Gets the ephemeral secret key
        public UInt256 get_esk()
        {
            return esk;
        }

        // Gets the ephemeral public key
        public UInt256 get_epk()
        {
            return epk;
        }

        public NoteEncryption(UInt256 hSig)
        {
            this.hSig = new UInt256(hSig);
            nonce = (char)0;

            // Create the ephemeral keypair
            byte[] random_byte256 = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random_byte256);
            }

            this.esk = new UInt256(random_byte256);
            this.epk = generate_pubkey(this.esk);
        }

        // Creates a NoteEncryption private key
        public static UInt256 generate_privkey(UInt252 a_sk)
        {
            UInt256 sk = PRFClass.PRF_addr_sk_enc(a_sk);

            clamp_curve25519(sk.ToArray());

            return sk;
        }

        // Creates a NoteEncryption public key from a private key
        public static UInt256 generate_pubkey(UInt256 sk_enc)
        {
            byte[] byPk;
            UInt256 pk;

            try
            {
                byPk = Sodium.ScalarMult.Base(sk_enc.ToArray());
            }
            catch (Sodium.Exceptions.KeyOutOfRangeException e)
            {
                return null;
            }

            pk = new UInt256(byPk);
            return pk;
        }

        public static void clamp_curve25519(byte[] key)
        {
            key[0] &= 248;
            key[31] &= 127;
            key[31] |= 64;
        }

        public byte[] Encrypt(UInt256 pk_enc, byte[] message)
        {
            byte[] dhsecret;
            dhsecret = Sodium.ScalarMult.Mult(esk.ToArray(), pk_enc.ToArray());

            byte[] K = Common.KDF(new UInt256(dhsecret), epk, pk_enc, hSig, nonce);

            nonce++;

            byte[] cipher_nonce = new byte[Common.crypto_aead_chacha20poly1305_IETF_NPUBBYTES];
            byte[] cipher_text = new byte[CLEN];

            cipher_text = Sodium.SecretAead.Encrypt(message, cipher_nonce, K);
            byte[] plain_text = Sodium.SecretAead.Decrypt(cipher_text, cipher_nonce, K);
            return cipher_text;
        }
    }
}
