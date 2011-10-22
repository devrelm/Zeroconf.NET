using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Network
{
    public static class BinaryHelper
    {

        public static void Write(Stream stream, ushort p)
        {
            Write(stream, ToBytes(p));
        }

        public static byte[] GetBytes(IServerResponse response)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                response.WriteTo(stream);
                return stream.ToArray();
            }
        }

        public static byte[] GetBytes(IClientRequest response)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                response.WriteTo(stream);
                return stream.ToArray();
            }
        }

        public static string ReadLine(BinaryReader reader)
        {
            bool stopReading = false;
            List<char> chars = new List<char>();
            char[] readChar = reader.ReadChars(2);
            if (readChar.Length == 0)
                return null;

            if (new string(readChar) == Environment.NewLine)
                stopReading = true;

            while (!stopReading)
            {
                chars.Add(readChar[0]);
                readChar[0] = readChar[1];
                readChar[1] = reader.ReadChar();
                if (new string(readChar) == Environment.NewLine)
                    stopReading = true;
            }

            return new string(chars.ToArray());
        }

        #region Write

        public static void Write(Stream stream, byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }

        public static void Write(Stream stream, int value)
        {
            Write(stream, BitConverter.GetBytes(value));
        }

        public static void Write(Stream stream, uint value)
        {
            Write(stream, BitConverter.GetBytes(value));
        }

        #endregion

        #region Read

        public static byte[] Read(Stream stream, int p)
        {
            byte[] bytes = new byte[p];
            int len = 0;
            do
            {
                len += stream.Read(bytes, 0, p);
            }
            while (len < p);
            return bytes;
        }

        public static int ReadInt32(Stream stream)
        {
            return ReadInt32(BinaryHelper.Read(stream, 4));
        }

        public static ushort ReadUInt16(Stream stream)
        {
            return ReadUInt16(BinaryHelper.Read(stream, sizeof(ushort)));
        }
        public static uint ReadUInt32(Stream stream)
        {
            return ReadUInt32(BinaryHelper.Read(stream, sizeof(uint)));
        }
        public static ulong ReadUInt64(Stream stream)
        {
            return ReadUInt64(BinaryHelper.Read(stream, sizeof(ulong)));
        }

        #endregion

        public static string ReadString(Stream stream, int p, Encoding encoding = null)
        {
            byte[] sBytes = Read(stream, p);
            if (encoding == null)
                encoding = Encoding.ASCII;
            return encoding.GetString(sBytes);
        }

        public static void Write(Stream stream, string s, bool writeLength = true, Encoding encoding = null)
        {
            if (writeLength)
                Write(stream, s.Length);
            if (encoding == null)
                encoding = Encoding.ASCII;
            Write(stream, encoding.GetBytes(s));
        }

        public static ushort ReadUInt16(BinaryReader reader)
        {
            ushort result;
            FromBytes(reader.ReadBytes(2), out result);
            return result;
        }

        public static uint ReadUInt32(BinaryReader reader)
        {
            uint result;
            FromBytes(reader.ReadBytes(4), out result);
            return result;
        }

        public static uint ReadUInt32(byte[] p)
        {
            return BitConverter.ToUInt32(p, 0);
        }
        public static int ReadInt32(byte[] p)
        {
            return BitConverter.ToInt32(p, 0);
        }
        public static ulong ReadUInt64(byte[] p)
        {
            return BitConverter.ToUInt64(p, 0);
        }

        public static long ReadInt64(byte[] p)
        {
            return BitConverter.ToInt64(p, 0);
        }

        public static void Write(BinaryWriter writer, string s, bool writeLength = true, Encoding encoding = null)
        {
            if (writeLength)
                writer.Write(s.Length);
            if (encoding == null)
                encoding = Encoding.ASCII;
            writer.Write(encoding.GetBytes(s));
        }

        public static void WriteByteLength(BinaryWriter writer, string s, bool writeLength = true, Encoding encoding = null)
        {
            if (writeLength)
                writer.Write((byte)s.Length);
            Write(writer, s, false, encoding);
        }

        public static void WriteShortLength(BinaryWriter writer, string s, bool writeLength = true, Encoding encoding = null)
        {
            if (writeLength)
                writer.Write((ushort)s.Length);
            Write(writer, s, false, encoding);
        }

        public static void WriteIntLength(BinaryWriter writer, string s, bool writeLength = true, Encoding encoding = null)
        {
            if (writeLength)
                writer.Write((uint)s.Length);
            Write(writer, s, false, encoding);
        }



        public static ushort ReadUInt16(byte[] p)
        {
            return BitConverter.ToUInt16(p, 0);
        }

        public static byte[] ToBytes(uint i)
        {
            byte[] bytes = new byte[4];
            bytes[3] = (byte)(i % (byte.MaxValue + 1));
            i = i >> 8;
            bytes[2] = (byte)(i % (byte.MaxValue + 1));
            i = i >> 8;
            bytes[1] = (byte)(i % (byte.MaxValue + 1));
            i = i >> 8;
            bytes[0] = (byte)(i % (byte.MaxValue + 1));
            return bytes;
        }

        public static void Write(BinaryWriter writer, ushort s)
        {
            writer.Write(ToBytes(s));
        }

        public static byte[] ToBytes(ushort s)
        {
            byte[] bytes = new byte[2];
            bytes[1] = (byte)(s % (byte.MaxValue + 1));
            s = (ushort)(s >> 8);
            bytes[0] = (byte)(s % (byte.MaxValue + 1));
            return bytes;
        }

        public static long LongFromBytes(byte[] bytes, int offset, int length)
        {
            long result = 0;
            for (int i = offset + length - 1; i >= offset; i--)
            {
                result += bytes[i] << (length - 1 - i + offset) * 8;
            }
            return result;
        }

        public static void FromBytes(byte[] bytes, int offset, out ushort s)
        {
            s = (ushort)LongFromBytes(bytes, offset, 2);
        }

        public static void FromBytes(byte[] bytes, out ushort s)
        {
            FromBytes(bytes, 0, out s);
        }

        public static void FromBytes(byte[] bytes, out uint i)
        {
            FromBytes(bytes, 0, out i);
        }

        public static void FromBytes(byte[] bytes, int offset, out uint i)
        {
            i = (uint)LongFromBytes(bytes, offset, 4);
        }

        public static void Write(BinaryWriter writer, uint i)
        {
            writer.Write(ToBytes(i));
        }

        public static T FromBytes<T>(IClientResponse<T> response, byte[] responseBytes)
        {
            using (MemoryStream ms = new MemoryStream(responseBytes))
            {
                return response.GetResponse(ms);
            }
        }

        public static T FromBytes<T>(IServerRequest<T> response, byte[] responseBytes)
        {
            using (MemoryStream ms = new MemoryStream(responseBytes))
            {
                return response.GetRequest(ms);
            }
        }
    }
}
