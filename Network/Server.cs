using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Collections;
using System.Linq;

namespace Network
{
    public abstract class Server : IServiceProvider
    {
        public Server(params IPEndPoint[] hosts)
        {
            Hosts = hosts;
        }

        public IPEndPoint[] Hosts { get; protected set; }

        //protected TcpListener tcp;
        //protected UdpClient udp;
        protected Socket[] servers;
        public bool IsTcp { get; set; }
        public bool IsUdp { get; set; }
        public bool IsStarted { get; set; }

        #region Start

        [SocketPermission(System.Security.Permissions.SecurityAction.Assert)]
        private void Start()
        {
            if (IsStarted)
                throw new NotSupportedException("You cannot start the server twice");
            IDictionary<Socket, IPEndPoint> servers = new Dictionary<Socket, IPEndPoint>();
            foreach (var host in Hosts.Where(ep => !IsMulticast(ep.Address)))
            {
                Socket server = null;
                if (IsTcp)
                    server = new Socket(host.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                if (IsUdp)
                    server = new Socket(host.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                servers.Add(server, host);
            }
            this.servers = servers.Keys.ToArray();
            foreach (var host in Hosts.Where(ep => IsMulticast(ep.Address)))
            {
                foreach (var kvp in servers.Where(kvp => kvp.Value.AddressFamily == host.AddressFamily))
                {
                    kvp.Key.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    JoinMulticastGroup(kvp.Key, host.Address, 5);
                };
            }
            foreach (var kvp in servers)
                kvp.Key.Bind(kvp.Value);

            if (IsTcp)
            {
                foreach (var server in this.servers)
                    server.Listen(10);
            }
            OnStart();
            IsStarted = true;
            if (IsTcp)
            {
                foreach (var server in this.servers)
                    server.BeginAccept(ReceiveRequest, server);
            }
            if (IsUdp)
            {
                foreach (var server in this.servers)
                {
                    UdpAsyncState state = new UdpAsyncState();
                    state.buffer = new byte[65536];
                    state.server = server;
                    EndPoint remote = new IPEndPoint(server.AddressFamily == AddressFamily.InterNetwork ? IPAddress.Any : IPAddress.IPv6Any, 0); ;
                    server.BeginReceiveFrom(state.buffer, 0, state.buffer.Length, SocketFlags.None, ref remote, ReceiveRequest, state);
                }
            }
        }

        private class UdpAsyncState
        {
            public Socket server;
            public byte[] buffer;
        }

        public void StartTcp()
        {
            IsTcp = true;
            IsUdp = false;
            Start();
        }

        public void StartUdp()
        {
            IsTcp = false;
            IsUdp = true;
            Start();
        }

        private static void JoinMulticastGroup(Socket server, IPAddress multicastAddr, byte timeToLive)
        {
            MulticastOption optionValue = new MulticastOption(multicastAddr);
            server.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, optionValue);
            server.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, timeToLive);
        }

        #endregion

        public event EventHandler Started;
        public event EventHandler Stopped;

