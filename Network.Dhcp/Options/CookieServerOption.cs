using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class CookieServerOption : IPRangeOption
    {
        public CookieServerOption(System.Net.IPAddress[] addresses)
            : base(addresses)
        {
        }

        internal static CookieServerOption Read(System.IO.Stream stream)
        {
            return new CookieServerOption(IPRangeOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.CookieServer; }
        }
    }
}
