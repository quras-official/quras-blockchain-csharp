﻿using System;
using System.Collections.Generic;
using System.Text;

using Quras.IO;
using Quras.IO.Json;
using Quras.VM;
using Quras.Cryptography.ECC;
using System.IO;

namespace Quras.Core.RingCT.Types
{
    public class RangeProveType : IInteropInterface, ISerializable
    {
        public RangeSigatureType rangeSig;
        public ECPoint C;
        public byte[] mask;

        public int Size => rangeSig.Size + 33 + 32;

        public RangeProveType()
        {
            rangeSig = new RangeSigatureType();
            C = new ECPoint();
            mask = new byte[32];
        }

        public RangeProveType(RangeSigatureType sig, ECPoint C, byte[] mask)
        {
            mask = new byte[32];

            this.rangeSig = sig;
            this.C = C;
            Buffer.BlockCopy(mask, 0, this.mask, 0, 32);
        }
    
        public RangeProveType Export()
        {
            CheckFields();
            return this;
        }

        public void CheckFields()
        {
            rangeSig.CheckFields();

            if (mask == null || mask.Length != 32)
                throw new Exception("Range Prover Type is not correct!");
        }

        public void Serialize(BinaryWriter writer)
        {
            CheckFields();

            writer.Write(rangeSig);
            writer.WriteVarBytes(C.EncodePoint(true));
            writer.WriteVarBytes(mask);
        }

        public void Deserialize(BinaryReader reader)
        {
            rangeSig = reader.ReadSerializable<RangeSigatureType>();
            C = ECPoint.DecodePoint(reader.ReadVarBytes(), ECCurve.Secp256r1);
            mask = reader.ReadVarBytes();

            CheckFields();
        }
    }
    public class RangeSigatureType : IInteropInterface, ISerializable
    {
        public BorromeanSignatureType boroSig;
        public List<ECPoint> Ci;

        public int Size => boroSig.Size + 64 * 33;

        public RangeSigatureType()
        {
            boroSig = new BorromeanSignatureType();
            Ci = new List<ECPoint>();
        }

        public RangeSigatureType Export()
        {
            CheckFields();
            return this;
        }

        public void CheckFields()
        {
            boroSig.CheckFields();

            if (Ci.Count != 64)
                throw new Exception("ASNL Format is not correct!");
        }

        public void Serialize(BinaryWriter writer)
        {
            CheckFields();
            writer.Write(boroSig);
            
            for (int i = 0; i < Ci.Count; i++)
            {
                writer.WriteVarBytes(Ci[i].EncodePoint(true));
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            boroSig = reader.ReadSerializable<BorromeanSignatureType>();

            for (int i = 0; i < 64; i++)
            {
                Ci.Add(ECPoint.DecodePoint(reader.ReadVarBytes(), ECCurve.Secp256r1));
            }

            CheckFields();
        }
    }
}
