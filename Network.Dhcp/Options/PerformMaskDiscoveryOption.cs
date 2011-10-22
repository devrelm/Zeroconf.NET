using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class PerformMaskDiscoveryOption : BooleanOption
    {
        public PerformMaskDiscoveryOption(bool p)
            : base(p)
        {
        }

        internal static PerformMaskDiscoveryOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new PerformMaskDiscoveryOption(stream.ReadByte() == 1);
        }

        public override OptionType Type
        {
            get { return OptionType.PerformMaskDiscovery; }
        }
    }
}
