using Pure.Cryptography.ECC;
using Pure.IO;
using Pure.IO.Json;
using Pure.SmartContract;
using Pure.Wallets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pure.Core
{
    [Obsolete]
    public class RegisterTransaction : Transaction
    {
        public AssetType AssetType;
        public string Name;
        public Fixed8 Amount;
        public byte Precision;
        public ECPoint Owner;
        public UInt160 Admin;
        public Fixed8 A_Fee;
        public Fixed8 T_Fee;
        public Fixed8 T_Fee_Min;
        public Fixed8 T_Fee_Max;

        public override int Size => base.Size + sizeof(AssetType) + Name.GetVarSize() + Amount.Size + sizeof(byte) + Owner.Size + Admin.Size + A_Fee.Size + T_Fee.Size + T_Fee_Min.Size + T_Fee_Max.Size;

        public override Fixed8 SystemFee
        {
            get
            {
                if (AssetType == AssetType.GoverningToken || AssetType == AssetType.UtilityToken)
                    return Fixed8.Zero;
                return base.SystemFee;
            }
        }

        public RegisterTransaction()
            : base(TransactionType.RegisterTransaction)
        {
        }

        protected override void DeserializeExclusiveData(BinaryReader reader)
        {
            if (Version != 0) throw new FormatException();
            AssetType = (AssetType)reader.ReadByte();
            Name = reader.ReadVarString(1024);
            Amount = reader.ReadSerializable<Fixed8>();
            Precision = reader.ReadByte();
            Owner = ECPoint.DeserializeFrom(reader, ECCurve.Secp256r1);
            if (Owner.IsInfinity && AssetType != AssetType.GoverningToken && AssetType != AssetType.UtilityToken)
                throw new FormatException();
            Admin = reader.ReadSerializable<UInt160>();
            A_Fee = reader.ReadSerializable<Fixed8>();
            T_Fee = reader.ReadSerializable<Fixed8>();
            T_Fee_Min = reader.ReadSerializable<Fixed8>();
            T_Fee_Max = reader.ReadSerializable<Fixed8>();
        }

        public override UInt160[] GetScriptHashesForVerifying()
        {
            UInt160 owner = Contract.CreateSignatureRedeemScript(Owner).ToScriptHash();
            return base.GetScriptHashesForVerifying().Union(new[] { owner }).OrderBy(p => p).ToArray();
        }

        protected override void OnDeserialized()
        {
            base.OnDeserialized();
            if (AssetType == AssetType.GoverningToken && !Hash.Equals(Blockchain.GoverningToken.Hash))
                throw new FormatException();
            if (AssetType == AssetType.UtilityToken && !Hash.Equals(Blockchain.UtilityToken.Hash))
                throw new FormatException();
        }

        protected override void SerializeExclusiveData(BinaryWriter writer)
        {
            writer.Write((byte)AssetType);
            writer.WriteVarString(Name);
            writer.Write(Amount);
            writer.Write(Precision);
            writer.Write(Owner);
            writer.Write(Admin);
            writer.Write(A_Fee);
            writer.Write(T_Fee);
            writer.Write(T_Fee_Min);
            writer.Write(T_Fee_Max);
        }

        public override JObject ToJson()
        {
            JObject json = base.ToJson();
            json["asset"] = new JObject();
            json["asset"]["type"] = AssetType;
            try
            {
                json["asset"]["name"] = Name == "" ? null : JObject.Parse(Name);
            }
            catch (FormatException)
            {
                json["asset"]["name"] = Name;
            }
            json["asset"]["amount"] = Amount.ToString();
            json["asset"]["precision"] = Precision;
            json["asset"]["owner"] = Owner.ToString();
            json["asset"]["admin"] = Wallet.ToAddress(Admin);
            json["asset"]["A_fee"] = A_Fee.ToString();
            json["asset"]["T_fee"] = T_Fee.ToString();
            json["asset"]["T_fee_min"] = T_Fee_Min.ToString();
            json["asset"]["T_fee_max"] = T_Fee_Max.ToString();
            return json;
        }

        public override bool Verify(IEnumerable<Transaction> mempool)
        {
            return false;
        }
    }
}
