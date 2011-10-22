using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class XWindowSystemDisplayManagerOption : IPRangeOption
    {
        public XWindowSystemDisplayManagerOption(System.Net.IPAddress[] iPAddress)
            : base(iPAddress)
        {
        }
        internal static XWindowSystemDisplayManagerOption Read(System.IO.Stream stream)
        {
            return new XWindowSystemDisplayManagerOption(IPRangeOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.XWindowSystemDisplayManager; }
        }
    }
}
