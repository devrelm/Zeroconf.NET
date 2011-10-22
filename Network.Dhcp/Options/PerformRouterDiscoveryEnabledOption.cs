using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class PerformRouterDiscoveryEnabledOption : BooleanOption
    {
        public PerformRouterDiscoveryEnabledOption(bool p)
            : base(p)
        {
        }

        internal static PerformRouterDiscoveryEnabledOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new PerformRouterDiscoveryEnabledOption(stream.ReadByte() == 1);
        }

        public override OptionType Type
        {
            get { return OptionType.PerformRouterDiscoveryEnabled; }
        }
    }
}
