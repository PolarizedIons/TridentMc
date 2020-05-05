using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using NetCoreServer;

namespace TridentMc.Networking
{
    public class Server : TcpServer
    {
        public ConcurrentDictionary<Guid, ServerSession> ConnectionSessions { get; private set; }

        public Server() : base(IPAddress.Parse(TridentSettings.BindHost), TridentSettings.BindPort)
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
