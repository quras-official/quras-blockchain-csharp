using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Quras.IO;
using Quras.IO.Json;
using Quras.VM;

namespace Quras.Core.RingCT.Types
{
    public class BorromeanSignatureType : IInteropInterface, ISerializable
    {
        public List<byte[]> s0;     // 64 * 32 array
        public List<byte[]> s1;
        public byte[] ee;

        public int Size => s0.Count * 33 + s1.Count * 32 + ee.Length;

        public BorromeanSignatureType()
        {
            s0 = new List<byte[]>();
            s1 = new List<byte[]>();
            ee = new byte[32];
        }

        public void InitSField()
        {
            for(int i = 0; i < ee.Length; i++)
            {
                ee[i] = 0x00;
            }
        }

        public BorromeanSignatureType Exports()
        {
            CheckFields();
            return this;
        }

        public void CheckFields()
        {
            if (s0.Count != 64 || s1.Count != 64 || ee.Length != 32)
                throw new Exception("Borrowmean Format is not correct!");

            for (int i = 0; i < 32; i++)
            {
                if (s1[i].Length != 32)
                {
                    throw new Exception("Borrowmean Format is not correct!");
                }
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            CheckFields();
            
            for (int i = 0; i < 64; i++)
            {
                writer.WriteVarBytes(s0[i]);
            }

            for (int i = 0; i < 64; i++)
            {
                writer.WriteVarBytes(s1[i]);
            }

            writer.WriteVarBytes(ee);
        }

        public void Deserialize(BinaryReader reader)
        {
            for (int i = 0; i < 64; i++)
            {
                byte[] tmp = reader.ReadVarBytes();
                s0.Add(tmp);
            }

            for (int i = 0; i < 64; i++)
            {
                s1.Add(reader.ReadVarBytes());
            }
            ee = reader.ReadVarBytes();

            CheckFields();
        }
    }
}
