using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    abstract class BooleanOption : Option
    {
        public bool Enabled { get; set; }

        public BooleanOption(bool value)
        {
            Enabled = value;
        }


        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(0);
            stream.WriteByte((byte)(Enabled ? 0 : 1));
        }
    }
}
