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
        private PacketManager _packetManager;

        public ICollection<ServerSession> Sessions => _server.ConnectionSessions.Values;

        public NetworkManager()
        {
            _server = new Server();
            _packetManager = new PacketManager();
        }

        public void Start()
        {
            Log.Debug("Network Loop starting");
            _networkThread = new Thread(NetworkLoop);
            _networkThread.Start();
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
                            
                            var serverPacket = _packetManager.CreateServerPacket(session.ConnectionState, packetId, packet);
                            if (serverPacket == null)
                            {
                                Log.Debug("[{SessionId}] -> Unknown packet", session.Id);
                                session.Disconnect();
                                break;
                            }

                            Type packetReceivedType = typeof(PacketReceived<>).MakeGenericType(serverPacket.GetType());
                            object[] args = {serverPacket, session};
                            TridentMc.Instance.EventManager.FireEvent((IEvent)Activator.CreateInstance(packetReceivedType, args));

                            if (serverPacket is IChangesState stateChangePacket)
                            {
                                Log.Debug("[{SessionId}] Changing state to {NewState}", session.Id, stateChangePacket.NextState);
                                session.ConnectionState = stateChangePacket.NextState;
                            }

                            serverPacket.Handle(session);
                        }
                    }
                }
            }
        }
    }
}
