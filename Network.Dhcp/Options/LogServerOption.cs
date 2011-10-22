using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class LogServerOption : IPRangeOption
    {
        public LogServerOption(System.Net.IPAddress[] addresses)
            : base(addresses)
        {
        }

        internal static LogServerOption Read(System.IO.Stream stream)
        {
            return new LogServerOption(IPRangeOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.LogServer; }
        }
    }
}
