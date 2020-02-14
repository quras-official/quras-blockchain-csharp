using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Quras.IO;
using Quras.IO.Json;
using Quras.Wallets;

namespace Quras.Core
{
    public class UploadRequestTransaction : Transaction
    {
        public string FileName;
        public string FileDescription;
        public string FileURL;
        public Fixed8 PayAmount;
        public UInt160 uploadHash;
        public UInt160[] FileVerifiers;
        public int ApproveCount => FileVerifiers.Length;
        


        public override int Size => base.Size + FileName.GetVarSize() + FileDescription.GetVarSize() + FileURL.GetVarSize() + PayAmount.Size + FileVerifiers.GetVarSize() + uploadHash.Size;

        public UploadRequestTransaction()
            : base(TransactionType.UploadRequestTransaction)
        {
        }

        public UploadRequestTransaction(string fileName, string fileDescription, string fileUrl, Fixed8 payAmount, UInt160[] fileVerifiers)
            : base(TransactionType.UploadRequestTransaction)
        {
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
            return json;
        }

        public override bool Verify(IEnumerable<Transaction> mempool)
        {
            try
            {
                if (FileName == "" || FileDescription == "" || FileURL == "" || PayAmount <= Fixed8.Zero || ApproveCount == 0)
                    return false;
                for (int i = 0; i < ApproveCount; i ++)
                {
                    if (Wallet.GetAddressVersion(Wallet.ToAddress(FileVerifiers[i])) != Wallet.AddressVersion)
                        return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
            return this.VerifyScripts();
        }

        protected override void DeserializeExclusiveData(BinaryReader reader)
        {
            if (Version != 1) throw new FormatException();

            FileName = reader.ReadString();
            FileDescription = reader.ReadString();
            FileURL = reader.ReadString();
            PayAmount = reader.ReadSerializable<Fixed8>();
            uploadHash = reader.ReadSerializable<UInt160>();
            FileVerifiers = reader.ReadSerializableArray<UInt160>();

        }
        protected override void SerializeExclusiveData(BinaryWriter writer)
        {
            writer.Write(FileName);
            writer.Write(FileDescription);
            writer.Write(FileURL);
            writer.Write(PayAmount);
            writer.Write(uploadHash);
            writer.Write(FileVerifiers);
        }
    }
}
