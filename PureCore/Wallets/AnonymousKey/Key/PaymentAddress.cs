using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Pure;
using Pure.IO;

namespace PureCore.Wallets.AnonymousKey.Key
{
    public class PaymentAddress : ISerializable
    {
        public UInt256 a_pk;
        public UInt256 pk_enc;

        public int Size => a_pk.Size + pk_enc.Size;

        public PaymentAddress()
        {
            a_pk = new UInt256();
            pk_enc = new UInt256();
        }

        public PaymentAddress(UInt256 p_a_pk, UInt256 p_pk_enc)
        {
            a_pk = new UInt256(p_a_pk);
            pk_enc = new UInt256(p_pk_enc);
        }

        public int GetVarSize()
        {
            return a_pk.Size + pk_enc.Size;
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.Write(a_pk.ToArray());
            writer.Write(pk_enc.ToArray());
        }

        void ISerializable.Deserialize(BinaryReader reader)
        {
            byte[] data_bytes = new byte[a_pk.Size];
            reader.Read(data_bytes, 0, data_bytes.Length);
            a_pk = new UInt256(data_bytes);
            byte[] data_pk_enc = new byte[pk_enc.Size];
            reader.Read(data_pk_enc, 0, data_pk_enc.Length);
            pk_enc = new UInt256(data_pk_enc);
        }
    }
}
