using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class DomainNameServerOption : IPRangeOption
    {
        public DomainNameServerOption(System.Net.IPAddress[] addresses)
            : base(addresses)
        {
        }

        internal static DomainNameServerOption Read(System.IO.Stream stream)
        {
            return new DomainNameServerOption(IPRangeOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.DomainNameServer; }
        }
    }
}
