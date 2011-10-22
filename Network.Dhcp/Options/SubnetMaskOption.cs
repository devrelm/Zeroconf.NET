using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Network.Dhcp
{
    public class SubnetMaskOption : Option
    {

        public SubnetMaskOption(IPAddress iPAddress)
        {
            // TODO: Complete member initialization
            this.IpAddress = iPAddress;
        }
        internal static SubnetMaskOption Read(System.IO.Stream stream)
        {
            //Useless Length
            stream.ReadByte();
            return new SubnetMaskOption(new IPAddress(BinaryHelper.Read(stream, 4)));
        }

        public override OptionType Type
        {
            get { return OptionType.SubnetMask; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            BinaryHelper.Write(stream, (uint)4);
            BinaryHelper.Write(stream, IpAddress.GetAddressBytes());
        }

        public IPAddress IpAddress { get; set; }
    }
}
