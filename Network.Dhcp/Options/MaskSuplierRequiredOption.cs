using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class MaskSuplierRequiredOption : BooleanOption
    {
        public MaskSuplierRequiredOption(bool p)
            : base(p)
        {
        }

        internal static MaskSuplierRequiredOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new MaskSuplierRequiredOption(stream.ReadByte() == 1);
        }

        public override OptionType Type
        {
            get { return OptionType.MaskSuplierRequiered; }
        }
    }
}
