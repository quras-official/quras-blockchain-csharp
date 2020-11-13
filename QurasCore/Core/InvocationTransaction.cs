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
using Quras.SmartContract;

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
                ApplicationEngine engine = ApplicationEngine.Run(Script, this);

                if (!engine.State.HasFlag(VMState.HALT))
                {
                    return false;
                }

                if (Gas < engine.GasConsumed.Ceiling())
                {
                    return false;
                }

                string assetName = GetAssetName();
                if (!assetName.Equals("") && isDuplicatedToken(assetName))
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

        private String GetAssetName()
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream(Script);

                int skipLength = 21 + 9 + 9 + 9 + 9 + 21 + 21;
                byte[] temp = new byte[skipLength];

                // skip feeAddress(UINT160), tFeeMax(Fixed8), tFeeMin(Fixed8), tFee(Fixed8), aFee(Fixed8), issur(UINT160), admin(UNINT160)
                ms.Read(temp, 0, skipLength);

                if (!InvocationUtils.SkipECPoint(ms)) throw new Exception();

                // skip precision(byte), amount(Fixed8)
                skipLength = 1 + 9;
                ms.Read(temp, 0, skipLength);

                string jname = InvocationUtils.GetString(ms);
                string name = ((JArray)JObject.Parse(jname))[0]["name"].AsString();
                if (name == "") throw new Exception();

                // skip assetType(byte)
                InvocationUtils.SkipByte(ms);

                string syscall = InvocationUtils.GetSysCall(ms);
                if (syscall != "Quras.Asset.Create") throw new Exception();

                ms.Close();

                return name;
            }
            catch (Exception ex)
            {
                if (ms != null)
                {
                    ms.Close();
                }

                return "";
            }
        }
    }

    class InvocationUtils
    {
        public static string GetString(MemoryStream ms)
        {
            try
            {
                byte[] byRet = null;
                int length = 0;
                byte opcode = (byte)ms.ReadByte();

                if ((byte) OpCode.PUSHBYTES75 >= opcode)
                {
                    length = opcode;
                    byRet = new byte[length];
                }
                else if ((byte) OpCode.PUSHDATA1 == opcode)
                {
                    length = ms.ReadByte();
                    byRet = new byte[length];
                }
                else if ((byte) OpCode.PUSHDATA2 == opcode)
                {
                    byte[] byLength = new byte[2];
                    ms.Read(byLength, 0, 2);
                    length = BitConverter.ToInt16(byLength, 0);
                    byRet = new byte[length];
                }
                else if ((byte) OpCode.PUSHDATA4 == opcode)
                {
                    byte[] byLength = new byte[4];
                    ms.Read(byLength, 0, 4);
                    length = BitConverter.ToInt16(byLength, 0);
                    byRet = new byte[length];
                }
                else
                {
                    return "";
                }

                if (length <= 0)
                {
                    return "";
                }

                ms.Read(byRet, 0, length);

                return Encoding.UTF8.GetString(byRet);
            }
            catch (Exception)
            {
                return "";
            }

        }

        public static string GetSysCall(MemoryStream ms)
        {
            try
            {
                byte opcode = (byte)ms.ReadByte();
                if ((byte) OpCode.SYSCALL != opcode)
                {
                    return "";
                }

                int length = ms.ReadByte();
                byte[] byRet = new byte[length];
                ms.Read(byRet, 0, length);

                return Encoding.ASCII.GetString(byRet);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static bool SkipECPoint(MemoryStream ms)
        {
            try
            {
                byte opcode = (byte)ms.ReadByte();

                byte[] byRet = new byte[opcode];
                ms.Read(byRet, 0, opcode);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool SkipByte(MemoryStream ms)
        {
            try
            {
                byte opcode = (byte)ms.ReadByte();

                if (opcode == (byte) OpCode.PUSH0) return true;
                if (opcode == (byte) OpCode.PUSHM1) return true;
                if (opcode >= (byte) OpCode.PUSH1 && opcode <= (byte) OpCode.PUSH16) return true;

                if (opcode != 0x01) return false;
                ms.ReadByte();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
