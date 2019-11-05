using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Pure.IO;
using Pure.Core;
using Pure.Cryptography;

namespace Pure.Wallets.StealthKey
{
    public class StealthPubKeys : ISerializable
    {
        public Cryptography.ECC.ECPoint PayloadPubKey;
        public Cryptography.ECC.ECPoint ViewPubKey;

        public StealthPubKeys()
        {
            PayloadPubKey = new Cryptography.ECC.ECPoint();
            ViewPubKey = new Cryptography.ECC.ECPoint();
        }

        public StealthPubKeys(Cryptography.ECC.ECPoint payloadPubKey, Cryptography.ECC.ECPoint viewPubKey)
        {
            this.PayloadPubKey = payloadPubKey;
            this.ViewPubKey = viewPubKey;
        }

        public UInt160 GetPublicKeyHash()
        {
            byte[] pubKeyBuffer = new byte[66];
            Buffer.BlockCopy(this.PayloadPubKey.EncodePoint(true), 0, pubKeyBuffer, 0, 33);
            Buffer.BlockCopy(this.ViewPubKey.EncodePoint(true), 0, pubKeyBuffer, 33, 33);
            return pubKeyBuffer.ToScriptHash();
        }
        
        /// <summary>
         /// Get the One-Time public key from r private key
         /// </summary>
         /// <param name="senderPrivKey"></param>
         /// <returns></returns>
        public byte[] GenPaymentPubKeyHash(byte[] senderPrivKey)
        {
            Cryptography.ECC.ECPoint S = this.ViewPubKey * senderPrivKey;
            byte[] d = Crypto.Default.Hash256(S.EncodePoint(true));
            Cryptography.ECC.ECPoint D = Cryptography.ECC.ECCurve.Secp256r1.G * d;
            Cryptography.ECC.ECPoint E = this.PayloadPubKey + D;

            return E.EncodePoint(true);
        }

        /// <summary>
        /// Get the One-Time address from r private key
        /// </summary>
        /// <param name="senderPrivKey"></param>
        /// <returns></returns>
        public string GenPaymentOneTimeAddress(byte[] senderPrivKey)
        {
            byte[] pubKeyHash = GenPaymentPubKeyHash(senderPrivKey);

            byte[] data = new byte[34];
            data[0] = Settings.Default.StealthAddressVersion;
            Buffer.BlockCopy(pubKeyHash.ToArray(), 0, data, 1, 33);
            return data.Base58CheckEncode();
        }


        public int Size => PayloadPubKey.Size + ViewPubKey.Size;

        public void Deserialize(BinaryReader reader)
        {
            PayloadPubKey = reader.ReadSerializable<Cryptography.ECC.ECPoint>();
            ViewPubKey = reader.ReadSerializable<Cryptography.ECC.ECPoint>();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(PayloadPubKey);
            writer.Write(ViewPubKey);
        }
    }
}