        private void ReceiveRequest(IAsyncResult result)
        {
            byte[] buffer = null;
            try
            {
                if (IsTcp)
                {
                    Socket client = ((Socket)result.AsyncState).EndAccept(result);

                    if (IsStarted)
                        ((Socket)result.AsyncState).BeginAccept(ReceiveRequest, null);

                    Treat(new NetworkStream(client), client.RemoteEndPoint as IPEndPoint);
                    if (IsStateLess)
                    {
                        client.Shutdown(SocketShutdown.Send);
                        client.Close();
                    }
                }
                if (IsUdp)
                {
                    EndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                    var state = ((UdpAsyncState)result.AsyncState);
                    int length = state.server.EndReceiveFrom(result, ref remote);
                    IPEndPoint client = (IPEndPoint)remote;
                    buffer = state.buffer;
                    byte[] bytes = new byte[length];
                    Buffer.BlockCopy(buffer, 0, bytes, 0, length);
                    if (IsStarted)
                    {
                        remote = new IPEndPoint(IPAddress.Any, 0);
                        state.server.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref remote, ReceiveRequest, result.AsyncState);
                    }
                    Treat(new MemoryStream(bytes), client);
                }

            }
            catch (ObjectDisposedException)
            {

            }
            catch (Exception)
            {
                if (IsStarted)
                {
                    if (IsTcp)
                        ((Socket)result.AsyncState).BeginAccept(ReceiveRequest, result.AsyncState);
                    if (IsUdp)
                    {
                        EndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                        if (buffer == null)
                            ((UdpAsyncState)result.AsyncState).buffer = buffer = new byte[65536];
                        ((UdpAsyncState)result.AsyncState).server.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref remote, ReceiveRequest, result.AsyncState);
                    }
                }
            }
        }

        protected abstract void Treat(Stream client, IPEndPoint remote);

        public void Stop()
        {
            if (IsStarted)
            {
                foreach (var server in servers)
                    server.Close();
                OnStop();
                IsStarted = false;
            }
        }

        public void Restart()
        {
            Stop();
            if (IsTcp)
                StartTcp();
            if (IsUdp)
                StartUdp();
        }

        protected virtual void OnStart()
        {
            if (Started != null)
                Started(this, EventArgs.Empty);
        }

        protected virtual void OnStop()
        {
            if (Stopped != null)
                Stopped(this, EventArgs.Empty);
        }

        static internal protected bool IsMulticast(IPAddress hostAddress)
        {
            if (hostAddress.IsIPv6Multicast)
                return true;
            byte[] addressBytes = hostAddress.GetAddressBytes();
            if (addressBytes[0] >= 224 && addressBytes[0] <= 239)
                return true;
            return false;
        }

        public bool IsStateLess { get; set; }

        protected IDictionary<Type, object> services = new Dictionary<Type, object>();

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            return services[serviceType];
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        #endregion
    }

    public abstract class Server<TRequest, TResponse> : Server
        where TResponse : IServerResponse, new()
        where TRequest : IServerRequest<TRequest>, new()
    {
        internal Server(Socket server, Client client)
            : this(((IPEndPoint)server.LocalEndPoint).Address, 0)
        {
            this.servers = new[] { server };
            IsStarted = server.IsBound;
            IsTcp = client.IsTcp;
            IsUdp = client.IsUdp;
        }

        public Server(ushort port)
            : this(IPAddress.Any, port)
        {

        }

        public Server(IPAddress address, ushort port)
            : this(new IPEndPoint(address, port))
        {

        }

        public Server(params IPEndPoint[] hosts)
            : base(hosts)
        {
        }

        public event EventHandler<RequestEventArgs<TRequest, TResponse>> RequestReceived;

        protected abstract RequestEventArgs<TRequest, TResponse> GetEventArgs(TRequest request);

        protected override void Treat(Stream client, IPEndPoint remote)
        {
            RequestEventArgs<TRequest, TResponse> rea = GetEventArgs(new TRequest().GetRequest(client));
            rea.Host = remote;
            Treat(rea, client);
        }

        protected virtual void Treat(RequestEventArgs<TRequest, TResponse> rea, Stream client)
        {
            OnRequestReceived(rea);
            if (rea.Response != null)
            {
                if (IsUdp && client is MemoryStream)
                {
                    Send(rea.Response, rea.Host);
                }
                else
                    Send(rea.Response, client);
            }
            client.Flush();
            if (IsStateLess)
                client.Close();
        }

        protected void Send(TResponse response, Stream client)
        {
            if (client.CanWrite)
                response.WriteTo(client);
        }

        public void Send(TResponse response, IPEndPoint client)
        {
            MemoryStream stream = new MemoryStream();
            Send(response, stream);
            foreach (var server in servers)
            {
                if (server.IsBound && server.AddressFamily == client.AddressFamily)
                    server.SendTo(stream.ToArray(), client);
            }
        }

        protected virtual void OnRequestReceived(RequestEventArgs<TRequest, TResponse> rea)
        {
            if (RequestReceived != null)
                RequestReceived(this, rea);
        }
    }
}
