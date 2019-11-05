using Pure.Cryptography.ECC;
using Pure.IO;
using Pure.IO.Json;
using Pure.VM;
using Pure.Wallets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Pure.Core
{
    public class AssetState : StateBase, ICloneable<AssetState>
    {
        public UInt256 AssetId;
        public AssetType AssetType;
        public string Name;
        public Fixed8 Amount;
        public Fixed8 Available;
        public byte Precision;
        public const byte FeeMode = 0;
        public Fixed8 Fee;
        public Fixed8 FeeMin;
        public Fixed8 FeeMax;
        public Fixed8 AFee;
        public UInt160 FeeAddress;
        public ECPoint Owner;
        public UInt160 Admin;
        public UInt160 Issuer;
        public uint Expiration;
        public bool IsFrozen;

        public override int Size => base.Size + AssetId.Size + sizeof(AssetType) + Name.GetVarSize() + Amount.Size + Available.Size + sizeof(byte) + sizeof(byte) + Fee.Size + FeeMin.Size + FeeMax.Size + FeeAddress.Size + Owner.Size + Admin.Size + Issuer.Size + sizeof(uint) + sizeof(bool) + AFee.Size;

        AssetState ICloneable<AssetState>.Clone()
        {
            return new AssetState
            {
                AssetId = AssetId,
                AssetType = AssetType,
                Name = Name,
                Amount = Amount,
                Available = Available,
                Precision = Precision,
                //FeeMode = FeeMode,
                Fee = Fee,
                FeeMin = FeeMin,
                FeeMax = FeeMax, 
                AFee = AFee,
                FeeAddress = FeeAddress,
                Owner = Owner,
                Admin = Admin,
                Issuer = Issuer,
                Expiration = Expiration,
                IsFrozen = IsFrozen,
                _names = _names
            };
        }

        public override void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);
            AssetId = reader.ReadSerializable<UInt256>();
            AssetType = (AssetType)reader.ReadByte();
            Name = reader.ReadVarString();
            Amount = reader.ReadSerializable<Fixed8>();
            Available = reader.ReadSerializable<Fixed8>();
            Precision = reader.ReadByte();
            reader.ReadByte(); //FeeMode
            Fee = reader.ReadSerializable<Fixed8>(); //Fee
            FeeMin = reader.ReadSerializable<Fixed8>(); //Fee
            FeeMax = reader.ReadSerializable<Fixed8>(); //Fee
            AFee = reader.ReadSerializable<Fixed8>();
            FeeAddress = reader.ReadSerializable<UInt160>();
            Owner = ECPoint.DeserializeFrom(reader, ECCurve.Secp256r1);
            Admin = reader.ReadSerializable<UInt160>();
            Issuer = reader.ReadSerializable<UInt160>();
            Expiration = reader.ReadUInt32();
            IsFrozen = reader.ReadBoolean();
        }

        void ICloneable<AssetState>.FromReplica(AssetState replica)
        {
            AssetId = replica.AssetId;
            AssetType = replica.AssetType;
            Name = replica.Name;
            Amount = replica.Amount;
            Available = replica.Available;
            Precision = replica.Precision;
            //FeeMode = replica.FeeMode;
            Fee = replica.Fee;
            FeeMin = replica.FeeMin;
            FeeMax = replica.FeeMax;
            AFee = replica.AFee;
            FeeAddress = replica.FeeAddress;
            Owner = replica.Owner;
            Admin = replica.Admin;
            Issuer = replica.Issuer;
            Expiration = replica.Expiration;
            IsFrozen = replica.IsFrozen;
            _names = replica._names;
        }

        private Dictionary<CultureInfo, string> _names;
        public string GetName(CultureInfo culture = null)
        {
            if (AssetType == AssetType.GoverningToken) return "XQC";
            if (AssetType == AssetType.UtilityToken) return "XQG";
            if (_names == null)
            {
                JObject name_obj;
                try
                {
                    name_obj = JObject.Parse(Name);
                }
                catch (FormatException)
                {
                    name_obj = Name;
                }
                if (name_obj is JString)
                    _names = new Dictionary<CultureInfo, string> { { new CultureInfo("en"), name_obj.AsString() } };
                else
                    _names = ((JArray)JObject.Parse(Name)).ToDictionary(p => new CultureInfo(p["lang"].AsString()), p => p["name"].AsString());
            }
            if (culture == null) culture = CultureInfo.CurrentCulture;
            if (_names.TryGetValue(culture, out string name))
            {
                return name;
            }
            else if (_names.TryGetValue(en, out name))
            {
                return name;
            }
            else
            {
                return _names.Values.First();
            }
        }

        private static readonly CultureInfo en = new CultureInfo("en");

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write(AssetId);
            writer.Write((byte)AssetType);
            writer.WriteVarString(Name);
            writer.Write(Amount);
            writer.Write(Available);
            writer.Write(Precision);
            writer.Write(FeeMode);
            writer.Write(Fee);
            writer.Write(FeeMin);
            writer.Write(FeeMax);
            writer.Write(AFee);
            writer.Write(FeeAddress);
            writer.Write(Owner);
            writer.Write(Admin);
            writer.Write(Issuer);
            writer.Write(Expiration);
            writer.Write(IsFrozen);
        }

        public override JObject ToJson()
        {
            JObject json = base.ToJson();
            json["id"] = AssetId.ToString();
            json["type"] = AssetType;
            try
            {
                json["name"] = Name == "" ? null : JObject.Parse(Name);
            }
            catch (FormatException)
            {
                json["name"] = Name;
            }
            json["amount"] = Amount.ToString();
            json["available"] = Available.ToString();
            json["precision"] = Precision;
            json["owner"] = Owner.ToString();
            json["admin"] = Wallet.ToAddress(Admin);
            json["issuer"] = Wallet.ToAddress(Issuer);
            json["expiration"] = Expiration;
            json["frozen"] = IsFrozen;
            json["feemode"] = FeeMode.ToString();
            json["fee"] = Fee.ToString();
            json["feemin"] = FeeMin.ToString();
            json["feemax"] = FeeMax.ToString();
            json["afee"] = AFee.ToString();
            return json;
        }

        public override string ToString()
        {
            return GetName();
        }
    }
}
