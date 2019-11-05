using System;
using System.IO;
using System.Linq;
using Pure.Core;
using Pure.Cryptography.ECC;
using Pure.IO;
using Pure.SmartContract;
using Pure.VM;

namespace Pure.Wallets
{
    class AnonymousVerificationContract : Contract, IEquatable<AnonymousVerificationContract>, ISerializable
    {
        public UInt160 PublicKeyHash;
        private string _address;

        public string Address
        {
            get
            {
                if (_address == null)
                {
                    _address = Wallet.ToAddress(ScriptHash);
                }
                return _address;
            }
        }

        public int Size => PublicKeyHash.Size + ParameterList.GetVarSize() + Script.GetVarSize();


        public static AnonymousVerificationContract Create(UInt160 publicKeyHash, ContractParameterType[] parameterList, byte[] redeemScript)
        {
            return new AnonymousVerificationContract
            {
                Script = redeemScript,
                ParameterList = parameterList,
                PublicKeyHash = publicKeyHash
            };
        }

        public static AnonymousVerificationContract CreateMultiSigContract(UInt160 publicKeyHash, int m, params ECPoint[] publicKeys)
        {
            return new AnonymousVerificationContract
            {
                Script = CreateMultiSigRedeemScript(m, publicKeys),
                ParameterList = Enumerable.Repeat(ContractParameterType.Signature, m).ToArray(),
                PublicKeyHash = publicKeyHash
            };
        }

        public static AnonymousVerificationContract CreateSignatureContract(ECPoint publicKey)
        {
            return new AnonymousVerificationContract
            {
                Script = CreateSignatureRedeemScript(publicKey),
                ParameterList = new[] { ContractParameterType.Signature },
                PublicKeyHash = publicKey.EncodePoint(true).ToScriptHash(),
            };
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(PublicKeyHash);
            writer.WriteVarBytes(ParameterList.Cast<byte>().ToArray());
            writer.WriteVarBytes(Script);
        }

        public void Deserialize(BinaryReader reader)
        {
            PublicKeyHash = reader.ReadSerializable<UInt160>();
            ParameterList = reader.ReadVarBytes().Select(p => (ContractParameterType)p).ToArray();
            Script = reader.ReadVarBytes();
        }

        public bool Equals(AnonymousVerificationContract other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            return ScriptHash.Equals(other.ScriptHash);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as AnonymousVerificationContract);
        }

        public override int GetHashCode()
        {
            return ScriptHash.GetHashCode();
        }

        
    }
}
