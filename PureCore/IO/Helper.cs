using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;

namespace Pure.IO
{
    public static class Helper
    {
        public static T AsSerializable<T>(this byte[] value) where T : ISerializable, new()
        {
            using (MemoryStream ms = new MemoryStream(value, false))
            using (BinaryReader reader = new BinaryReader(ms, Encoding.UTF8))
            {
                return reader.ReadSerializable<T>();
            }
        }

        public static ISerializable AsSerializable(this byte[] value, Type type)
        {
            if (!typeof(ISerializable).GetTypeInfo().IsAssignableFrom(type))
                throw new InvalidCastException();
            ISerializable serializable = (ISerializable)Activator.CreateInstance(type);
            using (MemoryStream ms = new MemoryStream(value, false))
            using (BinaryReader reader = new BinaryReader(ms, Encoding.UTF8))
            {
                serializable.Deserialize(reader);
            }
            return serializable;
        }

        internal static int GetVarSize(int value)
        {
            if (value < 0xFD)
                return sizeof(byte);
            else if (value <= 0xFFFF)
                return sizeof(byte) + sizeof(ushort);
            else
                return sizeof(byte) + sizeof(uint);
        }

        internal static int GetListLength(this List<byte[]> value)
        {
            int length = 0;
            for (int i = 0; i < value.Count; i++)
            {
                length += value[i].Length;
            }

            return length;
        }

        internal static unsafe bool UnsafeCompare(this byte[] a1, byte[] a2)
        {
            if (a1 == a2) return true;
            if (a1 == null || a2 == null || a1.Length != a2.Length)
                return false;
            fixed (byte* p1 = a1, p2 = a2)
            {
                byte* x1 = p1, x2 = p2;
                int l = a1.Length;
                for (int i = 0; i < l / 8; i++, x1 += 8, x2 += 8)
                    if (*((long*)x1) != *((long*)x2)) return false;
                if ((l & 4) != 0) { if (*((int*)x1) != *((int*)x2)) return false; x1 += 4; x2 += 4; }
                if ((l & 2) != 0) { if (*((short*)x1) != *((short*)x2)) return false; x1 += 2; x2 += 2; }
                if ((l & 1) != 0) if (*((byte*)x1) != *((byte*)x2)) return false;
                return true;
            }
        }

        internal static int GetVarSize<T>(this T[] value)
        {
            int value_size;
            Type t = typeof(T);
            if (typeof(ISerializable).IsAssignableFrom(t))
            {
                value_size = value.OfType<ISerializable>().Sum(p => p.Size);
            }
            else if (t.GetTypeInfo().IsEnum)
            {
                int element_size;
                Type u = t.GetTypeInfo().GetEnumUnderlyingType();
                if (u == typeof(sbyte) || u == typeof(byte))
                    element_size = 1;
                else if (u == typeof(short) || u == typeof(ushort))
                    element_size = 2;
                else if (u == typeof(int) || u == typeof(uint))
                    element_size = 4;
                else //if (u == typeof(long) || u == typeof(ulong))
                    element_size = 8;
                value_size = value.Length * element_size;
            }
            else
            {
                value_size = value.Length * Marshal.SizeOf<T>();
            }
            return GetVarSize(value.Length) + value_size;
        }

        internal static int GetVarSize(this string value)
        {
            int size = Encoding.UTF8.GetByteCount(value);
            return GetVarSize(size) + size;
        }

        public static string ReadFixedString(this BinaryReader reader, int length)
        {
            byte[] data = reader.ReadBytes(length);
            return Encoding.UTF8.GetString(data.TakeWhile(p => p != 0).ToArray());
        }

        public static T ReadSerializable<T>(this BinaryReader reader) where T : ISerializable, new()
        {
            T obj = new T();
            obj.Deserialize(reader);
            return obj;
        }

        public static T[] ReadSerializableArray<T>(this BinaryReader reader, int max = 0x10000000) where T : ISerializable, new()
        {
            T[] array = new T[reader.ReadVarInt((ulong)max)];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new T();
                array[i].Deserialize(reader);
            }
            return array;
        }

        public static List<T> ReadSerializableList<T>(this BinaryReader reader, int max = 0x10000000) where T : ISerializable, new()
        {
            T[] array = new T[reader.ReadVarInt((ulong)max)];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new T();
                array[i].Deserialize(reader);
            }
            return array.ToList<T>();
        }

        public static byte[] ReadVarBytes(this BinaryReader reader, int max = 0X7fffffc7)
        {
            return reader.ReadBytes((int)reader.ReadVarInt((ulong)max));
        }

        public static ulong ReadVarInt(this BinaryReader reader, ulong max = ulong.MaxValue)
        {
            byte fb = reader.ReadByte();
            ulong value;
            if (fb == 0xFD)
                value = reader.ReadUInt16();
            else if (fb == 0xFE)
                value = reader.ReadUInt32();
            else if (fb == 0xFF)
                value = reader.ReadUInt64();
            else
                value = fb;
            if (value > max) throw new FormatException();
            return value;
        }

        public static string ReadVarString(this BinaryReader reader, int max = 0X7fffffc7)
        {
            return Encoding.UTF8.GetString(reader.ReadVarBytes(max));
        }

        public static byte[] ToArray(this ISerializable value)
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms, Encoding.UTF8))
            {
                value.Serialize(writer);
                writer.Flush();
                return ms.ToArray();
            }
        }

        public static void Write(this BinaryWriter writer, ISerializable value)
        {
            value.Serialize(writer);
        }

        public static void Write(this BinaryWriter writer, ISerializable[] value)
        {
            writer.WriteVarInt(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                value[i].Serialize(writer);
            }
        }

        public static void Write(this BinaryWriter writer, ISerializable[][] value)
        {
            writer.WriteVarInt(value.Length);
            writer.WriteVarInt(value[0].Length);

            for (int i = 0; i < value.Length; i++)
            {
                for (int j = 0; j < value[0].Length; j++)
                {
                    value[i][j].Serialize(writer);
                }
            }
        }

        public static void WriteFixedString(this BinaryWriter writer, string value, int length)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (value.Length > length)
                throw new ArgumentException();
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            if (bytes.Length > length)
                throw new ArgumentException();
            writer.Write(bytes);
            if (bytes.Length < length)
                writer.Write(new byte[length - bytes.Length]);
        }

        public static void WriteVarBytes(this BinaryWriter writer, byte[] value)
        {
            writer.WriteVarInt(value.Length);
            writer.Write(value);
        }

        public static void WriteVarInt(this BinaryWriter writer, long value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException();
            if (value < 0xFD)
            {
                writer.Write((byte)value);
            }
            else if (value <= 0xFFFF)
            {
                writer.Write((byte)0xFD);
                writer.Write((ushort)value);
            }
            else if (value <= 0xFFFFFFFF)
            {
                writer.Write((byte)0xFE);
                writer.Write((uint)value);
            }
            else
            {
                writer.Write((byte)0xFF);
                writer.Write(value);
            }
        }

        public static void WriteVarString(this BinaryWriter writer, string value)
        {
            writer.WriteVarBytes(Encoding.UTF8.GetBytes(value));
        }
    }
}
