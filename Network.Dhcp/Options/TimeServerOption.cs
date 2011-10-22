using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class TimeServerOption : IPRangeOption
    {
        public TimeServerOption(System.Net.IPAddress[] addresses)
            : base(addresses)
        {
        }


        internal static TimeServerOption Read(System.IO.Stream stream)
        {
            return new TimeServerOption(IPRangeOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.TimeServer; }
        }
    }
}
