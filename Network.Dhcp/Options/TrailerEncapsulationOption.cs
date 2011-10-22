using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class TrailerEncapsulationOption : BooleanOption
    {
        public TrailerEncapsulationOption(bool p)
            : base(p)
        {
        }
        internal static TrailerEncapsulationOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new TrailerEncapsulationOption(stream.ReadByte() == 1);
        }

        public override OptionType Type
        {
            get { return OptionType.TrailerEncapsulation; }
        }
    }
}
