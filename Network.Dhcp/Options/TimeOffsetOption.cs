using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class TimeOffsetOption : Option
    {
        public TimeOffsetOption(int secondsSinceUTC)
        {
            Offset = new TimeSpan((long)secondsSinceUTC * 10000);
        }

        internal static TimeOffsetOption Read(System.IO.Stream stream)
        {
            //Useless Length
            stream.ReadByte();

            return new TimeOffsetOption(BinaryHelper.ReadInt32(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.TimeOffset; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(4);

            BinaryHelper.Write(stream, (uint)Offset.TotalSeconds);

        }

        public TimeSpan Offset { get; set; }
    }
}
