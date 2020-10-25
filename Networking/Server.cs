using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using NetCoreServer;

namespace TridentMc.Networking
{
    public class Server : TcpServer
    {
        public ConcurrentDictionary<Guid, ServerSession> ConnectionSessions { get; private set; }

        public Server(IConfiguration config) : base(IPAddress.Parse(config["Server:BindHost"]), int.Parse(config["Server:BindPort"]))
        {
            ConnectionSessions = new ConcurrentDictionary<Guid, ServerSession>();
        }

        protected override TcpSession CreateSession() { return new ServerSession(this); }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"TCP server caught an error with code {error}");
        }
    }
}
