using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Pure.IO;
using Pure.IO.Json;
using Pure.VM;

namespace Pure.Core.RingCT.Types
{
    public class ASNLSignatureType : IInteropInterface, ISerializable
    {
        public List<Cryptography.ECC.ECPoint> L1;     // 64 * 32 array
        public List<byte[]> s2;
        public byte[] s;

        public int Size => L1.Count * 33 + s2.Count * 32 + s.Length;

        public ASNLSignatureType()
        {
            L1 = new List<Cryptography.ECC.ECPoint>();
            s2 = new List<byte[]>();
            s = new byte[32];
        }

        public void InitSField()
        {
            for(int i = 0; i < s.Length; i++)
            {
                s[i] = 0x00;
            }
        }

        public ASNLSignatureType Exports()
        {
            CheckFields();
            return this;
        }

        public void CheckFields()
        {
            if (L1.Count != 64 || s2.Count != 64 || s.Length != 32)
                throw new Exception("ASNL Format is not correct!");

            for (int i = 0; i < 32; i++)
            {
                if (s2[i].Length != 32)
                {
                    throw new Exception("ASNL Format is not correct!");
                }
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            CheckFields();
            
            for (int i = 0; i < 64; i++)
            {
                writer.WriteVarBytes(L1[i].EncodePoint(true));
            }

            for (int i = 0; i < 64; i++)
            {
                writer.WriteVarBytes(s2[i]);
            }

            writer.WriteVarBytes(s);
        }

        public void Deserialize(BinaryReader reader)
        {
            for (int i = 0; i < 64; i++)
            {
                byte[] tmp = reader.ReadVarBytes();
                L1.Add(Pure.Cryptography.ECC.ECPoint.DecodePoint(tmp, Cryptography.ECC.ECCurve.Secp256r1));
            }

            for (int i = 0; i < 64; i++)
            {
                s2.Add(reader.ReadVarBytes());
            }
            s = reader.ReadVarBytes();

            CheckFields();
        }
    }
}
