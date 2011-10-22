using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Dhcp
{
    [Flags]
    public enum NodeType : byte
    {
        BNode = 0x1,
        PNode = 0x2,
        MNode = 0x4,
        HNode = 0x8,
    }
    class NetbiosOverTcpIpNodeTypeOption : Option
    {
        public NetbiosOverTcpIpNodeTypeOption(NodeType nodeType)
        {
            NodeType = nodeType;
        }
        internal static NetbiosOverTcpIpNodeTypeOption Read(System.IO.Stream stream)
        {
            stream.ReadByte();

            return new NetbiosOverTcpIpNodeTypeOption((NodeType)stream.ReadByte());
        }

        public NodeType NodeType { get; set; }

        public override OptionType Type
        {
            get { return OptionType.NetbiosOverTcpIpNodeType; }
        }

        public override void WriteTo(System.IO.Stream stream)
        {
            
            stream.WriteByte(1);
            stream.WriteByte((byte)NodeType);
        }
    }
}
