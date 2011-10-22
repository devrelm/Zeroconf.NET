using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Network.Dhcp
{
    class RouterOption : IPRangeOption
    {
        public RouterOption(IPAddress[] address)
            : base(address)
        {
        }

        internal static RouterOption Read(System.IO.Stream stream)
        {
            return new RouterOption(IPRangeOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.Router; }
        }
    }
}
