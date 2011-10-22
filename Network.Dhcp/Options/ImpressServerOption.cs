using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class ImpressServerOption : IPRangeOption
    {
        public ImpressServerOption(System.Net.IPAddress[] addresses)
            : base(addresses)
        {
        }

        internal static ImpressServerOption Read(System.IO.Stream stream)
        {
            return new ImpressServerOption(IPRangeOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.ImpressServer; }
        }
    }
}
