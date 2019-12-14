using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Quras.IO;
using Quras.IO.Json;
using Quras.Wallets;

namespace Quras.Core
{

    public class ApproveDownloadTransaction : Transaction
    {
        public UInt160 approveHash;
        public UInt160 downloadHash;
        public UInt256 dTXHash;
        public bool approveState;
        


        public override int Size => base.Size +  dTXHash.Size + approveHash.Size + sizeof(bool) + downloadHash.Size;

        public ApproveDownloadTransaction()
            : base(TransactionType.ApproveDownloadTransaction)
        {
        }

        public ApproveDownloadTransaction(UInt256 txId, UInt160 addrHash, UInt160 downHash, bool bApprove)
            : base(TransactionType.ApproveDownloadTransaction)
        {
            dTXHash = txId;
            approveHash = addrHash;
            downloadHash = downHash;
            approveState = bApprove;
        }


        public override JObject ToJson()
        {
            JObject json = base.ToJson();
            json["approveHash"] = approveHash.ToString();
            json["downloadHash"] = downloadHash.ToString();
            json["dTXHash"] = dTXHash.ToString();
            json["approveState"] = approveState.ToString();
            return json;
        }

        public override bool Verify(IEnumerable<Transaction> mempool)
        {
            try
            {
                if (Wallet.GetAddressVersion(Wallet.ToAddress(downloadHash)) != Wallet.AddressVersion)
                    return false;

                if (Wallet.GetAddressVersion(Wallet.ToAddress(approveHash)) != Wallet.AddressVersion)
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
            if (Version != 0) throw new FormatException();

            dTXHash = reader.ReadSerializable<UInt256>();
            approveHash = reader.ReadSerializable<UInt160>();
            downloadHash = reader.ReadSerializable<UInt160>();
            approveState = reader.ReadBoolean();

        }
        protected override void SerializeExclusiveData(BinaryWriter writer)
        {
            writer.Write(dTXHash);
            writer.Write(approveHash);
            writer.Write(downloadHash);
            writer.Write(approveState);
        }
    }
}
