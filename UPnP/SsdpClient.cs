using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network.Dns;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Network.ZeroConf;
using System.Xml;
using Network.Rest;
using Network.UPnP;

namespace Network.UPnP
{
    class SsdpClient : Client<HttpRequest, HttpResponse>
    {
        public static readonly IPEndPoint EndPoint = new IPEndPoint(IPAddress.Parse("239.255.255.250"), 1900);

        private IPEndPoint local;

        public SsdpClient(ushort port)
            : base(new NetworkConfig(true, false, true))
        {
            StartUdp(IPAddress.Any, port);
        }

        public void Resolve(string protocol)
        {
            IsUdp = true;
            HttpRequest request = new HttpRequest();
            request.Method = "M-SEARCH";
            request.Host = EndPoint.ToString();
            request.Headers.Add("ST", protocol);
            request.Headers.Add("MAN", "\"ssdp:discover\"");
            request.Headers.Add("MX", "3");
            Send(request, EndPoint);
        }

        public static SsdpClient CreateAndResolve(string protocol)
        {
            SsdpClient client = new SsdpClient(0);
            client.Resolve(protocol);
            return client;
        }

        protected override ClientEventArgs<HttpRequest, HttpResponse> GetEventArgs(HttpResponse response)
        {
            return new HttpClientEventArgs(response);
        }
    }
}
