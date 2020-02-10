using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Quras.IO;
using Quras.IO.Json;
using Quras.Wallets;

namespace Quras.Core
{
    public class StorageTransaction : Transaction
    {
        public uint EndTimeStamp;
        public uint StorageSize;
        public Fixed8 GuaranteeAmountPerGB;
        public Fixed8 PayAmountPerGB;
        public UInt160 OwnerHash;

        public override int Size => base.Size + sizeof(uint) * 2 + GuaranteeAmountPerGB.Size + PayAmountPerGB.Size + OwnerHash.Size;

        public StorageTransaction()
            : base(TransactionType.StorageTransaction)
        {
        }

        public StorageTransaction(uint endtimeStamp, uint storageSize, Fixed8 guaranteeAmount, Fixed8 payAmount, UInt160 ownerHash)
            : base(TransactionType.StorageTransaction)
        {
            EndTimeStamp = endtimeStamp;
            StorageSize = storageSize;
            GuaranteeAmountPerGB = guaranteeAmount;
            PayAmountPerGB = payAmount;
            OwnerHash = ownerHash;
        }


        public override JObject ToJson()
        {
            JObject json = base.ToJson();
            json["endtimeStamp"] = EndTimeStamp.ToString();
            json["storageSize"] = StorageSize.ToString();
            json["guaranteeAmount"] = GuaranteeAmountPerGB.ToString();
            json["payAmount"] = PayAmountPerGB.ToString();
            json["ownerHash"] = OwnerHash.ToString();
            return json;
        }

        public override bool Verify(IEnumerable<Transaction> mempool)
        {
            try
            {
                //TODO: 
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

            EndTimeStamp = reader.ReadUInt32();
            StorageSize = reader.ReadUInt32();
            GuaranteeAmountPerGB = reader.ReadSerializable<Fixed8>();
            PayAmountPerGB = reader.ReadSerializable<Fixed8>();
            OwnerHash = reader.ReadSerializable<UInt160>();

        }
        protected override void SerializeExclusiveData(BinaryWriter writer)
        {
            writer.Write(EndTimeStamp);
            writer.Write(StorageSize);
            writer.Write(GuaranteeAmountPerGB);
            writer.Write(PayAmountPerGB);
            writer.Write(OwnerHash);
        }
    }
}
