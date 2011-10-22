using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class RootPathOption : StringOption
    {
        public string RootPath
        {
            get { return value; }
            set { this.value = value; }
        }

        public RootPathOption(string value)
            : base(value)
        {
        }
        internal static RootPathOption Read(System.IO.Stream stream)
        {
            return new RootPathOption(StringOption.Read(stream));
        }

        public override OptionType Type
        {
            get { return OptionType.RootPath; }
        }
    }
}
