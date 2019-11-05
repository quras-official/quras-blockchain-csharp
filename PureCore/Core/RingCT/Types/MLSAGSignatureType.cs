using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Pure.IO;
using Pure.IO.Json;
using Pure.VM;

namespace Pure.Core.RingCT.Types
{
    public class MLSAGSignatureType : IInteropInterface, ISerializable
    {
        public List<List<byte[]>> ss;
        public List<byte[]> II;
        public byte[] cc;

        public int Size => ss.Count * ss[0].Count * ss[0][0].Length + II.Count * II[0].Length + cc.Length;

        public MLSAGSignatureType()
        {
            ss = new List<List<byte[]>>();
            II = new List<byte[]>();
            cc = new byte[32];
        }

        public MLSAGSignatureType Export()
        {
            CheckFields();
            return this;
        }

        public void CheckFields()
        {
            if (ss.Count == 0 || II.Count == 0 || cc.Length == 0)
                throw new Exception("MLSAG Signature Field error");
        }

        public void Serialize(BinaryWriter writer)
        {
            CheckFields();

            writer.Write(ss.Count);
            writer.Write(ss[0].Count);

            for (int i = 0; i < ss.Count; i++)
            {
                for (int j = 0; j < ss[0].Count; j++)
                {
                    writer.WriteVarBytes(ss[i][j]);
                }
            }

            writer.Write(II.Count);
            for (int i = 0; i < II.Count; i++)
            {
                writer.WriteVarBytes(II[i]);
            }

            writer.WriteVarBytes(cc);
        }

        public void Deserialize(BinaryReader reader)
        {
            ss.Clear();
            II.Clear();

            int row = reader.ReadInt32();
            int col = reader.ReadInt32();

            for (int i = 0; i < row; i++)
            {
                List<byte[]> ss_i = new List<byte[]>();
                for (int j = 0; j < col; j++)
                {
                    ss_i.Add(reader.ReadVarBytes());
                }
                ss.Add(ss_i);
            }

            row = reader.ReadInt32();

            for (int i = 0; i < row; i++)
            {
                II.Add(reader.ReadVarBytes());
            }

            cc = reader.ReadVarBytes();
        }
    }
}
