using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pure.IO;
using Pure.IO.Json;
using Pure.Core.Anonoymous;
using Pure.Cryptography;

namespace Pure.Core
{
    public class AnonymousContractTransaction : Transaction
    {
        public List<byte[]> byJoinSplit;
        public UInt256 joinSplitPubKey;
        public byte[] joinSplitSig;

        public Fixed8 FromTSysFee
        {
            get
            {
                return Fixed8.Satoshi * 1000000;
            }
        }

        public Fixed8 FromASysFee
        {
            get
            {
                return Fixed8.Satoshi * 1000000;
            }
        }

        public override Fixed8 SystemFee
        {
            get
            {
                Dictionary<UInt256, Fixed8> fee = new Dictionary<UInt256, Fixed8>();
                Fixed8 assetFee = Fixed8.Zero;

                if (Inputs == null || Inputs?.Length == 0) // A => T OR A => A
                {
                    foreach (var output in Outputs)
                    {
                        if (!fee.ContainsKey(output.AssetId) && output.AssetId != Blockchain.GoverningToken.Hash)
                        {
                            AssetState asset = Blockchain.Default.GetAssetState(output.AssetId);
                            fee[output.AssetId] = asset.AFee;
                        }
                    }

                    for (int i = 0; i < byJoinSplit.Count; i ++)
                    {
                        if (!fee.ContainsKey(Asset_ID(i)) && Asset_ID(i) != Blockchain.GoverningToken.Hash)
                        {
                            AssetState asset = Blockchain.Default.GetAssetState(Asset_ID(i));
                            fee[Asset_ID(i)] = asset.AFee;
                        }
                    }
                }
                else // T => A
                {
                    foreach (var output in Outputs)
                    {
                        if (!fee.ContainsKey(output.AssetId) && output.AssetId != Blockchain.GoverningToken.Hash)
                        {
                            AssetState asset = Blockchain.Default.GetAssetState(output.AssetId);
                            fee[output.AssetId] = asset.Fee;
                        }
                    }

                    for (int i = 0; i < byJoinSplit.Count; i++)
                    {
                        if (!fee.ContainsKey(Asset_ID(i)) && Asset_ID(i) != Blockchain.GoverningToken.Hash)
                        {
                            AssetState asset = Blockchain.Default.GetAssetState(Asset_ID(i));
                            fee[Asset_ID(i)] = asset.Fee;
                        }
                    }
                }

                assetFee = fee.Sum(p => p.Value);
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

                for (int i = 0; i < byJoinSplit.Count; i ++)
                {
                    if (Asset_ID(i) == Blockchain.GoverningToken.Hash)
                    {
                        isContainQRS = true;
                        break;
                    }
                }

                if (isContainQRS == true)
                {
                    if (Inputs == null || Inputs?.Length == 0)
                    {
                        return Blockchain.GoverningToken.A_Fee;
                    }
                    else
                    {
                        return Blockchain.GoverningToken.T_Fee;
                    }
                }
                else
                {
                    return Fixed8.Zero;
                }
            }
        }

        public override int Size => base.Size + byJoinSplit.GetListLength() + joinSplitPubKey.Size + joinSplitSig.GetVarSize();

        private UInt256 _jshash = null;

        public UInt256 JsHash
        {
            get
            {
                if (_jshash == null)
                {
                    AnonymousContractTransaction tx = new AnonymousContractTransaction();
                    tx = this;
                    byte[] temp_joinsplitsig = { 0 };
                    byte[] raw_joinsplitsig = tx.joinSplitSig;
                    tx.joinSplitSig = temp_joinsplitsig;

                    _jshash = new UInt256(Crypto.Default.Hash256(tx.GetHashData()));
                    tx.joinSplitSig = raw_joinsplitsig;
                }
                return _jshash;
            }
        }

        public Fixed8 vPub_Old(int index)
        {

            byte[] byOld = new byte[8];
            for (int i = 0; i < 8; i ++)
            {
                byOld[i] = byJoinSplit[index][i];
            }

            long lOld = BitConverter.ToInt64(byOld, 0);
            return new Fixed8(lOld);
        }

        public Fixed8 vPub_New(int index)
        {
            byte[] byNew = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                byNew[i] = byJoinSplit[index][i + 8];
            }

