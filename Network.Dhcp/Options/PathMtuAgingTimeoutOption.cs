using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class PathMtuAgingTimeoutOption : Option
    {
        public PathMtuAgingTimeoutOption(uint seconds)
        {
            Timeout = new TimeSpan((long)seconds * 10000);
        }
        internal static PathMtuAgingTimeoutOption Read(System.IO.Stream stream)
        {
            return new PathMtuAgingTimeoutOption(BinaryHelper.ReadUInt32(stream));
        }

        public TimeSpan Timeout { get; set; }

        public override OptionType Type
        {
            get { return OptionType.PathMtuAgingTimeout; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(4);
            BinaryHelper.Write(stream, (uint)Timeout.TotalSeconds);
        }
    }
}
