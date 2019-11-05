using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Pure;
using Pure.IO;
using Pure.Core;
using PureCore.Wallets.AnonymousKey.Note;
using PureCore.Wallets.AnonymousKey.Key;
namespace Pure.Core.Anonoymous
{
    public class NotePlaintext : ISerializable
    {
        public static byte NETWORK_VERSION = 0x01;
        public static byte[] PROTOCOL_VERSION = { 0x01, 0x00, 0x00, 0x01 };

        public Fixed8 value;
        public UInt256 rho;
        public UInt256 r;
        public UInt256 assetID;
        public byte[] memo;

        public int Size => value.Size + rho.Size + r.Size + memo.Length + assetID.Size;
        public NotePlaintext()
        {
            value = new Fixed8(0);
            memo = new byte[0];
            value = new Fixed8();
            rho = new UInt256();
            r = new UInt256();
            assetID = new UInt256();
        }

        public NotePlaintext(Note note, byte[] memo)
        {
            value = note.value;
            rho = note.rho;
            r = note.r;
            this.memo = memo;
            assetID = note.assetID;
        }

        public Note note(PaymentAddress addr) 
        {
            return new Note(addr.a_pk, value, rho, r, assetID);
        }

        public NotePlaintext decrypt(NoteDecryption decryptor, 
                                     byte[] ciphertext,
                                     UInt256 ephermeralKey,
                                     UInt256 h_sig,
                                     byte nonce)
        {
            byte[] plaintext = decryptor.Decrypt(ciphertext, ephermeralKey, h_sig, (char)nonce);

            NotePlaintext pt = new NotePlaintext();
            using (MemoryStream ms = new MemoryStream(plaintext, 0, plaintext.Length, false))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                ((IVerifiable)pt).Deserialize(reader);
                return pt;
            }
        }

        public byte[] encrypt(NoteEncryption encryptor,
                              UInt256 pk_enc)
        {
            NotePlaintext pt = new NotePlaintext();

            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                ((ISerializable)this).Serialize(writer);
                writer.Flush();
                return encryptor.Encrypt(pk_enc, ms.ToArray());
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(NETWORK_VERSION);
            writer.Write(PROTOCOL_VERSION);
            writer.Write(value);
            writer.Write(rho);
            writer.Write(r);
            writer.Write(assetID);
            writer.WriteVarBytes(memo);
        }

        public void Deserialize(BinaryReader reader)
        {
            byte network_version = reader.ReadByte();
            byte[] protocol_version = reader.ReadBytes(4);
            value = reader.ReadSerializable<Fixed8>();
            rho = reader.ReadSerializable<UInt256>();
            r = reader.ReadSerializable<UInt256>();
            assetID = reader.ReadSerializable<UInt256>();
            memo = reader.ReadVarBytes();
        }
    }
}
