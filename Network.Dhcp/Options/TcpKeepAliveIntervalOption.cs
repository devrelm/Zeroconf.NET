using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class TcpKeepAliveIntervalOption : Option
    {
        public TcpKeepAliveIntervalOption(uint seconds)
        {
            KeepaliveInterval = new TimeSpan(seconds * 10000);
        }
        internal static TcpKeepAliveIntervalOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new TcpKeepAliveIntervalOption(BinaryHelper.ReadUInt32(stream));
        }

        public TimeSpan KeepaliveInterval { get; set; }

        public override OptionType Type
        {
            get { return OptionType.TcpKeepaliveInterval; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(4);
            BinaryHelper.Write(stream, (uint)KeepaliveInterval.TotalSeconds);
        }
    }
}
