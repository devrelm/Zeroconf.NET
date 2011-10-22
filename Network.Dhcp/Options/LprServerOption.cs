using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class LprServerOption : IPRangeOption
    {
        private System.Net.IPAddress[] iPAddress;

        public LprServerOption(System.Net.IPAddress[] addresses)
            : base(addresses)
        {
        }

        internal static LprServerOption Read(System.IO.Stream stream)
        {
            return new LprServerOption(IPRangeOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.LprServer; }
        }
    }
}
