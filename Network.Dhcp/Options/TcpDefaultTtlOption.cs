using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class TcpDefaultTtlOption : Option
    {
        public TcpDefaultTtlOption(byte ttl)
        {
            Ttl = ttl;
        }
        internal static TcpDefaultTtlOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new TcpDefaultTtlOption((byte)stream.ReadByte());
        }

        public byte Ttl { get; set; }

        public override OptionType Type
        {
            get { return OptionType.TcpDefaultTtl; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(1);
            stream.WriteByte(Ttl);
        }
    }
}
