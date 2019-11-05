using Pure.Core;
using Pure.Cryptography.ECC;
using Pure.IO;
using Pure.SmartContract;
using Pure.VM;
using PureCore.Wallets.AnonymousKey.Key;
using System;
using System.IO;
using System.Linq;

namespace Pure.Wallets
{
    public class VerificationContract : Contract, IEquatable<VerificationContract>, ISerializable
    {
        public UInt160 PublicKeyHash;
        public KeyType ContractType;
        public PaymentAddress paymentAddress;
        public StealthKey.StealthPubKeys stealthKey;

        private string _address;

        public string Address
        {
            get
            {
                if (_address == null)
                {
                    if (ContractType == KeyType.Transparent)
                    {
                        _address = Wallet.ToAddress(ScriptHash);
                    }
                    else if (ContractType == KeyType.Anonymous)
                    {
                        _address = Wallet.ToAnonymousAddress(paymentAddress);
                      
                    }
                    else if (ContractType == KeyType.Stealth)
                    {
                        _address = Wallet.ToStealthAddress(stealthKey);
                    }
                }
                return _address;
            }
        }

        public int Size => ContractType == KeyType.Transparent ?
            PublicKeyHash.Size + ParameterList.GetVarSize() + Script.GetVarSize() : 
            PublicKeyHash.Size + ParameterList.GetVarSize() + Script.GetVarSize() + paymentAddress.GetVarSize();

        public static VerificationContract Create(UInt160 publicKeyHash, ContractParameterType[] parameterList, byte[] redeemScript, KeyType keyType = KeyType.Transparent, PaymentAddress pAddr = null)
        {
            if (keyType == KeyType.Anonymous)
            {
                return new VerificationContract
                {
                    ContractType = keyType,
                    Script = redeemScript,
                    ParameterList = parameterList,
                    PublicKeyHash = publicKeyHash,
                    paymentAddress = pAddr
                };
            }
            else
            {
                return new VerificationContract
                {
                    ContractType = keyType,
                    Script = redeemScript,
                    ParameterList = parameterList,
                    PublicKeyHash = publicKeyHash
                };
            }
            
        }

        public static VerificationContract CreateMultiSigContract(UInt160 publicKeyHash, int m, params ECPoint[] publicKeys)
        {
            return new VerificationContract
            {
                ContractType = KeyType.Transparent,
                Script = CreateMultiSigRedeemScript(m, publicKeys),
                ParameterList = Enumerable.Repeat(ContractParameterType.Signature, m).ToArray(),
                PublicKeyHash = publicKeyHash
            };
        }

        public static VerificationContract CreateSignatureContract(ECPoint publicKey)
        {
            return new VerificationContract
            {
                ContractType = KeyType.Transparent,
                Script = CreateSignatureRedeemScript(publicKey),
                ParameterList = new[] { ContractParameterType.Signature },
                PublicKeyHash = publicKey.EncodePoint(true).ToScriptHash(),
            };
        }

        public static VerificationContract CreateRingSignatureContract(StealthKey.StealthPubKeys account)
        {
            return new VerificationContract
            {
                ContractType = KeyType.Stealth,
                Script = CreateRingSignatureRedeemScript(account.PayloadPubKey, account.ViewPubKey),
                ParameterList = new[] { ContractParameterType.Signature },
                PublicKeyHash = account.GetPublicKeyHash(),
                stealthKey = account
            };
        }

        public static VerificationContract CreateSignatureAnonymousContract(ECPoint publicKey, PaymentAddress pAddr)
        {
            return new VerificationContract
            {
                ContractType = KeyType.Anonymous,
                Script = pAddr.ToArray(),
                ParameterList = new[] { ContractParameterType.Signature },
                PublicKeyHash = publicKey.EncodePoint(true).ToScriptHash(),
                paymentAddress = pAddr
            };
        }

        public void Deserialize(BinaryReader reader)
        {
            ContractType = (KeyType)reader.ReadVarInt();
            PublicKeyHash = reader.ReadSerializable<UInt160>();
            ParameterList = reader.ReadVarBytes().Select(p => (ContractParameterType)p).ToArray();
            Script = reader.ReadVarBytes();
            if (ContractType == KeyType.Anonymous)
                paymentAddress = reader.ReadSerializable<PaymentAddress>();
            if (ContractType == KeyType.Stealth)
                stealthKey = reader.ReadSerializable<StealthKey.StealthPubKeys>();
        }

        public bool Equals(VerificationContract other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            return ScriptHash.Equals(other.ScriptHash);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VerificationContract);
        }

        public override int GetHashCode()
        {
            return ScriptHash.GetHashCode();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteVarInt((long)ContractType);
            writer.Write(PublicKeyHash);
            writer.WriteVarBytes(ParameterList.Cast<byte>().ToArray());
            writer.WriteVarBytes(Script);
            if (ContractType == KeyType.Anonymous)
            {
                writer.Write(paymentAddress);
            }
            if (ContractType == KeyType.Stealth)
            {
                writer.Write(stealthKey);
            }
        }
    }
}
