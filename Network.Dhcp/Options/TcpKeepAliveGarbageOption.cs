using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class TcpKeepAliveGarbageOption : BooleanOption
    {
        public TcpKeepAliveGarbageOption(bool p)
            : base(p)
        {
        }
        internal static TcpKeepAliveGarbageOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new TcpKeepAliveGarbageOption(stream.ReadByte() == 1);
        }

        public uint KeepAliveInterval { get; set; }

        public override OptionType Type
        {
            get { return OptionType.TcpKeepAliveGarbage; }
        }
    }
}
