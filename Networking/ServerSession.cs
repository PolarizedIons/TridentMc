using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using NetCoreServer;
using Serilog;
using TridentMc.Extentions;
using TridentMc.Extentions.PacketDataTypes;
using TridentMc.Networking.State;

namespace TridentMc.Networking
{
    public class ServerSession : TcpSession
    {
        private readonly Server _server;
        private Queue<byte[]> _packetBuffer = new Queue<byte[]>();

        public ConnectionState ConnectionState { get; private set; } = ConnectionState.Handshaking;
        public int PacketsEnqueued => _packetBuffer.Count();

        public ServerSession(Server server) : base(server)
        {
            _server = server;
        }

        protected override void OnConnected()
        {
            Log.Debug("[{SessionId}] New connection from {Ip}:{Port}", 
                Id,
                (Socket.RemoteEndPoint as IPEndPoint)?.Address,
                (Socket.RemoteEndPoint as IPEndPoint)?.Port
            );
            _server.ConnectionSessions[Id] = this;
        }

        protected override void OnDisconnected()
        {
            Log.Debug("[{SessionId}] Disconnected", Id); 
            _server.ConnectionSessions.TryRemove(Id, out _);
        }

        protected override void OnError(SocketError error)
        {
            Log.Debug("[{SessionId}] Had an error! {Error}", Id, error);
            _server.ConnectionSessions.TryRemove(Id, out _);
            Disconnect();
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            var remaining = new byte[size];
            Array.Copy(buffer, remaining, size);
            Log.Verbose("[{SessionId}] RECV: {Bytes}", Id, remaining.GetPrintableBytes());

            // Legacy packet
            if (remaining[0] == 0xfe)
            {
                Disconnect();
                return;
            }

            while (remaining.Length != 0)
            {
                var packetLength = remaining.DecodeVarint(out remaining);
                var packet = new byte[packetLength];
                Array.Copy(remaining, packet, packetLength);
                _packetBuffer.Enqueue(packet);

                remaining = remaining.Skip(packetLength).ToArray();
            }
        }

        public byte[] DequeuePacket()
        {
            return _packetBuffer.Dequeue();
        }
    }
}
