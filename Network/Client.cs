using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Network
{
    public abstract class Client
    {
        protected Client(Socket s, Server server)
        {
            this.client = s;
            dataStream = new NetworkStream(s, true);
            IsUdp = server.IsUdp;
            IsTcp = server.IsTcp;
        }

        public Client(bool isStateLess)
            : this(new NetworkConfig(isStateLess))
        {
        }

        public Client(NetworkConfig config)
        {
            this.Configuration = config;
        }

        public void StartTcp(IPEndPoint host)
        {
            Host = host;
            StartTcp();
        }

        public void StartTcp()
        {
            if (!Configuration.SupportsTcp)
                throw new NotSupportedException("This client is not meant for Tcp");
            if (Host == null)
                throw new NotSupportedException("The host is not initialized.");
            IsTcp = true;
            IsUdp = false;
        }

        public void StartTcp(IPAddress address, ushort port)
        {
            StartTcp(new IPEndPoint(address, port));
        }

        public void StartUdp(IPAddress address, ushort port)
        {
            StartUdp(new IPEndPoint(address, port));
        }

        public void StartUdp(IPEndPoint host)
        {
            Host = host;
            StartUdp();
        }

        public void StartUdp()
        {
            if (!Configuration.SupportsUdp)
                throw new NotSupportedException("This client is not meant for UDP");
            if (Host == null)
                throw new NotSupportedException("The host is not initialized.");
            IsTcp = false;
            IsUdp = true;
        }


        public IPEndPoint Host { get; protected set; }
        public NetworkConfig Configuration { get; private set; }
        protected Socket client;
        protected internal Stream dataStream;

        protected Stream DataStream { get { return dataStream; } }


        public bool IsTcp { get; set; }
        public bool IsUdp { get; set; }

        protected void JoinMulticastGroup(IPAddress multicastAddr, byte timeToLive)
        {
            MulticastOption optionValue;
            optionValue = new MulticastOption(multicastAddr);
            client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, optionValue);
            client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, timeToLive);
        }

        protected bool expectMultipleResponses = false;

    }

    public abstract class Client<TRequest, TResponse> : Client
        where TResponse : class,IClientResponse<TResponse>, new()
        where TRequest : class,IClientRequest, new()
    {
        protected Client(Socket s, Server server)
            : base(s, server)
        {
        }

        public Client(bool isStateLess)
            : base(isStateLess)
        {
        }

        public Client(NetworkConfig configuration)
            : base(configuration)
        {
        }

        public event EventHandler<ClientEventArgs<TRequest, TResponse>> ResponseReceived;

        public void SendOneWay(TRequest request, IPEndPoint endpoint)
        {
            SendOneWayInternal(request, endpoint);
            if (Configuration.IsStateLess && IsTcp)
            {
                client.Disconnect(false);
                client = null;
            }

        }

        private void SendOneWayInternal(TRequest request, IPEndPoint endpoint)
        {
            if (client == null)
            {
                if (IsTcp)
                    client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                if (IsUdp)
                    client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                if (Server.IsMulticast(endpoint.Address))
                {
                    client.Bind(Host);
                    if (ResponseReceived != null)
                    {
                        JoinMulticastGroup(endpoint.Address, 5);
                        expectMultipleResponses = true;
                    }
                }
                else
                    client.Connect(endpoint);
                if (Host == null)
                    Host = client.LocalEndPoint as IPEndPoint;
                if (!IsUdp)
                    dataStream = new NetworkStream(client);
            }
            if (dataStream != null)
            {
                request.WriteTo(dataStream);
            }
            else
            {
                if (client.Connected)
                    client.Send(request.GetBytes());
                else
                    client.SendTo(request.GetBytes(), endpoint);
            }
        }

        public TResponse Send(TRequest request)
        {
            return Send(request, Host);
        }

        public TResponse Send(TRequest request, IPEndPoint endpoint)
        {
            SendOneWayInternal(request, endpoint);
            if (expectMultipleResponses && !Configuration.IsOneWayOnly)
            {
                StartReceive(request);
                return default(TResponse);
            }
            else
                return ReceiveResponse(request);
        }

        private void StartReceive(TRequest request)
        {
            Thread t = new Thread(new ParameterizedThreadStart(Receive));
            try
            {
                t.Start(request);
            }
            catch (SocketException)
            {
            }
        }

        private void Receive(object request)
        {
            while (expectMultipleResponses)
            {
                OnResponseReceived(ReceiveResponse((TRequest)request));
            }
        }

        public void Stop()
        {
            expectMultipleResponses = false;
            client.Close();
        }

        protected abstract ClientEventArgs<TRequest, TResponse> GetEventArgs(TResponse response);

        private void OnResponseReceived(TResponse response)
        {
            if (ResponseReceived != null && response != null)
                ResponseReceived(this, GetEventArgs(response));
        }

        private TResponse ReceiveResponse(TRequest request)
        {
            if (Configuration.IsOneWayOnly)
            {
                client.Disconnect(false);
                client = null;
                return default(TResponse);
            }

            Stream stream;
            TResponse result;
            try
            {
                if (dataStream == null)
                {
                    byte[] buffer = new byte[1024];
                    MemoryStream ms = new MemoryStream();
                    do
                    {
                        ms.Write(buffer, 0, client.Receive(buffer));
                    }
                    while (client.Available > 0);
                    ms.Position = 0;
                    stream = ms;
                }
                else
                    stream = dataStream;

                TResponse tempResponse = request as TResponse;
                if (tempResponse == null)
                    tempResponse = new TResponse();

                result = tempResponse.GetResponse(stream);
                if (dataStream == null)
                {
                    //Ensure not multiple responses were not read at once
                    while (stream.Position != stream.Length)
                    {
                        OnResponseReceived(result);

                        result = tempResponse.GetResponse(stream);
                    }

                }
            }
            catch (ObjectDisposedException)
            {
                return default(TResponse);
            }
            if (Configuration.IsStateLess && IsTcp)
            {
                client.Disconnect(false);
                client = null;
            }
            return result;
        }
    }
}