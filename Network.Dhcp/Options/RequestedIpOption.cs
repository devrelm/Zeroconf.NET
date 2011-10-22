using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Network.Dhcp
{
    class RequestedIpOption : Option
    {
        public RequestedIpOption(IPAddress iPAddress)
        {
            Address = iPAddress;
        }
        internal static RequestedIpOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new RequestedIpOption(new IPAddress(BinaryHelper.Read(stream, 4)));
        }

        public IPAddress Address { get; set; }

        public override OptionType Type
        {
            get { return OptionType.RequestedIp; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(4);
            BinaryHelper.Write(stream, Address.GetAddressBytes());
        }
    }
}
