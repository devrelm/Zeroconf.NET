using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class EthernetEncapsulationOption : BooleanOption
    {
        public EthernetEncapsulationOption(bool p)
            : base(p)
        {
        }
        internal static EthernetEncapsulationOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new EthernetEncapsulationOption(stream.ReadByte() == 1);
        }

        public override OptionType Type
        {
            get { return OptionType.EthernetEncapsulation; }
        }
    }
}
