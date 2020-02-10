using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Quras.IO;
using Quras.IO.Json;
using Quras.Wallets;
using Quras.Cryptography.ECC;

namespace Quras.Core
{
    public class ApproveKeyTransaction : Transaction
    {
        public UInt256 dTXHash;

        public ECPoint VerifyScript;
        public ECPoint InPK;
        public ECPoint EncKeyScript;


        public override int Size => base.Size + dTXHash.Size + EncKeyScript.Size + InPK.Size + VerifyScript.Size;

        public ApproveKeyTransaction()
            : base(TransactionType.ApproveKeyTransaction)
        {
        }

        public override JObject ToJson()
        {
            JObject json = base.ToJson();
            json["dTXHash"] = dTXHash.ToString();
            json["encKeyScript"] = EncKeyScript.ToString();
            json["inPk"] = InPK.ToString();
            json["verifyScript"] = VerifyScript.ToString();
            return json;
        }

        public override bool Verify(IEnumerable<Transaction> mempool)
        {
            return this.VerifyScripts();
        }

        protected override void DeserializeExclusiveData(BinaryReader reader)
        {
            if (Version != 1) throw new FormatException();

            dTXHash = reader.ReadSerializable<UInt256>();
            VerifyScript = reader.ReadSerializable<ECPoint>();
            InPK = reader.ReadSerializable<ECPoint>();
            EncKeyScript = reader.ReadSerializable<ECPoint>();
        }
        protected override void SerializeExclusiveData(BinaryWriter writer)
        {
            writer.Write(dTXHash);
            writer.Write(VerifyScript);
            writer.Write(InPK);
            writer.Write(EncKeyScript);
        }

        public bool EncryptKeyData(ECPoint PubKey, byte[] private_key, ECPoint FileKey)
        {
            EncKeyScript = PubKey * private_key + FileKey;
            return true;
        }

        public ECPoint DecryptKeyData(ECPoint PubKey, byte[] private_key)
        {
            return EncKeyScript - ( PubKey * private_key );
        }

        public void SignScript(UInt160 scriptHash, UInt160 destination)
        {
            UInt256 hashKey1 = new UInt256(Quras.Cryptography.Crypto.Default.Hash256(scriptHash.ToArray()));
            UInt256 hashKey2 = new UInt256(Quras.Cryptography.Crypto.Default.Hash256(destination.ToArray()));
            VerifyScript =  ECCurve.Secp256r1.G * hashKey1.ToArray() + ECCurve.Secp256k1.G * hashKey2.ToArray();
        }
    }
}
