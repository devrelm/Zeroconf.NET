using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class MaximumDatagramReassemblySizeOption : Option
    {
        public MaximumDatagramReassemblySizeOption(ushort size)
        {
            Size = size;
        }
        internal static MaximumDatagramReassemblySizeOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new MaximumDatagramReassemblySizeOption(BinaryHelper.ReadUInt16(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.MaximumDatagramReassemblySize; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            stream.WriteByte(2);
            BinaryHelper.Write(stream, Size);
        }

        public ushort Size { get; set; }
    }
}
