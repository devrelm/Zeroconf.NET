using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class MessageOption : Option
    {
        public MessageOption(OptionType[] options)
        {
            Options = options;
        }
        internal static MessageOption Read(System.IO.Stream stream)
        {
            int length = stream.ReadByte();
            OptionType[] options = new OptionType[length];
            for (int i = 0; i < length; i++)
                options[i] = (OptionType)stream.ReadByte();

            return new MessageOption(options);
        }

        public OptionType[] Options { get; set; }

        public override OptionType Type
        {
            get { return OptionType.Message; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte((byte)Options.Length);
            for (int i = 0; i < Options.Length; i++)
                stream.WriteByte((byte)Options[i]);
        }
    }
}
