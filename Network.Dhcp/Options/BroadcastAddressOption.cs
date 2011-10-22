using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Network.Dhcp
{
    class BroadcastAddressOption : Option
    {
        public BroadcastAddressOption(IPAddress address)
        {
            BroadcastAddress = address;
        }

        internal static BroadcastAddressOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new BroadcastAddressOption(new IPAddress(BinaryHelper.Read(stream, 4)));
        }

        public IPAddress BroadcastAddress { get; set; }

        public override OptionType Type
        {
            get { return OptionType.BroadcastAddress; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(4);
            BinaryHelper.Write(stream, BroadcastAddress.GetAddressBytes());
        }
    }
}
