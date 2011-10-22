using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class PathMtuPlateauTableOption : Option
    {
        public PathMtuPlateauTableOption(ushort[] size1, ushort[] size2)
        {
            Size1 = size1;
            Size2 = size2;
        }

        internal static PathMtuPlateauTableOption Read(System.IO.Stream stream)
        {
            ushort[] size1 = new ushort[stream.ReadByte() / 2];
            ushort[] size2 = new ushort[size1.Length];
            for (int i = 0; i < size1.Length; i++)
            {
                size1[i] = BinaryHelper.ReadUInt16(stream);
                size2[i] = BinaryHelper.ReadUInt16(stream);
            }
            return new PathMtuPlateauTableOption(size1, size2);
        }

        public override OptionType Type
        {
            get { return OptionType.PathMtuPlateauTable; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte((byte)(Size1.Length + Size2.Length));
            for (int i = 0; i < Size1.Length; i++)
            {
                BinaryHelper.Write(stream, Size1[i]);
                BinaryHelper.Write(stream, Size2[i]);
            }
        }

        public ushort[] Size1 { get; set; }

        public ushort[] Size2 { get; set; }
    }
}
