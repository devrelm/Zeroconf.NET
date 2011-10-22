using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class RebindingTimeValueOption : Option
    {
        public RebindingTimeValueOption(uint seconds)
        {
            Time = new TimeSpan((long)seconds * 10000);
        }
        internal static RebindingTimeValueOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new RebindingTimeValueOption(BinaryHelper.ReadUInt32(stream));
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
