using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class NetworkTimeProtocolServersOption : IPRangeOption
    {
        public NetworkTimeProtocolServersOption(System.Net.IPAddress[] iPAddress)
            : base(iPAddress)
        {
        }
        internal static NetworkTimeProtocolServersOption Read(System.IO.Stream stream)
        {
            return new NetworkTimeProtocolServersOption(IPRangeOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.NetworkTimeProtocolServers; }
        }
    }
}
