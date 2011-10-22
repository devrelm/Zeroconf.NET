using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Network.Dns
{
    public class Answer : IServerResponse
    {
        public DomainName DomainName { get; set; }
        public Type Type { get; set; }
        public Class Class { get; set; }
        public uint Ttl { get; set; }
        public ResponseData ResponseData { get; set; }

        public byte[] GetBytes()
        {
            return BinaryHelper.GetBytes(this);
        }

        public override string ToString()
        {
            return string.Format("{0}, Type: {1}, Class: {2}, TTL: {3} = {4}", DomainName, Type, Class, Ttl, ResponseData);
        }

        internal static Answer Get(System.IO.BinaryReader reader)
        {
            Answer a = new Answer();
            a.DomainName = DomainName.Get(reader);
            a.Type = (Type)BinaryHelper.ReadUInt16(reader);
            a.Class = (Class)BinaryHelper.ReadUInt16(reader);
            a.Ttl = BinaryHelper.ReadUInt32(reader);
            a.ResponseData = ResponseData.Get(a.Type, reader);
            return a;
        }

        public void WriteTo(Stream stream)
        {
            DomainName.WriteTo(stream);
            BinaryHelper.Write(stream, (ushort)Type);
            BinaryHelper.Write(stream, (ushort)Class);
            BinaryHelper.Write(stream, Ttl);
            if (ResponseData != null)
                ResponseData.WriteTo(stream);
        }
    }
}
