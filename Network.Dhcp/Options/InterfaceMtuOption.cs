using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class InterfaceMtuOption : Option
    {
        public InterfaceMtuOption(ushort mtu)
        {
            Mtu = mtu;
        }
        internal static InterfaceMtuOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new InterfaceMtuOption(BinaryHelper.ReadUInt16(stream));
        }

        public ushort Mtu { get; set; }

        public override OptionType Type
        {
            get { return OptionType.InterfaceMtu; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(2);
            BinaryHelper.Write(stream, Mtu);
        }
    }
}
