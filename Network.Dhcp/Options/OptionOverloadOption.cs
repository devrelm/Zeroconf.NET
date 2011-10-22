using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    [Flags]
    public enum Overloads : byte
    {
        FileFieldUsed = 1,
        SNameFieldUsed = 2,
        BotFieldsUsed = 3,
    }
    class OptionOverloadOption : Option
    {
        public OptionOverloadOption(Overloads p)
        {
            Value = p;
        }
        internal static OptionOverloadOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();
            return new OptionOverloadOption((Overloads)stream.ReadByte());
        }

        public Overloads Value { get; set; }

        public override OptionType Type
        {
            get { return OptionType.OptionOverload; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(1);
            stream.WriteByte((byte)Value);
        }
    }
}
