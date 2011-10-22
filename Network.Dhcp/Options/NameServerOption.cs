using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class NameServerOption : IPRangeOption
    {
        public NameServerOption(System.Net.IPAddress[] addresses)
            : base(addresses)
        {
        }
        internal static NameServerOption Read(System.IO.Stream stream)
        {
            return new NameServerOption(IPRangeOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.NameServer; }
        }
    }
}
