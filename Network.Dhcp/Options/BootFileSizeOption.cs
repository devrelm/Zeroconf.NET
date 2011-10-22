using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class BootFileSizeOption : Option
    {
        public BootFileSizeOption(ushort size)
        {
            Size = size;
        }
        internal static BootFileSizeOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new BootFileSizeOption(BinaryHelper.ReadUInt16(stream));
        }

        public ushort Size { get; set; }

        public override OptionType Type
        {
            get { return OptionType.BootFileSize; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(2);
            BinaryHelper.Write(stream, Size);
        }
    }
}
