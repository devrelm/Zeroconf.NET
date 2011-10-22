using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    class ClientIdentifierOption : Option
    {
        public ClientIdentifierOption(byte[] p)
        {
            Identifier = p;
        }
        internal static ClassIdentifierOption Read(System.IO.Stream stream)
        {
            return new ClassIdentifierOption(BinaryHelper.Read(stream, stream.ReadByte()));
        }

        public override OptionType Type
        {
            get { return OptionType.ClassIdentifier; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte((byte)Identifier.Length);
            BinaryHelper.Write(stream, Identifier);
        }

        public byte[] Identifier { get; set; }
    }
}
