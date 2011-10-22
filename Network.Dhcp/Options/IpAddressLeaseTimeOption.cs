using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class IpAddressLeaseTimeOption : Option
    {
        public IpAddressLeaseTimeOption(uint p)
        {
            LeaseTime = new TimeSpan((long)p * 10000);
        }
        internal static IpAddressLeaseTimeOption Read(System.IO.Stream stream)
        {
            return new IpAddressLeaseTimeOption(BinaryHelper.ReadUInt32(stream));
        }

        public TimeSpan LeaseTime { get; set; }

        public override OptionType Type
        {
            get { return OptionType.IpAddressLeaseTime; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(4);
            BinaryHelper.Write(stream, (uint)LeaseTime.TotalSeconds);
        }
    }
}
