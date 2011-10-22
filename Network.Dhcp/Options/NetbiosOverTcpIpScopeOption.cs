using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class NetbiosOverTcpIpScopeOption : StringOption
    {
        public NetbiosOverTcpIpScopeOption(string p)
            : base(p)
        {
        }

        internal static NetbiosOverTcpIpScopeOption Read(System.IO.Stream stream)
        {
            return new NetbiosOverTcpIpScopeOption(StringOption.Read(stream));
        }

        public string Scope
        {
            get { return value; }
            set { this.value = value; }
        }

        public override OptionType Type
        {
            get { return OptionType.NetbiosOverTcpIpScope; }
        }
    }
}
