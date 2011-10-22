using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Network.Dhcp
{
    abstract class StringOption : Option
    {
        protected string value;

        public StringOption(string value)
        {
            this.value = value;
        }

        protected static string Read(Stream stream)
        {
            return BinaryHelper.ReadString(stream, stream.ReadByte());
        }

        public override void WriteTo(Stream stream)
        {
            
            BinaryHelper.Write(stream, value);
        }
    }
}
