using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class HostNameOption : StringOption
    {
        public string HostName
        {
            get { return value; }
            set { this.value = value; }
        }

        public HostNameOption(string value)
            : base(value)
        {
        }

        internal static HostNameOption Read(System.IO.Stream stream)
        {
            return new HostNameOption(StringOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.HostName; }
        }
    }
}
