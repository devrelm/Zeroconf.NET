using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class VendorSpecificationInformationOption : StringOption
    {
        public string Information
        {
            get { return value; }
            set { this.value = value; }
        }

        public VendorSpecificationInformationOption(string p)
            : base(p)
        {
        }
        internal static VendorSpecificationInformationOption Read(System.IO.Stream stream)
        {
            return new VendorSpecificationInformationOption(StringOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.VendorSpecificInformation; }
        }
    }
}
