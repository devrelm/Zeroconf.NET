using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Network.Dns
{
    public abstract class ResponseData
    {
        public abstract void WriteTo(Stream writer);

        internal static ResponseData Get(Type type, System.IO.BinaryReader reader)
        {
            switch (type)
            {
                case Type.A:
                case Type.AAAA:
                    return HostAddress.Get(reader);

                case Type.NS:
                    break;
                case Type.MD:
                    break;
                case Type.MF:
                    break;
                case Type.CNAME:
                    return CName.Get(reader);

                case Type.SOA:
                    break;
                case Type.MB:
                    break;
                case Type.MG:
                    break;
                case Type.MR:
                    break;
                case Type.NULL:
                    break;
                case Type.WKS:
                    break;
                case Type.PTR:
                    return Ptr.Get(reader);

                case Type.HINFO:
                    break;
                case Type.MINFO:
                    break;
                case Type.MX:
                    break;
                case Type.TXT:
                    return Txt.Get(reader);

                case Type.SRV:
                    return Srv.Get(reader);

                default:
                    break;
            }
            //throw new NotImplementedException(string.Format("Cannot read {0} response", type));
            return UnknownResponseData.Get(reader);
        }
    }

    public class UnknownResponseData : ResponseData
    {
        public byte[] Bytes { get; protected set; }
        public ushort Length { get { return (ushort)Bytes.Length; } }

        public override void WriteTo(Stream writer)
        {
            ushort length = Length;
            BinaryHelper.Write(writer, length);

            if (length > 0)
                BinaryHelper.Write(writer, Bytes);
        }

        internal static ResponseData Get(BinaryReader reader)
        {
            ushort byteCount = BinaryHelper.ReadUInt16(reader);
            UnknownResponseData data = new UnknownResponseData();
            if (byteCount > 0)
                data.Bytes = reader.ReadBytes(byteCount);
            else
                data.Bytes = new byte[0];

            return data;
        }
    }

    public class CName : ResponseData
    {
        public string CNAME { get; set; }

        public override void WriteTo(Stream writer)
        {
            BinaryHelper.Write(writer, ((ushort)Encoding.UTF8.GetByteCount(CNAME)));
            BinaryHelper.Write(writer, Encoding.UTF8.GetBytes(CNAME));
        }

        internal static CName Get(BinaryReader reader)
        {
            ushort byteCount = BinaryHelper.ReadUInt16(reader);
            CName cName = new CName();
            cName.CNAME = Encoding.UTF8.GetString(reader.ReadBytes(byteCount), 0, byteCount);
            return cName;
        }
    }

    public class HostAddress : ResponseData
    {
        public IPAddress Address { get; set; }

        internal static HostAddress Get(BinaryReader reader)
        {
            ushort byteCount;
            BinaryHelper.FromBytes(reader.ReadBytes(2), out byteCount);
            HostAddress ha = new HostAddress();
            ha.Address = new IPAddress(reader.ReadBytes(byteCount));
            return ha;
        }

        public override void WriteTo(Stream writer)
        {
            byte[] address = Address.GetAddressBytes();
            BinaryHelper.Write(writer, (ushort)address.Length);
            BinaryHelper.Write(writer, address);
        }

        public override string ToString()
        {
            return Address.ToString();
        }
    }
    public class Ptr : ResponseData
    {
        public DomainName DomainName { get; set; }

        public override void WriteTo(Stream writer)
        {
            BinaryHelper.Write(writer, DomainName.GetByteCount());
            DomainName.WriteTo(writer);
        }

        internal static Ptr Get(BinaryReader reader)
        {
            Ptr p = new Ptr();
            //useless datalength
            reader.ReadBytes(2);

            p.DomainName = DomainName.Get(reader);
            //index += byteCount;
            return p;
        }
    }

    public class Srv : ResponseData
    {
        public override void WriteTo(Stream writer)
        {

            BinaryHelper.Write(writer, ((ushort)(6 + Target.GetByteCount())));
            BinaryHelper.Write(writer, Priority);
            BinaryHelper.Write(writer, Weight);
            BinaryHelper.Write(writer, Port);
            Target.WriteTo(writer);
        }

        public ushort Priority { get; set; }
        public ushort Weight { get; set; }
        public ushort Port { get; set; }
        public DomainName Target { get; set; }

        internal static Srv Get(BinaryReader reader)
        {
            Srv srv = new Srv();
            //Useless Datalength
            reader.ReadBytes(2);
            srv.Priority = BinaryHelper.ReadUInt16(reader);
            srv.Weight = BinaryHelper.ReadUInt16(reader);
            srv.Port = BinaryHelper.ReadUInt16(reader);
            srv.Target = DomainName.Get(reader);
            return srv;
        }
    }

    public class Txt : ResponseData
    {
        public Txt()
        {
            Properties = new Dictionary<string, string>();
        }

        public override void WriteTo(Stream writer)
        {
            ushort length = 0;
            List<KeyValuePair<byte[], byte>> bytes = new List<KeyValuePair<byte[], byte>>();
            foreach (KeyValuePair<string, string> kvp in Properties)
            {
                byte[] kvpBytes = Encoding.UTF8.GetBytes(kvp.Key + "=" + kvp.Value);
                bytes.Add(new KeyValuePair<byte[], byte>(kvpBytes, (byte)kvpBytes.Length));
                //writer.Write((byte)kvpBytes.Length);
                //writer.Write(kvpBytes);
                length += (ushort)kvpBytes.Length;
                length++;
            }
            BinaryHelper.Write(writer, length);
            foreach (var properties in bytes)
            {
                writer.WriteByte(properties.Value);
                BinaryHelper.Write(writer, properties.Key);
            }
        }

        public const string True = "true";
        public const string False = "false";

        public void AddProperty(string txt)
        {
            string[] kvp = txt.Split('=');
            if (kvp.Length == 2)
            {
                if (string.Compare(kvp[1], True, true) == 0)
                    Properties.Add(kvp[0], True);
                else if (string.Compare(kvp[1], False, true) == 0)
                    Properties.Add(kvp[0], False);
                else
                    Properties.Add(kvp[0], kvp[1]);
            }
        }

        public bool Contains(string key)
        {
            if (Properties.ContainsKey(key))
                return Properties[key] != False;
            return false;
        }

        public IDictionary<string, string> Properties { get; set; }

        internal static Txt Get(BinaryReader reader)
        {
            Txt txt = new Txt();
            ushort byteCount = BinaryHelper.ReadUInt16(reader), byteRead = 0;
            while (byteRead < byteCount)
            {
                byte propertyLength = reader.ReadByte();
                byteRead += (ushort)(propertyLength + 1);
                txt.AddProperty(Encoding.UTF8.GetString(reader.ReadBytes(propertyLength), 0, propertyLength));
            }
            return txt;
        }
    }
}