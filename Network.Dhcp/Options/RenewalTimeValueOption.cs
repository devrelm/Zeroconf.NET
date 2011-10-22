using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class RenewalTimeValueOption : Option
    {
        public RenewalTimeValueOption(uint seconds)
        {
            Time = new TimeSpan((long)seconds * 10000);
        }
        internal static RenewalTimeValueOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new RenewalTimeValueOption(BinaryHelper.ReadUInt32(stream));
        }

        public TimeSpan Time { get; set; }

        public override OptionType Type
        {
            get { return OptionType.RenewalTimeValue; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(4);
            BinaryHelper.Write(stream, (uint)Time.TotalSeconds);
        }
    }
}
