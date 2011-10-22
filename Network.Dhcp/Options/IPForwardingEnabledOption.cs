using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class IpForwardingEnabledOption : BooleanOption
    {
        public IpForwardingEnabledOption(bool value)
            : base(value)
        {
        }
        internal static IpForwardingEnabledOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new IpForwardingEnabledOption(stream.ReadByte() == 1);
        }

        public override OptionType Type
        {
            get { return OptionType.IPForwardingEnabled; }
        }
    }
}
