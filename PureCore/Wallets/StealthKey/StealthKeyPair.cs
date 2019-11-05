using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Numerics;

using Pure.Core;
using Pure.Cryptography;
namespace Pure.Wallets.StealthKey
{
    public class StealthKeyPair : KeyPairBase, IEquatable<StealthKeyPair>
    {
        public readonly byte[] PayloadPrivKey;
        public readonly Cryptography.ECC.ECPoint PayloadPubKey;

        public readonly byte[] ViewPrivKey;
        public readonly Cryptography.ECC.ECPoint ViewPubKey;

        public StealthKeyPair(byte[] payloadPrivKey, byte[] viewPrivKey, byte[] payloadPubKey, byte[] viewPubKey)
        {
            this.nVersion = KeyType.Stealth;

            this.PayloadPrivKey = payloadPrivKey;
            this.ViewPrivKey = viewPrivKey;

            this.PayloadPubKey = Cryptography.ECC.ECPoint.DecodePoint(payloadPubKey, Cryptography.ECC.ECCurve.Secp256r1);
            this.ViewPubKey = Cryptography.ECC.ECPoint.DecodePoint(viewPubKey, Cryptography.ECC.ECCurve.Secp256r1);
        }

        public StealthKeyPair(byte[] payloadPrivKey, byte[] viewPrivKey)
        {
            this.nVersion = KeyType.Stealth;

            #region Payload Key Section
            if (payloadPrivKey.Length != 32 && payloadPrivKey.Length != 96 && payloadPrivKey.Length != 104)
                throw new ArgumentException();

            this.PayloadPrivKey = new byte[32];
            Buffer.BlockCopy(payloadPrivKey, payloadPrivKey.Length - 32, PayloadPrivKey, 0, 32);

            if (payloadPrivKey.Length == 32)
            {
                this.PayloadPubKey = Cryptography.ECC.ECCurve.Secp256r1.G * payloadPrivKey;
            }
            else
            {
                this.PayloadPubKey = Cryptography.ECC.ECPoint.FromBytes(payloadPrivKey, Cryptography.ECC.ECCurve.Secp256r1);
            }


#if NET461
            ProtectedMemory.Protect(PayloadPrivKey, MemoryProtectionScope.SameProcess);
#endif
            #endregion

            #region View Key Section
            if (viewPrivKey.Length != 32 && viewPrivKey.Length != 96 && viewPrivKey.Length != 104)
                throw new ArgumentException();

            this.ViewPrivKey = new byte[32];
            Buffer.BlockCopy(viewPrivKey, viewPrivKey.Length - 32, ViewPrivKey, 0, 32);

            if (viewPrivKey.Length == 32)
            {
                this.ViewPubKey = Cryptography.ECC.ECCurve.Secp256r1.G * viewPrivKey;
            }
            else
            {
                this.ViewPubKey = Cryptography.ECC.ECPoint.FromBytes(viewPrivKey, Cryptography.ECC.ECCurve.Secp256r1);
            }

#if NET461
            ProtectedMemory.Protect(ViewPrivKey, MemoryProtectionScope.SameProcess);
#endif
            #endregion

            byte[] pubKeyBuffer = new byte[66];
            Buffer.BlockCopy(this.PayloadPubKey.EncodePoint(true), 0, pubKeyBuffer, 0, 33);
            Buffer.BlockCopy(this.ViewPubKey.EncodePoint(true), 0, pubKeyBuffer, 33, 33);
            this.PublicKeyHash = pubKeyBuffer.ToScriptHash();
        }

        public StealthPubKeys ToStelathPubKeys()
        {
            StealthPubKeys key = new StealthPubKeys(PayloadPubKey, ViewPubKey);
            return key;
        }

        public IDisposable DecryptPayloadKey()
        {
#if NET461
            return new ProtectedMemoryContext(PayloadPrivKey, MemoryProtectionScope.SameProcess);
#else
            return new System.IO.MemoryStream(0);
#endif
        }

        public IDisposable DecryptViewKey()
        {
#if NET461
            return new ProtectedMemoryContext(ViewPrivKey, MemoryProtectionScope.SameProcess);
#else
            return new System.IO.MemoryStream(0);
#endif
        }
        
