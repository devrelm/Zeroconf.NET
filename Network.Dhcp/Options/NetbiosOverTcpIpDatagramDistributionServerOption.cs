using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class NetbiosOverTcpIpDatagramDistributionServerOption : IPRangeOption
    {
        public NetbiosOverTcpIpDatagramDistributionServerOption(System.Net.IPAddress[] iPAddress)
            :base(iPAddress)
        {
        }
        internal static NetbiosOverTcpIpDatagramDistributionServerOption Read(System.IO.Stream stream)
        {
            return new NetbiosOverTcpIpDatagramDistributionServerOption(IPRangeOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.NetbiosOverTcpIpDatagramDistributionServer; }
        }
    }
}
