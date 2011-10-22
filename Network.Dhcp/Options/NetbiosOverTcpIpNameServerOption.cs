using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class NetbiosOverTcpIpNameServerOption : IPRangeOption
    {
        public NetbiosOverTcpIpNameServerOption(System.Net.IPAddress[] iPAddress)
            :base(iPAddress)
        {
        }
        internal static NetbiosOverTcpIpNameServerOption Read(System.IO.Stream stream)
        {
            return new NetbiosOverTcpIpNameServerOption(IPRangeOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.NetbiosOverTcpIpNameServer; }
        }
    }
}
