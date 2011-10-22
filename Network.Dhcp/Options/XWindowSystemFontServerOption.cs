using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class XWindowSystemFontServerOption : IPRangeOption
    {
        public XWindowSystemFontServerOption(System.Net.IPAddress[] iPAddress)
            : base(iPAddress)
        {
        }

        internal static XWindowSystemFontServerOption Read(System.IO.Stream stream)
        {
            return new XWindowSystemFontServerOption(IPRangeOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.XWindowSystemFontServer; }
        }
    }
}
