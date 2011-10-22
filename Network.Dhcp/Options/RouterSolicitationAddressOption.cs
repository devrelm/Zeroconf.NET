using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Network.Dhcp
{
    class RouterSolicitationAddressOption : Option
    {
        public RouterSolicitationAddressOption(IPAddress iPAddress)
        {
            this.RouterSolicitation = iPAddress;
        }
        internal static RouterSolicitationAddressOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new RouterSolicitationAddressOption(new IPAddress(BinaryHelper.Read(stream, 4)));
        }

        public IPAddress RouterSolicitation { get; set; }

        public override OptionType Type
        {
            get { return OptionType.RouterSolicitationAddress; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(4);
            BinaryHelper.Write(stream, RouterSolicitation.GetAddressBytes());
        }
    }
}
