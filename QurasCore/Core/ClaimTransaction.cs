﻿using Quras.IO;
using Quras.IO.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Quras.Core
{
    public class ClaimTransaction : Transaction
    {
        public CoinReference[] Claims;

        public override Fixed8 NetworkFee => Fixed8.Zero;

        public override int Size => base.Size + Claims.GetVarSize();

        public ClaimTransaction()
            : base(TransactionType.ClaimTransaction)
        {
        }

        protected override void DeserializeExclusiveData(BinaryReader reader)
        {
            if (Version != 0) throw new FormatException();
            Claims = reader.ReadSerializableArray<CoinReference>();
            if (Claims.Length == 0) throw new FormatException();
        }

        public override UInt160[] GetScriptHashesForVerifying()
        {
            HashSet<UInt160> hashes = new HashSet<UInt160>(base.GetScriptHashesForVerifying());
            foreach (var group in Claims.GroupBy(p => p.PrevHash))
            {
                Transaction tx = Blockchain.Default.GetTransaction(group.Key);
                if (tx == null) throw new InvalidOperationException();
                foreach (CoinReference claim in group)
                {
                    if (tx.Outputs.Length <= claim.PrevIndex) throw new InvalidOperationException();
                    hashes.Add(tx.Outputs[claim.PrevIndex].ScriptHash);
                }
            }
            return hashes.OrderBy(p => p).ToArray();
        }

        protected override void SerializeExclusiveData(BinaryWriter writer)
        {
            writer.Write(Claims);
        }

        public override JObject ToJson()
        {
            JObject json = base.ToJson();
            json["claims"] = new JArray(Claims.Select(p => p.ToJson()).ToArray());
            return json;
        }

        public override JObject ToJsonString()
        {
            JObject json = base.ToJsonString();
            json["claims"] = new JArray(Claims.Select(p => p.ToJsonString()).ToArray());
            return json;
        }

        public new static ClaimTransaction FromJsonString(JObject json)
        {
            ClaimTransaction ctx = (ClaimTransaction)Transaction.FromJsonString(json);
            ctx.Claims = ((JArray)json["claims"]).Select(p => CoinReference.FromJsonString(p)).ToArray();
            return ctx;
        }

        public override void FromJsonObject(JObject json)
        {
            base.FromJsonObject(json);
            Claims = ((JArray)json["claims"]).Select(p => CoinReference.FromJsonString(p)).ToArray();
        }

        public override bool Verify(IEnumerable<Transaction> mempool)
        {
            if (!base.Verify(mempool)) return false;
            if (Claims.Length != Claims.Distinct().Count())
                return false;
            if (mempool.OfType<ClaimTransaction>().Where(p => p != this).SelectMany(p => p.Claims).Intersect(Claims).Count() > 0)
                return false;
            if (is_consensus_mempool == false)
            {
                foreach (CoinReference claim in Claims)
                {
                    if (!(Blockchain.Default.GetTransaction(claim.PrevHash) is Transaction tx
                        && tx != null
                        && tx.Outputs.Count() > claim.PrevIndex
                        && tx.Outputs[claim.PrevIndex].AssetId == Blockchain.GoverningToken.Hash))
                        return false;
                }
            }
            
            TransactionResult result = GetTransactionResults().FirstOrDefault(p => p.AssetId == Blockchain.UtilityToken.Hash);
            if (result == null || result.Amount > Fixed8.Zero) return false;
            try
            {
                return Blockchain.CalculateBonus(Claims, true) == -result.Amount;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }
    }
}
