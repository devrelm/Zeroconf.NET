using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class AreAllSubnetsLocalOption : BooleanOption
    {
        public AreAllSubnetsLocalOption(bool p)
            : base(p)
        {
        }

        internal static AreAllSubnetsLocalOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new AreAllSubnetsLocalOption(stream.ReadByte() == 1);
        }

        public override OptionType Type
        {
            get { return OptionType.AreAllSubnetsLocal; }
        }
    }
}
