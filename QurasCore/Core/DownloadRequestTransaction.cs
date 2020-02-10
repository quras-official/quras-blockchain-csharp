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
    public struct DownFileInformation
    {
        public string FileName;
        public string FileDescription;
        public string FileURL;
        public Fixed8 PayAmount;
        public UInt160 uploadHash;
        public UInt256 txHash;
        public List<ECPoint> FileVerifiers;
    }

    public struct HttpDownFileInformation
    {
        public string txid;
        public string file_name;
        public string file_description;
        public string file_url; 
        public double pay_amount;
        public string upload_address;
        public string file_verifiers;
        public long time;
        public long block_number;
        public string hash;
    }

    public class DownloadRequestTransaction : Transaction
    {
        public string FileName;
        public string FileDescription;
        public string FileURL;
        public Fixed8 PayAmount;
        public UInt160 uploadHash;
        public UInt160 downloadHash;
        public UInt256 txHash;
        public ECPoint[] FileVerifiers;
        public ECPoint RequestPK;
        public int ApproveCount => FileVerifiers.Length;
        


        public override int Size => base.Size + FileName.GetVarSize() + FileDescription.GetVarSize() + FileURL.GetVarSize() +
                                        PayAmount.Size + FileVerifiers.GetVarSize() + uploadHash.Size + downloadHash.Size + txHash.Size + RequestPK.Size;

        public DownloadRequestTransaction()
            : base(TransactionType.DownloadRequestTransaction)
        {
        }

        public DownloadRequestTransaction(UInt256 txId, string fileName, string fileDescription, string fileUrl, Fixed8 payAmount, ECPoint[] fileVerifiers, ECPoint requestKey)
            : base(TransactionType.DownloadRequestTransaction)
        {
            txHash = txId;
            FileName = fileName;
            FileDescription = fileDescription;
            FileURL = fileUrl;
            PayAmount = payAmount;
            for (int i = 0; i < fileVerifiers.Length; i ++)
            {
                FileVerifiers[ApproveCount] = fileVerifiers[i];
            }
            RequestPK = requestKey;
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
            json["downloadHash"] = downloadHash.ToString();
            json["txId"] = txHash.ToString();
            JArray jbVerifier = new JArray();
            for (int i = 0; i < ApproveCount; i++)
                jbVerifier.Add(FileVerifiers[i].ToString());
            json["fileVerifiers"] = jbVerifier;
            json["requestPK"] = RequestPK.ToString();
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

        protected override void DeserializeExclusiveData(BinaryReader reader)
        {
            if (Version != 1) throw new FormatException();

            FileName = reader.ReadString();
            FileDescription = reader.ReadString();
            FileURL = reader.ReadString();
            PayAmount = reader.ReadSerializable<Fixed8>();
            uploadHash = reader.ReadSerializable<UInt160>();
            downloadHash = reader.ReadSerializable<UInt160>();
            txHash = reader.ReadSerializable<UInt256>();
            FileVerifiers = reader.ReadSerializableArray<ECPoint>();
            RequestPK = reader.ReadSerializable<ECPoint>();

        }
        protected override void SerializeExclusiveData(BinaryWriter writer)
        {
            writer.Write(FileName);
            writer.Write(FileDescription);
            writer.Write(FileURL);
            writer.Write(PayAmount);
            writer.Write(uploadHash);
            writer.Write(downloadHash);
            writer.Write(txHash);
            writer.Write(FileVerifiers);
            writer.Write(RequestPK);
        }
    }
}
