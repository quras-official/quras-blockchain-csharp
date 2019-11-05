using Pure.Cryptography;
using Pure.IO;
using Pure.IO.Json;
using Pure.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pure.Core
{
    public class Block : BlockBase, IInventory, IEquatable<Block>
    {
        public Transaction[] Transactions;

        private Header _header = null;

        public Header Header
        {
            get
            {
                if (_header == null)
                {
                    _header = new Header
                    {
                        PrevHash = PrevHash,
                        MerkleRoot = MerkleRoot,
                        Timestamp = Timestamp,
                        Index = Index,
                        ConsensusData = ConsensusData,
                        NextConsensus = NextConsensus,
                        Script = Script
                    };
                }
                return _header;
            }
        }

        InventoryType IInventory.InventoryType => InventoryType.Block;

        public override int Size => base.Size + Transactions.GetVarSize();

        public static Dictionary<UInt256, Fixed8> CalculateNetFee(IEnumerable<Transaction> transactions)
        {
            Dictionary<UInt256, Fixed8> ret = new Dictionary<UInt256, Fixed8>();
            #region Calculate QRG fee
            {
                Transaction[] ats = transactions.Where(p => p.Type == TransactionType.AnonymousContractTransaction).ToArray();

                Transaction[] ars = transactions.Where(p => p.Type == TransactionType.RingConfidentialTransaction).ToArray();

                foreach(var arx in ars)
                {
                    if (arx is RingConfidentialTransaction)
                    {
                        var ringTR = arx as RingConfidentialTransaction;
                        if (ringTR.RingCTSig.Count > 0)
                        {
                            AssetState asset = Blockchain.Default.GetAssetState(ringTR.RingCTSig[0].AssetID);

                            if (ret.ContainsKey(asset.AssetId))
                                ret[asset.AssetId] += asset.AFee;
                            else
                                ret[asset.AssetId] = asset.AFee;
                        }
                    }
                }
                    
                foreach (var atx in ats)
                {
                    if (atx is AnonymousContractTransaction)
                    {
                        var formatedTx = atx as AnonymousContractTransaction;
                        if (formatedTx.Outputs.Length > 0)
                        {
                            AssetState asset = Blockchain.Default.GetAssetState(formatedTx.Outputs[0].AssetId);

                            if (ret.ContainsKey(asset.AssetId))
                                ret[asset.AssetId] += asset.AFee;
                            else
                                ret[asset.AssetId] = asset.AFee;
                        }
                        else
                        {
                            AssetState asset = Blockchain.Default.GetAssetState(((AnonymousContractTransaction)formatedTx).Asset_ID(0));
                            if (ret.ContainsKey(asset.AssetId))
                                ret[asset.AssetId] += asset.AFee;
                            else
                                ret[asset.AssetId] = asset.AFee;
                        }
                    }
                }

                Transaction[] invocationTxs = transactions.Where(p => p.Type == TransactionType.InvocationTransaction).ToArray();

                foreach (var tx in invocationTxs)
                {
                    if (ret.ContainsKey(Blockchain.UtilityToken.Hash))
                    {
                        ret[Blockchain.UtilityToken.Hash] += ((InvocationTransaction) tx).Gas;
                    }
                    else
                    {
                        ret[Blockchain.UtilityToken.Hash] = ((InvocationTransaction)tx).Gas;
                    }
                }

                Transaction[] issueTxs = transactions.Where(p => p.Type == TransactionType.IssueTransaction).ToArray();

                foreach (var tx in issueTxs)
                {
                    IssueTransaction tempTx = (IssueTransaction)tx;
                    if (ret.ContainsKey(Blockchain.UtilityToken.Hash))
                    {
                        ret[Blockchain.UtilityToken.Hash] += tempTx.NetworkFee + tempTx.SystemFee;
                    }
                    else
                    {
                        ret[Blockchain.UtilityToken.Hash] = tempTx.NetworkFee + tempTx.SystemFee;
                    }
                }

                Transaction[] feeTxs = transactions.Where(p => p.Type != TransactionType.MinerTransaction 
                                                            && p.Type != TransactionType.ClaimTransaction 
                                                            && p.Type != TransactionType.AnonymousContractTransaction 
                                                            && p.Type != TransactionType.RingConfidentialTransaction 
                                                            && p.Type != TransactionType.InvocationTransaction 
                                                            && p.Type != TransactionType.IssueTransaction).ToArray();

                foreach (var tx in feeTxs)
                {
                    Dictionary<UInt256, Fixed8> fee = new Dictionary<UInt256, Fixed8>();

                    foreach (var txOut in tx.Outputs)
                    {
                        if (!ret.ContainsKey(txOut.AssetId))
                        {
                            ret[txOut.AssetId] = txOut.Fee;
                        }
                        else
                        {
                            ret[txOut.AssetId] += txOut.Fee;
                        }
                    }
                }
            }

            #endregion

            Dictionary<UInt256, Fixed8> retFees = new Dictionary<UInt256, Fixed8>();
            foreach (var key in ret.Keys)
            {
                if (ret[key] > Fixed8.Zero)
                {
                    retFees[key] = ret[key];
                }
            }

            return retFees;
        }

        public override void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);
            Transactions = new Transaction[reader.ReadVarInt(0x10000)];
            if (Transactions.Length == 0) throw new FormatException();
            for (int i = 0; i < Transactions.Length; i++)
            {
                Transactions[i] = Transaction.DeserializeFrom(reader);
            }
            if (MerkleTree.ComputeRoot(Transactions.Select(p => p.Hash).ToArray()) != MerkleRoot)
                throw new FormatException();
        }

        public bool Equals(Block other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            return Hash.Equals(other.Hash);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Block);
        }

        public static Block FromTrimmedData(byte[] data, int index, Func<UInt256, Transaction> txSelector)
        {
            Block block = new Block();
            using (MemoryStream ms = new MemoryStream(data, index, data.Length - index, false))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                ((IVerifiable)block).DeserializeUnsigned(reader);
                reader.ReadByte(); block.Script = reader.ReadSerializable<Witness>();
                block.Transactions = new Transaction[reader.ReadVarInt(0x10000000)];
                for (int i = 0; i < block.Transactions.Length; i++)
                {
                    block.Transactions[i] = txSelector(reader.ReadSerializable<UInt256>());
                }
            }
            return block;
        }

        public override int GetHashCode()
        {
            return Hash.GetHashCode();
        }

        public void RebuildMerkleRoot()
        {
            MerkleRoot = MerkleTree.ComputeRoot(Transactions.Select(p => p.Hash).ToArray());
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write(Transactions);
        }

        public override JObject ToJson()
        {
            JObject json = base.ToJson();
            json["tx"] = Transactions.Select(p => p.ToJson()).ToArray();
            return json;
        }

        public byte[] Trim()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                ((IVerifiable)this).SerializeUnsigned(writer);
                writer.Write((byte)1); writer.Write(Script);
                writer.Write(Transactions.Select(p => p.Hash).ToArray());
                writer.Flush();
                return ms.ToArray();
            }
        }

        public bool Verify(bool completely)
        {
            if (!Verify()) return false;
            if (Transactions[0].Type != TransactionType.MinerTransaction || Transactions.Skip(1).Any(p => p.Type == TransactionType.MinerTransaction))
                return false;
            if (completely)
            {
                if (NextConsensus != Blockchain.GetConsensusAddress(Blockchain.Default.GetValidators(Transactions).ToArray()))
                    return false;
                foreach (Transaction tx in Transactions)
                    if (!tx.Verify(Transactions.Where(p => !p.Hash.Equals(tx.Hash)))) return false;
                Transaction tx_gen = Transactions.FirstOrDefault(p => p.Type == TransactionType.MinerTransaction);

                Dictionary<UInt256, Fixed8> assetFee = CalculateNetFee(Transactions);

                foreach (var key in assetFee.Keys)
                {
                    AssetState asset = Blockchain.Default.GetAssetState(key);

                    if (asset.AssetId == Blockchain.GoverningToken.Hash)
                    {
                        if (tx_gen?.Outputs.Where(p => p.AssetId == Blockchain.GoverningToken.Hash).Sum(p => p.Value) != assetFee[key]) return false;
                    }
                    else if (asset.AssetId == Blockchain.UtilityToken.Hash)
                    {
                        if (tx_gen?.Outputs.Where(p => p.AssetId == Blockchain.UtilityToken.Hash).Sum(p => p.Value) != assetFee.Where(p => p.Key != Blockchain.GoverningToken.Hash).Sum(p => p.Value)) return false;
                    }
                    else
                    {
                        Fixed8 consensusFee = Fixed8.Zero;
                        Fixed8 assetOwnerFee = Fixed8.Zero;

                        if (assetFee[key] <= Fixed8.Satoshi * 10000000)
                        {
                            consensusFee = assetFee[key] * 8 / 10;
                            assetOwnerFee = assetFee[key] * 2 / 10;
                        }
                        else if (assetFee[key] < Fixed8.FromDecimal(1))
                        {
                            consensusFee = assetFee[key] * 75 / 100;
                            assetOwnerFee = assetFee[key] * 25 / 100;
                        }
                        else if (assetFee[key] < Fixed8.FromDecimal(5))
                        {
                            consensusFee = assetFee[key] * 7 / 10;
                            assetOwnerFee = assetFee[key] * 3 / 10;
                        }
                        else if (assetFee[key] < Fixed8.FromDecimal(10))
                        {
                            consensusFee = assetFee[key] * 65 / 100;
                            assetOwnerFee = assetFee[key] * 35 / 100;
                        }
                        else
                        {
                            consensusFee = assetFee[key] * 6 / 10;
                            assetOwnerFee = assetFee[key] * 4 / 10;
                        }

                        if (tx_gen?.Outputs.Where(p => p.ScriptHash == asset.FeeAddress).Sum(p => p.Value) != assetOwnerFee) return false;
                    }
                }
                // if (tx_gen?.Outputs.Sum(p => p.Value) != CalculateNetFee(Transactions)) return false;
            }
            return true;
        }
    }
}
