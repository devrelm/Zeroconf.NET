using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class NetworkInformationServiceDomainOption : StringOption
    {
        public string NisDomainName
        {
            get { return value; }
            set { this.value = value; }
        }

        public NetworkInformationServiceDomainOption(string p)
            : base(p)
        {
        }

        internal static NetworkInformationServiceDomainOption Read(System.IO.Stream stream)
        {
            return new NetworkInformationServiceDomainOption(StringOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.NetworkInformationServiceDomain; }
        }
    }
}