        public bool Equals(StealthKeyPair other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            return PayloadPubKey.Equals(other.PayloadPubKey) && ViewPubKey.Equals(other.ViewPubKey);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StealthKeyPair);
        }

        public UInt160 GetPublicKeyHash()
        {
            byte[] pubKeyBuffer = new byte[66];
            Buffer.BlockCopy(this.PayloadPubKey.EncodePoint(true), 0, pubKeyBuffer, 0, 33);
            Buffer.BlockCopy(this.ViewPubKey.EncodePoint(true), 0, pubKeyBuffer, 33, 33);
            return pubKeyBuffer.ToScriptHash();
        }

        /// <summary>
        /// Get the One-Time public key from R of Tx
        /// </summary>
        /// <param name="R"></param>
        /// <returns></returns>
        public byte[] GetPaymentPubKeyFromR(Cryptography.ECC.ECPoint R)
        {
            if (PayloadPrivKey == null || ViewPrivKey == null)
            {
                return new byte[33];
            }

            using (DecryptPayloadKey())
            {
                using (DecryptViewKey())
                {
                    Cryptography.ECC.ECPoint S = R * this.ViewPrivKey;
                    byte[] d = Crypto.Default.Hash256(S.EncodePoint(true));
                    Cryptography.ECC.ECPoint D = Cryptography.ECC.ECCurve.Secp256r1.G * d;
                    Cryptography.ECC.ECPoint E = D + Cryptography.ECC.ECCurve.Secp256r1.G * this.PayloadPrivKey;

                    return E.EncodePoint(true);
                }
            }
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

        /// <summary>
        /// Get the One-Time Private key from R of TX
        /// </summary>
        /// <param name="R"></param>
        /// <returns></returns>
        public byte[] GenOneTimePrivKey(Pure.Cryptography.ECC.ECPoint R)
        {
            using (DecryptViewKey())
            {
                using (DecryptPayloadKey())
                {
                    Cryptography.ECC.ECPoint S = R * this.ViewPrivKey;
                    byte[] d = Crypto.Default.Hash256(S.EncodePoint(true));
                    byte[] ret = Pure.Core.RingCT.Impls.ScalarFunctions.Add(d, this.PayloadPrivKey);

                    return ret;
                }
            }
        }

        public KeyPair CheckPaymentPubKeyHash(Cryptography.ECC.ECPoint opReturnPubKey, string pubKeyHashToCompare)
        {
            using (DecryptPayloadKey())
            {
                using (DecryptViewKey())
                {
                    Cryptography.ECC.ECPoint S = opReturnPubKey * this.ViewPrivKey;
                    byte[] d = Crypto.Default.Hash256(S.EncodePoint(true));

                    BigInteger e = ((new BigInteger(d.Reverse().Concat(new byte[1]).ToArray())) + (new BigInteger(this.PayloadPrivKey.Reverse().Concat(new byte[1]).ToArray()))).Mod(Cryptography.ECC.ECCurve.Secp256r1.N);
                    byte[] e_bytes = e.ToByteArray().Reverse().ToArray();
                    byte[] e_final = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    if (e_bytes.Length != 32)
                    {
                        for (int i = 0; i < 32; i ++)
                        {
                            e_final[31 - i] = e_bytes[e_bytes.Length - i - 1];
                        }
                    }
                    else
                    {
                        e_final = e.ToByteArray().Reverse().ToArray();
                    }
                    Cryptography.ECC.ECPoint E = Cryptography.ECC.ECCurve.Secp256r1.G * e_final;

                    Cryptography.ECC.ECPoint D = Cryptography.ECC.ECCurve.Secp256r1.G * d;

                    Cryptography.ECC.ECPoint PPK = Cryptography.ECC.ECCurve.Secp256r1.G * PayloadPrivKey;
                    Cryptography.ECC.ECPoint E1 = D + PayloadPubKey;

                    UInt160 pubKeyHash = E.EncodePoint(true).ToScriptHash();

                    return new KeyPair(e_final);
                }
            }
        }
    }
}
