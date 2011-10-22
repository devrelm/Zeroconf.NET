using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class ExtensionsPathOption : StringOption
    {
        public ExtensionsPathOption(string value)
            : base(value)
        {
        }

        internal static ExtensionsPathOption Read(System.IO.Stream stream)
        {
            return new ExtensionsPathOption(StringOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.ExtensionsPath; }
        }
    }
}
