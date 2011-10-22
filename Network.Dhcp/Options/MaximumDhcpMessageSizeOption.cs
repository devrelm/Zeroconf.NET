using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class MaximumDhcpMessageSizeOption : Option
    {
        public MaximumDhcpMessageSizeOption(ushort p)
        {
            Size = p;
        }
        internal static MaximumDhcpMessageSizeOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new MaximumDhcpMessageSizeOption(BinaryHelper.ReadUInt16(stream));
        }

        public ushort Size { get; set; }

        public override OptionType Type
        {
            get { return OptionType.MaximumDhcpMessageSize; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(2);
            BinaryHelper.Write(stream, Size);
        }
    }
}
