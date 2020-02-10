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
    public class UploadRequestTransaction : Transaction
    {
        public uint DurationDays;
        public string FileName;
        public string FileDescription;
        public string FileURL;
        public Fixed8 PayAmount;
        public UInt160 uploadHash;
        public ECPoint[] FileVerifiers;
        public int ApproveCount => FileVerifiers.Length;
        public byte[] EncryptedKey = new byte [0x20];
        
        public override int Size => base.Size + sizeof(uint) + FileName.GetVarSize() + FileDescription.GetVarSize() + FileURL.GetVarSize() + PayAmount.Size + FileVerifiers.GetVarSize() + uploadHash.Size + EncryptedKey.GetVarSize();

        public UploadRequestTransaction()
            : base(TransactionType.UploadRequestTransaction)
        {
        }

        public UploadRequestTransaction(string fileName, string fileDescription, string fileUrl, Fixed8 payAmount, ECPoint[] fileVerifiers, uint durationDays = 0)
            : base(TransactionType.UploadRequestTransaction)
        {
            DurationDays = durationDays;
            FileName = fileName;
            FileDescription = fileDescription;
            FileURL = fileUrl;
            PayAmount = payAmount;
            for (int i = 0; i < fileVerifiers.Length; i ++)
            {
                FileVerifiers[ApproveCount] = fileVerifiers[i];
            }
        }


        public override JObject ToJson()
        {
            JObject json = base.ToJson();
            json["durationDays"] = DurationDays.ToString();
            json["fileName"] = FileName;
            json["fileDescription"] = FileDescription;
            json["fileUrl"] = FileURL;
            json["payAmount"] = PayAmount.ToString();
            json["approveCount"] = ApproveCount.ToString();
            json["uploadHash"] = uploadHash.ToString();
            JArray jbVerifier = new JArray();
            for (int i = 0; i < ApproveCount; i++)
                jbVerifier.Add(FileVerifiers[i].ToString());
            json["fileVerifiers"] = jbVerifier;
            json["encryptedKey"] = EncryptedKey.ToString();
            return json;
        }

        public override bool Verify(IEnumerable<Transaction> mempool)
        {
            try
            {
                if (FileName == "" || FileDescription == "" || FileURL == "" || PayAmount <= Fixed8.Zero || ApproveCount == 0)
                    return false;
            }
            catch(Exception ex)
            {
                return false;
            }
            return this.VerifyScripts();
        }

        public void SetEncryptKey(byte[] ownerKey, byte[] unEncryptedKey)
        {
            byte[] ownerKeyBytes = ownerKey;
            
            for (int i = 0; i < ownerKeyBytes.Length && i < unEncryptedKey.Length; i ++)
            {
                EncryptedKey[i] = (byte)(unEncryptedKey[i] ^ ownerKeyBytes[i]);
            }
        }

        public static ECPoint Encrypt_Verifier(UInt256 encKey, UInt256 verifierHash)
        {
            return ECCurve.Secp256r1.G * encKey.ToArray() + ECCurve.Secp256k1.G * verifierHash.ToArray();
        }

        public byte[] DecryptKey(byte[] ownerKeyBytes)
        {
            byte[] fileKeyBytes = new byte [EncryptedKey.Length];

            for (int i = 0; i < ownerKeyBytes.Length && i < fileKeyBytes.Length; i++)
            {
                fileKeyBytes[i] = (byte)(EncryptedKey[i] ^ ownerKeyBytes[i]);
            }
            return fileKeyBytes;
        }

        protected override void DeserializeExclusiveData(BinaryReader reader)
        {
            if (Version != 1) throw new FormatException();

            DurationDays = reader.ReadUInt32();
            FileName = reader.ReadString();
            FileDescription = reader.ReadString();
            FileURL = reader.ReadString();
            PayAmount = reader.ReadSerializable<Fixed8>();
            uploadHash = reader.ReadSerializable<UInt160>();
            FileVerifiers = reader.ReadSerializableArray<ECPoint>();
            EncryptedKey = reader.ReadBytes(EncryptedKey.GetVarSize());

        }
        protected override void SerializeExclusiveData(BinaryWriter writer)
        {
            writer.Write(DurationDays);
            writer.Write(FileName);
            writer.Write(FileDescription);
            writer.Write(FileURL);
            writer.Write(PayAmount);
            writer.Write(uploadHash);
            writer.Write(FileVerifiers);
            writer.Write(EncryptedKey);
        }
    }
}
