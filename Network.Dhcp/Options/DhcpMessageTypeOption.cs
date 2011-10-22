using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    public enum DhcpMessageType : byte
    {
        Discover = 1,
        Offer = 2,
        Request = 3,
        Decline = 4,
        Ack = 5,
        Nak = 6,
        Release = 7,
    }

    class DhcpMessageTypeOption : Option
    {
        public DhcpMessageTypeOption(DhcpMessageType dhcpMessageType)
        {
            MessageType = dhcpMessageType;
        }
        internal static DhcpMessageTypeOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new DhcpMessageTypeOption((DhcpMessageType)stream.ReadByte());
        }

        public DhcpMessageType MessageType { get; set; }

        public override OptionType Type
        {
            get { return OptionType.DhcpMessageType; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(1);
            stream.WriteByte((byte)MessageType);
        }
    }
}
