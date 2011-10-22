using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class PolicyFilterOption : IPRangeOption
    {
        public PolicyFilterOption(System.Net.IPAddress[] addressesAndMasks)
            : base(null)
        {
            Addresses = Masks = new System.Net.IPAddress[addressesAndMasks.Length / 2];

            for (int i = 0; i < addressesAndMasks.Length; i++)
            {
                if (i % 2 == 0)
                    Addresses[i / 2] = addressesAndMasks[i];
                else
                    Addresses[(i - 1) / 2] = addressesAndMasks[i];
            }
        }


        internal static PolicyFilterOption Read(System.IO.Stream stream)
        {
            return new PolicyFilterOption(IPRangeOption.Read(stream));
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            if (Addresses == null || Masks == null || Addresses.Length != Masks.Length)
                throw new NotSupportedException("Addresses and Masks do not have the same length");
            stream.WriteByte((byte)(Addresses.Length + Masks.Length));

            for (int i = 0; i < Addresses.Length; i++)
            {
                BinaryHelper.Write(stream, Addresses[i].GetAddressBytes());
                BinaryHelper.Write(stream, Masks[i].GetAddressBytes());

            }
        }

        public override OptionType Type
        {
            get { return OptionType.PolicyFilter; }
        }

        public System.Net.IPAddress[] Masks { get; set; }
    }
}
