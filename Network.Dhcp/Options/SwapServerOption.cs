using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Network.Dhcp
{
    class SwapServerOption : Option
    {
        public SwapServerOption(IPAddress value)
        {
            SwapIp = value;
        }
        internal static SwapServerOption Read(System.IO.Stream stream)
        {
            return new SwapServerOption(new IPAddress(BinaryHelper.Read(stream, 4)));
        }

        public override OptionType Type
        {
            get { return OptionType.SwapServer; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(4);
            BinaryHelper.Write(stream, SwapIp.GetAddressBytes());
        }

        public IPAddress SwapIp { get; set; }
    }
}
