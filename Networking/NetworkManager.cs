using System;
using System.Collections.Generic;
using System.Threading;
using Serilog;
using TridentMc.Events;
using TridentMc.Events.Networking;
using TridentMc.Exceptions;
using TridentMc.Networking.Packets;
using TridentMc.Networking.Packets.DataTypes;
using TridentMc.Networking.State;

namespace TridentMc.Networking
{
    public class NetworkManager
    {

        private readonly Server _server;
        private Thread _networkThread;
        private readonly Packets.Packets _packets;
        private PacketResponders _responders;

        public ICollection<ServerSession> Sessions => _server.ConnectionSessions.Values;

        public NetworkManager()
        {
            _server = new Server();
            _packets = new Packets.Packets();
        }

        public void Start()
        {
            Log.Debug("Network Loop starting");
            _networkThread = new Thread(NetworkLoop);
            _networkThread.Start();
            
            _responders = new PacketResponders();
        }

        public void Stop()
        {
            _server.Stop();
        }

        private void NetworkLoop()
        {
            _server.Start();
            while (!TridentMc.Instance.State.IsShuttingDown)
            {
                Thread.Sleep(1);
                foreach (var session in Sessions)
                {
                    while (session.PacketBuffer.Count > 0)
                    {
                        var packet = session.PacketBuffer.Dequeue();

                        if (session.UsesCompression)
                        {
                            throw new NotYetImplemented("Compression isn't implemented yet");
                        }
                        else
                        {
                            var packetId = VarInt.Decode(packet, out packet);
                            Log.Debug("[{SessionId}] Packet {PacketId} with state being {State}", session.Id, packetId ,session.ConnectionState);
                            
                            IServerPacket serverPacketDecoder = _packets.Get<IServerPacket>(PacketDirection.ServerBound, session.ConnectionState, packetId);
                            if (serverPacketDecoder == null)
                            {
                                Log.Debug("[{SessionId}] -> Unknown packet", session.Id);
                                continue;
                            }

                            IServerPacket serverPacket = serverPacketDecoder.Decode(packet);

                            Type packetReceivedType = typeof(PacketReceived<>).MakeGenericType(serverPacket.GetType());
                            object[] args = {serverPacket, session};
                            TridentMc.Instance.EventManager.FireEvent((IEvent)Activator.CreateInstance(packetReceivedType, args));

                            if (serverPacket is IChangesState stateChangePacket)
                            {
                                Log.Debug("[{SessionId}] Changing state to {NewState}", session.Id, stateChangePacket.NextState);
                                session.ConnectionState = stateChangePacket.NextState;
                            }
                        }
                    }
                }
            }
        }
    }
}
