using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Network.Dhcp
{
    class StaticRouteOption : IPRangeOption
    {
        public IPAddress[] Routers { get; set; }

        public StaticRouteOption(IPAddress[] destinations, IPAddress[] routers)
            : base(destinations)
        {
            this.Routers = routers;
        }

        internal static StaticRouteOption Read(System.IO.Stream stream)
        {
            IPAddress[] addresses = IPRangeOption.Read(stream);
            IPAddress[] destinations = new IPAddress[addresses.Length / 2];
            IPAddress[] routers = new IPAddress[addresses.Length / 2];
            for (int i = 0; i < addresses.Length; i++)
            {
                if (i % 2 == 0)
                    destinations[i / 2] = addresses[i];
                else
                    routers[(i - 1) / 2] = addresses[i];
            }
            return new StaticRouteOption(destinations, routers);
        }

        public override OptionType Type
        {
            get { return OptionType.StaticRouteOption; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte((byte)(Addresses.Length * 2));

            for (int i = 0; i < Addresses.Length; i++)
            {
                BinaryHelper.Write(stream, Addresses[i].GetAddressBytes());
                BinaryHelper.Write(stream, Routers[i].GetAddressBytes());
            }
        }
    }
}
