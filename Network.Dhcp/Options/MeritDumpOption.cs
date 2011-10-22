using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class MeritDumpOption : StringOption
    {
        public MeritDumpOption(string value)
            : base(value)
        {
        }

        internal static MeritDumpOption Read(System.IO.Stream stream)
        {
            return new MeritDumpOption(StringOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.MeritDump; }
        }
    }
}
