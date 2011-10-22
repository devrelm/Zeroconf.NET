using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Network.Dhcp
{
    abstract class IPRangeOption : Option
    {
        public IPRangeOption(IPAddress[] address)
        {
            Addresses = address;
        }
        
        internal static IPAddress[] Read(System.IO.Stream stream)
        {
            byte length = (byte)stream.ReadByte();
            IPAddress[] address = new IPAddress[length / 4];
            for (int i = 0; i < address.Length; i++)
                address[i] = new IPAddress(BinaryHelper.Read(stream, 4));
            return address;
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            stream.WriteByte((byte)Addresses.Length);

            foreach (var address in Addresses)
                BinaryHelper.Write(stream, address.GetAddressBytes());
        }

        public IPAddress[] Addresses { get; set; }
    }
}
