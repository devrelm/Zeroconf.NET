using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Network.Dhcp
{
    class ArpCacheTimeoutOption : Option
    {
        public ArpCacheTimeoutOption(uint seconds)
        {
            Timeout = new TimeSpan(seconds);
        }

        internal static ArpCacheTimeoutOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new ArpCacheTimeoutOption(BinaryHelper.ReadUInt32(stream));
        }

        public TimeSpan Timeout { get; set; }

        public override OptionType Type
        {
            get { return OptionType.ArpCacheTimeout; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(4);
            BinaryHelper.Write(stream, (uint)Timeout.TotalSeconds);
        }
    }
}
