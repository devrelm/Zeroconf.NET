using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class NonLocalSourceRootingEnabledOption : BooleanOption
    {
        public NonLocalSourceRootingEnabledOption(bool value)
            : base(value)
        {
        }

        internal static NonLocalSourceRootingEnabledOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new NonLocalSourceRootingEnabledOption(stream.ReadByte() == 1);
        }

        public override OptionType Type
        {
            get { return OptionType.NonLocalSourceRootingEnabled; }
        }
    }
}
