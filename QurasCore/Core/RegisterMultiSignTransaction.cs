using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Quras.IO;
using Quras.IO.Json;
using Quras.Wallets;
using Quras.Cryptography.ECC;
using Quras.SmartContract;

namespace Quras.Core
{
    public class RegisterMultiSignTransaction : Transaction
    {
        public List<ECPoint> WalletValidatorList = new List<ECPoint>();
        public int ValidatorCount => WalletValidatorList.Count;
        public UInt160 MultiSigRedeemScript = UInt160.Zero;


        public override int Size => base.Size + sizeof(uint) + WalletValidatorList.ToArray().GetVarSize() + MultiSigRedeemScript.Size;

        public RegisterMultiSignTransaction()
            : base(TransactionType.RegisterMultiSignTransaction)
        {
        }

        public void InitializeValidators(ECPoint[] validatorList)
        {
            WalletValidatorList = validatorList.ToList();
            MultiSigRedeemScript = Quras.SmartContract.Contract.CreateMultiSigRedeemScript(WalletValidatorList.Count / 2 + 1, WalletValidatorList.ToArray()).ToScriptHash();
        }


        public override JObject ToJson()
        {
            JObject json = base.ToJson();
            json["MultiSigRedeemScript"] = MultiSigRedeemScript.ToString();
            json["ValidatorCount"] = ValidatorCount;
            JArray jValidator = new JArray();
            for (int i = 0; i < WalletValidatorList.Count; i++)
            {
                JObject jMember = new JObject();
                jMember["PublicKey"] = WalletValidatorList[i].ToString();
                jMember["Address"] = Wallet.ToAddress(Contract.CreateSignatureRedeemScript(WalletValidatorList[i]).ToScriptHash());
                jValidator.Add(jMember);
            }
            json["ValidatorList"] = jValidator;
            return json;
        }

        public String ToJsonString()
        {
            JObject json = new JObject();
            json["multisig_redeem_script"] = MultiSigRedeemScript.ToString();
            json["validators_count"] = ValidatorCount;
            JArray jValidator = new JArray();
            for (int i = 0; i < WalletValidatorList.Count; i++)
                jValidator.Add(WalletValidatorList[i].ToString());
            json["validators"] = jValidator;
            return json.ToString();
        }

        public void FromJsonString(string jsonString)
        {
            WalletValidatorList.Clear();
            JObject obj = JObject.Parse(jsonString);

            MultiSigRedeemScript = UInt160.Parse(obj["multisig_redeem_script"].AsString());
            JArray jValidator = (JArray)obj["validators"];
            for (int i = 0; i < jValidator.Count; i ++)
            {
                WalletValidatorList.Insert(i, ECPoint.Parse(jValidator[i].AsString(), ECCurve.Secp256r1));
            }
        }

        public override bool Verify(IEnumerable<Transaction> mempool)
        {
            if (ValidatorCount > Blockchain.Default.ValidatorMaxCount)
                return false;
            for(int i = 0; i < ValidatorCount - 1; i ++)
            {
                for (int j = i + 1; j < ValidatorCount; j++)
                    if (WalletValidatorList[i].Equals(WalletValidatorList[j]))
                        return false;
            }

            Console.WriteLine("MultiSigRedeemScript");
            Console.WriteLine(this.ToJsonString());
            return this.VerifyScripts();
        }

        protected override void DeserializeExclusiveData(BinaryReader reader)
        {
            if (Version != 1) throw new FormatException();
            MultiSigRedeemScript = reader.ReadSerializable<UInt160>();
            ConvertValidatorFromArray(reader);
        }
        protected override void SerializeExclusiveData(BinaryWriter writer)
        {
            writer.Write(MultiSigRedeemScript);
            ConvertValidatorToArray(writer);
        }

        private void ConvertValidatorToArray(BinaryWriter writer)
        {
            writer.Write((uint)ValidatorCount);

            foreach(ECPoint Validator in WalletValidatorList)
            {
                writer.Write(Validator.ToString());
            }
        }

        public void SerializeSpecial(BinaryWriter writer)
        {
            writer.Write(MultiSigRedeemScript);
            writer.Write((uint)ValidatorCount);
            List<ECPoint> tempList = new List<ECPoint>(WalletValidatorList);
            tempList.Sort();
            foreach(ECPoint Validator in tempList)
            {
                writer.Write(Validator.ToString());
            }
        }

        private void ConvertValidatorFromArray(BinaryReader reader)
        {
            uint nCount;
            nCount = reader.ReadUInt32();
            WalletValidatorList = new List<ECPoint>();
            for (int i = 0; i < nCount; i ++)
            {
                WalletValidatorList.Add(ECPoint.Parse(reader.ReadString(), ECCurve.Secp256r1));
            }
        }

        public void DeserializeFromStream(Stream stream)
        {
            using (BinaryReader binaryReader = new BinaryReader(stream))
            {
                DeserializeExclusiveData(binaryReader);
            }
        }
        public void SerializeToStream(Stream stream)
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(stream))
            {
                SerializeExclusiveData(binaryWriter);
            }
        }

        public bool AddValidator(ECPoint publicKey)
        {
            try
            {
                if (WalletValidatorList.Contains(publicKey)) return false;
                if (WalletValidatorList.Count >= Blockchain.Default.ValidatorMaxCount) return false;
                WalletValidatorList.Add(publicKey);
                MultiSigRedeemScript = Quras.SmartContract.Contract.CreateMultiSigRedeemScript(WalletValidatorList.Count / 2 + 1, WalletValidatorList.ToArray()).ToScriptHash();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override UInt160[] GetScriptHashesForVerifying()
        {
            UInt160 owner = Contract.CreateSignatureRedeemScript(WalletValidatorList[ValidatorCount - 1]).ToScriptHash();
            return base.GetScriptHashesForVerifying().Union(new[] { owner }).OrderBy(p => p).ToArray();
        }

        public UInt160[] GetVerifiableScriptHashes()
        {
            List<UInt160> owner = new List<UInt160>();
            foreach(ECPoint validator in WalletValidatorList)
            {
                owner.Add(Contract.CreateSignatureRedeemScript(validator).ToScriptHash());
            }
            return base.GetScriptHashesForVerifying().Union(owner).ToArray();
        }
    }
}
