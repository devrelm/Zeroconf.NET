using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class ResourceLocationServerOption : IPRangeOption
    {
        public ResourceLocationServerOption(System.Net.IPAddress[] addresses)
            : base(addresses)
        {
        }
        internal static ResourceLocationServerOption Read(System.IO.Stream stream)
        {
            return new ResourceLocationServerOption(IPRangeOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.ResourceLocationServer; }
        }
    }
}
