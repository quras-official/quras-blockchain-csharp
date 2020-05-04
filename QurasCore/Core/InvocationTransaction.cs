using Quras.IO;
using Quras.IO.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Globalization;
using Quras.VM;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace Quras.Core
{
    public class InvocationTransaction : Transaction
    {
        public byte[] Script;
        public Fixed8 Gas;

        public override int Size => base.Size + Script.GetVarSize();

        public override Fixed8 SystemFee => Gas;

        public InvocationTransaction()
            : base(TransactionType.InvocationTransaction)
        {
        }

        protected override void DeserializeExclusiveData(BinaryReader reader)
        {
            if (Version > 1) throw new FormatException();
            Script = reader.ReadVarBytes(65536);
            if (Script.Length == 0) throw new FormatException();
            if (Version >= 1)
            {
                Gas = reader.ReadSerializable<Fixed8>();
                if (Gas < Fixed8.Zero) throw new FormatException();
            }
            else
            {
                Gas = Fixed8.Zero;
            }
        }

        public static Fixed8 GetGas(Fixed8 consumed)
        {
            Fixed8 gas = consumed - Fixed8.FromDecimal(10);
            if (gas <= Fixed8.Zero) return Fixed8.Zero;
            return gas.Ceiling();
        }

        protected override void SerializeExclusiveData(BinaryWriter writer)
        {
            writer.WriteVarBytes(Script);
            if (Version >= 1)
                writer.Write(Gas);
        }

        public override JObject ToJson()
        {
            JObject json = base.ToJson();
            json["script"] = Script.ToHexString();
            json["gas"] = Gas.ToString();
            return json;
        }

        public bool isDuplicatedToken(string assetName)
        {
            var wb = new WebClient();
            var response = "";
            try
            {
                Console.WriteLine(Settings.Default.APIPrefix + "/v1/assets/all");
                response = wb.DownloadString(Settings.Default.APIPrefix + "/v1/assets/all");
                wb.Dispose();
                var model = new JavaScriptSerializer().Deserialize<HttpAssetInfo>(response);
                foreach (var asset in model.assets)
                {
                    
                    if (asset.name == assetName)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }


            return false;
        }

        public override bool Verify(IEnumerable<Transaction> mempool)
        {
            try
            {
                string assetName;
                ExecutionEngine engine = new ExecutionEngine(this, Cryptography.Crypto.Default);
                Dictionary<CultureInfo, string> _names;
                CultureInfo culture = null;

                engine.LoadScript(this.Script, false);
                engine.Execute();

                if (engine.EvaluationStack.Count == 0)
                    return false;

                if (engine.State == VMState.FAULT)
                {
                    return false;
                }

                StackItem val = engine.EvaluationStack.Pop();
                AssetType asset_type = (AssetType)(byte)val.GetBigInteger();
                if (!Enum.IsDefined(typeof(AssetType), asset_type) || asset_type == AssetType.CreditFlag || asset_type == AssetType.DutyFlag || asset_type == AssetType.GoverningToken || asset_type == AssetType.UtilityToken)
                    return false;

                string name = Encoding.UTF8.GetString(engine.EvaluationStack.Pop().GetByteArray());
                JObject name_obj;
                try
                {
                    name_obj = JObject.Parse(name);
                }
                catch (FormatException)
                {
                    name_obj = name;
                }
                if (name_obj is JString)
                    _names = new Dictionary<CultureInfo, string> { { new CultureInfo("en"), name_obj.AsString() } };
                else
                    _names = ((JArray)JObject.Parse(name)).ToDictionary(p => new CultureInfo(p["lang"].AsString()), p => p["name"].AsString());

                if (culture == null) culture = CultureInfo.CurrentCulture;
                if (_names.TryGetValue(culture, out assetName))
                {
                }
                else if (_names.TryGetValue(new CultureInfo("en"), out assetName))
                {
                }
                else
                {
                    assetName = _names.Values.First();
                }
                if (isDuplicatedToken(assetName))
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
            }

            if (Gas.GetData() % 100000000 != 0) return false;
            return base.Verify(mempool);
        }
    }
}
