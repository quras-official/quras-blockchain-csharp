using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Quras.IO;
using Quras.IO.Json;

namespace Quras.Core
{
    public class PayUploadTransaction : Transaction
    {
        public UInt256 uTXHash;
        public UInt64 UploadByteSize;
        public PayUploadTransaction()
            : base(TransactionType.PayUploadTransaction)
        {
        }

        public override Fixed8 SystemFee
        {
            get
            {
                Dictionary<UInt256, Fixed8> fee = new Dictionary<UInt256, Fixed8>();
                bool isQrgFee = false;
                foreach (var output in Outputs)
                {
                    if (!fee.ContainsKey(output.AssetId) && output.AssetId != Blockchain.GoverningToken.Hash && output.AssetId != Blockchain.UtilityToken.Hash)
                    {
                        AssetState asset = Blockchain.Default.GetAssetState(output.AssetId);
                        fee[output.AssetId] = output.Fee;
                    }

                    if (output.AssetId == Blockchain.UtilityToken.Hash)
                    {
                        if (!References.Values.Select(p => p.ScriptHash).Contains(output.ScriptHash))
                        {
                            isQrgFee = true;
                        }
                    }
                }

                Fixed8 assetFee = fee.Sum(p => p.Value);

                if (isQrgFee == true)
                {
                    assetFee += Blockchain.UtilityToken.T_Fee;
                }

                return assetFee;
            }
        }

        public override Fixed8 QrsSystemFee
        {
            get
            {
                bool isContainQRS = false;
                foreach (var output in Outputs)
                {
                    if (output.AssetId == Blockchain.GoverningToken.Hash)
                    {
                        isContainQRS = true;
                        break;
                    }
                }

                if (isContainQRS == true)
                {
                    return Blockchain.GoverningToken.T_Fee;
                }
                else
                {
                    return Fixed8.Zero;
                }
            }
        }
        public override JObject ToJson()
        {
            JObject json = base.ToJson();
            json["dTXHash"] = uTXHash.ToString();
            json["uploadByteSize"] = UploadByteSize.ToString();
            return json;
        }

        protected override void DeserializeExclusiveData(BinaryReader reader)
        {
            if (Version != 0) throw new FormatException();
            uTXHash = reader.ReadSerializable<UInt256>();
            UploadByteSize = (UInt64)reader.ReadInt64();
        }

        protected override void SerializeExclusiveData(BinaryWriter writer)
        {
            writer.Write(uTXHash);
            writer.Write(UploadByteSize);
        }
    }
}
