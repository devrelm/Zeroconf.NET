using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class DomainNameOption : StringOption
    {
        public DomainNameOption(string value)
            : base(value)
        {

        }

        internal static DomainNameOption Read(System.IO.Stream stream)
        {
            return new DomainNameOption(StringOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.DomainName; }
        }
    }
}
