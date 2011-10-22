using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Network.Rtmp
{
    class HandshakeClient : Client<Handshake, Handshake>
    {
        public HandshakeClient()
            : base(false)
        {

        }

        public static void Main()
        {
            Handshake hs = new Handshake();
            Client<Handshake, Handshake> client = new HandshakeClient();
            client.StartTcp(IPAddress.Parse("199.93.39.126"), 443);
            var serverHs = client.Send(hs);
            hs.Version = byte.MaxValue;
            hs.Time2 = hs.Time;
            hs.Time = serverHs.Time;
            hs.Random = serverHs.Random;
            serverHs = client.Send(hs);
            client.Stop();
        }

        protected override ClientEventArgs<Handshake, Handshake> GetEventArgs(Handshake response)
        {
            return new ClientEventArgs<Handshake, Handshake>() { Response = response };
        }
    }
}
