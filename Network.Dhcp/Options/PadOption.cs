using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class PadOption : Option
    {
        public override OptionType Type
        {
            get { return OptionType.Pad; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            stream.WriteByte(0);
        }
    }
}
