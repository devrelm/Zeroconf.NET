using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Network.Dhcp
{
    class ServerIdentifierOption : Option
    {
        public ServerIdentifierOption(IPAddress iPAddress)
        {
            Address = iPAddress;
        }
        internal static ServerIdentifierOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new ServerIdentifierOption(new IPAddress(BinaryHelper.Read(stream, 4)));
        }

        public override OptionType Type
        {
            get { return OptionType.ServerIdentifier; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(4);
            BinaryHelper.Write(stream, Address.GetAddressBytes());
        }

        public IPAddress Address { get; set; }
    }
}
