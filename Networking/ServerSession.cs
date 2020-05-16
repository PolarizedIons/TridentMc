using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using NetCoreServer;
using Serilog;
using TridentMc.Events;
using TridentMc.Events.Networking;
using TridentMc.Exceptions;
using TridentMc.Networking.Packets;
using TridentMc.Networking.Packets.DataTypes;
using TridentMc.Networking.State;

namespace TridentMc.Networking
{
    public class ServerSession : TcpSession
    {
        private readonly Server _server;

        public ConnectionState ConnectionState { get; set; }
        public bool UsesCompression { get; set; }

        public Queue<byte[]> PacketBuffer { get; private set; }

        public ServerSession(Server server) : base(server)
        {
            _server = server;
        }

        protected override void OnConnected()
        {
            Log.Debug("[{SessionId}] Connected", Id);
            _server.ConnectionSessions[Id] = this;

            ConnectionState = ConnectionState.Handshaking;
            UsesCompression = false;
            PacketBuffer = new Queue<byte[]>();
        }

        protected override void OnDisconnected()
        {
            Log.Debug("[{SessionId}] Disconnected", Id);
            _server.ConnectionSessions.Remove(Id, out _);
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            byte[] remaining = new byte[size];
            Array.Copy(buffer, remaining, size);
            Log.Debug("[{SessionId}] RECV: {Bytes}", Id, remaining.GetPrintableBytes());

            // Legacy packet
            if (remaining[0] == 0xfe)
            {
                Disconnect();
                return;
            }

            while (true)
            {
                int packetLength = VarInt.Decode(remaining, out remaining);
                byte[] packet = new byte[packetLength];
                Array.Copy(remaining, packet, packetLength);
                PacketBuffer.Enqueue(packet);

                remaining = remaining.Skip(packetLength).ToArray();
                if (remaining.Length == 0)
                {
                    break;
                }
            }
        }

        protected override void OnError(SocketError error)
        {
            Log.Debug("[{SessionId}] Had an error! {Error}", Id, error);
            _server.ConnectionSessions.Remove(Id, out _);
        }

        public void SendPacket(ClientPacket packet)
        {
            if (UsesCompression)
            {
                throw new NotYetImplemented("Compression isn't implemented yet");
            }
            else
            {
                var packetId = VarInt.Encode(packet.Id);
                var encoded = packet.Encode().Bytes;
                var length = VarInt.Encode(packetId.Length + encoded.Length);
                var data = length.Concat(packetId).Concat(encoded).ToArray();
                Log.Debug("[{SessionId}] SEND: {Bytes}", Id, data.GetPrintableBytes());
                Send(data);
            }

            Type packetReceivedType = typeof(PacketSent<>).MakeGenericType(packet.GetType());
            object[] args = {packet, this};
            TridentMc.Instance.EventManager.FireEvent((IEvent)Activator.CreateInstance(packetReceivedType, args));
        }
    }
}
