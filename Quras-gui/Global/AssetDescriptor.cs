using Pure.Core;
using Pure.SmartContract;
using Pure.VM;
using Pure;
using System;
using System.Linq;
using System.Numerics;

namespace Quras_gui.Global
{
    internal class AssetDescriptor
    {
        public UIntBase AssetId;
        public string AssetName;
        public byte Precision;

        public AssetDescriptor(UInt160 asset_id)
        {
            byte[] script;
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitAppCall(asset_id, "decimals");
                sb.EmitAppCall(asset_id, "name");
                script = sb.ToArray();
            }
            ApplicationEngine engine = ApplicationEngine.Run(script);
            if (engine.State.HasFlag(VMState.FAULT)) throw new ArgumentException();
            this.AssetId = asset_id;
            this.AssetName = engine.EvaluationStack.Pop().GetString();
            this.Precision = (byte)engine.EvaluationStack.Pop().GetBigInteger();
        }

        public AssetDescriptor(AssetState state)
        {
            this.AssetId = state.AssetId;
            this.AssetName = state.GetName();
            this.Precision = state.Precision;
        }

        public Global.BigDecimal GetAvailable()
        {
            if (AssetId is UInt256 asset_id)
            {
                return new Global.BigDecimal(Constant.CurrentWallet.GetAvailable(asset_id).GetData(), 8);
            }
            else
            {
                byte[] script;
                using (ScriptBuilder sb = new ScriptBuilder())
                {
                    foreach (UInt160 account in Constant.CurrentWallet.GetContracts().Select(p => p.ScriptHash))
                        sb.EmitAppCall((UInt160)AssetId, "balanceOf", account);
                    sb.Emit(OpCode.DEPTH, OpCode.PACK);
                    script = sb.ToArray();
                }
                ApplicationEngine engine = ApplicationEngine.Run(script);
                BigInteger amount = engine.EvaluationStack.Pop().GetArray().Aggregate(BigInteger.Zero, (x, y) => x + y.GetBigInteger());
                return new Global.BigDecimal(amount, Precision);
            }
        }

        public override string ToString()
        {
            return AssetName;
        }
    }
}
