using System;
using System.Collections.Generic;
using System.Text;
using Pure;

namespace PureCore.Wallets.AnonymousKey.Note
{
    public class Common
    {
        public static int NOTEENCRYPTION_AUTH_BYTES = 16;
        public static int NOTEENCRYPTION_CIPHER_KEYSIZE = 32;
        public static int crypto_generichash_blake2b_PERSONALBYTES = 16;
        public static int crypto_aead_chacha20poly1305_IETF_NPUBBYTES = 8;

        public static byte[] KDF(UInt256 dhsecret,
                UInt256 epk,
                UInt256 pk_enc,
                UInt256 hSig,
                char nonce)
        {
            if (nonce == 0xff)
            {
                throw new FormatException("no additional nonce space for KDF");
            }

            try
            {
                byte[] block = new byte[128];

                Buffer.BlockCopy(hSig.ToArray(), 0, block, 0, 32);
                Buffer.BlockCopy(dhsecret.ToArray(), 0, block, 32, 32);
                Buffer.BlockCopy(epk.ToArray(), 0, block, 64, 32);
                Buffer.BlockCopy(pk_enc.ToArray(), 0, block, 96, 32);

                Sodium.GenericHash key = new Sodium.GenericHash();
                byte[] personalization = new byte[crypto_generichash_blake2b_PERSONALBYTES];
                char[] personalization_header = { 'Q', 'u', 'r', 'a', 's', 'K', 'D', 'F' };
                Buffer.BlockCopy(Encoding.Default.GetBytes(personalization_header), 0, personalization, 0, 8);
                personalization[8] = (byte)nonce;

                byte[] salt = new byte[crypto_generichash_blake2b_PERSONALBYTES];
                char[] salt_str = { 'Q', 'u', 'r', 'a', 's', 'K', 'D', 'F', 'S', 'A', 'L', 'T' };
                Buffer.BlockCopy(Encoding.Default.GetBytes(salt_str), 0, salt, 0, 12);

                byte[] K = new byte[NOTEENCRYPTION_CIPHER_KEYSIZE];

                K = Sodium.GenericHash.HashSaltPersonal(block, null, salt, personalization, NOTEENCRYPTION_CIPHER_KEYSIZE);
                return K;
            }
            catch (Exception ex)
            {
                throw new FormatException("KDF hash function failed");
            }

            return null;
        }
    }
}
