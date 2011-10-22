using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Network.Dhcp
{
    class NetworkInformationServersOption : IPRangeOption
    {
        public NetworkInformationServersOption(IPAddress[] addresses)
            : base(addresses)
        {
        }
        internal static NetworkInformationServersOption Read(System.IO.Stream stream)
        {
            return new NetworkInformationServersOption(IPRangeOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.NetworkInformationServers; }
        }
    }
}
