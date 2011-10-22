using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class DefaultIpTtlOption : Option
    {
        public DefaultIpTtlOption(byte ttl)
        {
            Ttl = ttl;
        }
        internal static DefaultIpTtlOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new DefaultIpTtlOption((byte)stream.ReadByte());
        }

        public override OptionType Type
        {
            get { return OptionType.DefaultIpTtl; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(2);
            BinaryHelper.Write(stream, Ttl);
        }

        public byte Ttl { get; set; }
    }
}