            long lNew = BitConverter.ToInt64(byNew, 0);
            return new Fixed8(lNew);
        }

        public UInt256 Asset_ID(int index)
        {
            byte[] byAssetID = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                byAssetID[i] = byJoinSplit[index][i + 16];
            }

            return new UInt256(byAssetID);
        }

        public UInt256 Anchor(int index)
        {
            byte[] byAssetID = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                byAssetID[31 - i] = byJoinSplit[index][i + 48];
            }

            return new UInt256(byAssetID);
        }

        public UInt256[] Nullifiers(int index)
        {
            byte[] nullifier1 = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                nullifier1[31 - i] = byJoinSplit[index][i + 80];
            }

            byte[] nullifier2 = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                nullifier2[31 - i] = byJoinSplit[index][i + 112];
            }

            UInt256[] ret = new UInt256[2];
            ret[0] = new UInt256(nullifier1);
            ret[1] = new UInt256(nullifier2);
            return ret;
        }

        public UInt256[] Commitments(int index)
        {
            byte[] commitment1 = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                commitment1[31 - i] = byJoinSplit[index][i + 144];
            }

            byte[] commitment2 = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                commitment2[31 - i] = byJoinSplit[index][i + 176];
            }

            UInt256[] ret = new UInt256[2];
            ret[0] = new UInt256(commitment1);
            ret[1] = new UInt256(commitment2);
            return ret;
        }

        public AnonymousContractTransaction()
            : base(TransactionType.AnonymousContractTransaction)
        {
            byJoinSplit = new List<byte[]>();
            joinSplitSig = new byte[64];
        }

        public override JObject ToJson()
        {
            JObject json = base.ToJson();
            json["byJoinSplit"] = byJoinSplit.ToHexString();
            json["joinsplitPubkey"] = joinSplitPubKey.ToString();
            json["joinsplitSig"] = joinSplitSig.ToHexString();

            return json;
        }

        protected override void DeserializeExclusiveData(BinaryReader reader)
        {
            int nJoinSplitCount = reader.ReadInt32();

            for (int i = 0; i < nJoinSplitCount; i ++)
            {
                byJoinSplit.Add(reader.ReadVarBytes());
            }

            joinSplitPubKey = reader.ReadSerializable<UInt256>();
            joinSplitSig = reader.ReadVarBytes();
        }

        protected override void SerializeExclusiveData(BinaryWriter writer)
        {
            int nJoinSplitCount = byJoinSplit.Count;
            writer.Write(nJoinSplitCount);

            for (int i = 0; i < nJoinSplitCount; i ++)
            {
                writer.WriteVarBytes(byJoinSplit[i]);
            }
            
            writer.Write(joinSplitPubKey);
            writer.WriteVarBytes(joinSplitSig);
        }

        public override bool Verify(IEnumerable<Transaction> mempool)
        {
            // Check Token Type
            for (int i = 0; i < byJoinSplit.Count; i++)
            {
                UInt256 AssetID = Asset_ID(i);
                AssetState asset = Blockchain.Default.GetAssetState(AssetID);

                if (asset.AssetType == AssetType.TransparentToken)
                {
                    return false;
                }
            }

            // Check double spend about tranparent input
            for (int i = 1; i < Inputs.Length; i++)
                for (int j = 0; j < i; j++)
                    if (Inputs[i].PrevHash == Inputs[j].PrevHash && Inputs[i].PrevIndex == Inputs[j].PrevIndex)
                        return false;

            if (mempool.Where(p => p != this).SelectMany(p => p.Inputs).Intersect(Inputs).Count() > 0)
                return false;

            if (Blockchain.Default.IsDoubleSpend(this))
                return false;

            if (Blockchain.Default.IsDoubleNullifier(this))
                return false;

            Fixed8 assetFee = Fixed8.Zero;
            Fixed8 qrsAssetFee = Fixed8.Zero;

            // Check output format
            foreach (var group in Outputs.GroupBy(p => p.AssetId))
            {
                AssetState asset = Blockchain.Default.GetAssetState(group.Key);
                if (asset == null) return false;
                if (asset.Expiration <= Blockchain.Default.Height + 1 && asset.AssetType != AssetType.GoverningToken && asset.AssetType != AssetType.UtilityToken)
                    return false;
                foreach (TransactionOutput output in group)
                    if (output.Value.GetData() % (long)Math.Pow(10, 8 - asset.Precision) != 0)
                        return false;
            }

            TransactionResult[] results_transparent = GetTransactionResults()?.ToArray();
            List<TransactionResult> results_anonymous = new List<TransactionResult>();

            for (int i = 0; i < this.byJoinSplit.Count; i++)
            {
                bool isAdded = false;
                for (int j = 0; j < results_anonymous.Count; j++)
                {
                    if (results_anonymous[j].AssetId == this.Asset_ID(i))
                    {
                        results_anonymous[j].Amount -= this.vPub_Old(i);
                        results_anonymous[j].Amount += this.vPub_New(i);
                        isAdded = true;
                        break;
                    }
                }

                if (isAdded == false)
                {
                    TransactionResult anonyItem = new TransactionResult();
                    anonyItem.AssetId = this.Asset_ID(i);
                    anonyItem.Amount = this.vPub_New(i) - this.vPub_Old(i);
                    results_anonymous.Add(anonyItem);
                }
                AssetState asset = Blockchain.Default.GetAssetState(this.Asset_ID(i));
            }

            TransactionResult[] results = results_anonymous.Select(p => new
            {
                AssetId = p.AssetId,
                Value = p.Amount
            }).Concat(results_transparent.Select(p => new
            {
                AssetId = p.AssetId,
                Value = p.Amount
            })).GroupBy(p => p.AssetId, (k, g) => new TransactionResult
            {
                AssetId = k,
                Amount = g.Sum(p => p.Value)
            }).Where(p => p.Amount != Fixed8.Zero)?.ToArray();

            if (results == null) return false;
            TransactionResult[] results_destroy_temp = results.Where(p => p.Amount > Fixed8.Zero).ToArray();
           
            TransactionResult[] results_destroy = results_destroy_temp.Where(p => p.Amount > Fixed8.Zero).ToArray();

            Fixed8 result_qrs_fee = Fixed8.Zero;
            Fixed8 result_qrg_fee = Fixed8.Zero;
            Fixed8 result_other_fee = Fixed8.Zero;
            for (int i = 0; i < results_destroy.Length; i++)
            {
                if (results_destroy[i].AssetId == Blockchain.GoverningToken.Hash)
                {
                    result_qrs_fee = results_destroy[i].Amount;
                }
                else if (results_destroy[i].AssetId == Blockchain.UtilityToken.Hash)
                {
                    result_qrg_fee = results_destroy[i].Amount;
                }
                else
                {
                    result_other_fee = results_destroy[i].Amount;
                }
            }

            if (result_other_fee > Fixed8.Zero || (SystemFee > result_qrg_fee && assetFee > Fixed8.Zero))
            {
                return false;
            }

            TransactionResult[] results_issue = results.Where(p => p.Amount < Fixed8.Zero).ToArray();
            switch (Type)
            {
                case TransactionType.MinerTransaction:
                case TransactionType.ClaimTransaction:
                    if (results_issue.Any(p => p.AssetId != Blockchain.UtilityToken.Hash))
                        return false;
                    break;
                case TransactionType.IssueTransaction:
                    if (results_issue.Any(p => p.AssetId == Blockchain.UtilityToken.Hash))
                        return false;
                    break;
                default:
                    if (results_issue.Length > 0)
                        return false;
                    break;
            }
            if (Attributes.Count(p => p.Usage == TransactionAttributeUsage.ECDH02 || p.Usage == TransactionAttributeUsage.ECDH03) > 1)
                return false;

            if (!Sodium.PublicKeyAuth.VerifyDetached(joinSplitSig, JsHash.ToArray(), joinSplitPubKey.ToArray()))
            {
                return false;
            } 

            for (int i = 0; i < byJoinSplit.Count; i ++)
            {
                if (!SnarkDllApi.Snark_JSVerify(byJoinSplit[i], joinSplitPubKey.ToArray()))
                {
                    return false;
                }
            }

            if (Inputs.Length > 0)
            {
                return this.VerifyScripts();
            }
            else
            {
                return true;
            }
        }
    }
}
