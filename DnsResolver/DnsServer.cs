using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Network.Dns
{
    public class DnsServer : Server<Message, Message>
    {
        public DnsServer(ushort port)
            : base(port)
        {

        }

        public DnsServer(IPAddress address, ushort port)
            : base(address, port)
        {

        }

        public DnsServer(params IPEndPoint[] hosts)
            : base(hosts)
        {
        }



        public DnsServer()
            : this(53)
        {

        }
        protected override RequestEventArgs<Message, Message> GetEventArgs(Message request)
        {
            return new RequestEventArgs<Message, Message>() { Request = request };
        }
    }
}
