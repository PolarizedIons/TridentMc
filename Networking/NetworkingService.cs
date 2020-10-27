using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using TridentMc.Events;
using TridentMc.Events.Network;
using TridentMc.Extentions;
using TridentMc.Extentions.PacketDataTypes;
using TridentMc.Networking.Packets;

namespace TridentMc.Networking
{
    public class NetworkingService : ISingletonDiService
    {
        private readonly Server _server;
        private readonly EventService _eventService;
        private readonly ServerBoundPacketDictionary _serverBoundPackets;
        private Thread _networkingThread;
        private IEnumerable<ServerConnection> Sessions => _server.ConnectionSessions.Values;
        private CancellationToken _cancellationToken;

        public NetworkingService(IConfiguration config, ServerBoundPacketDictionary serverBoundPackets, EventService eventService)
        {
            _server = new Server(config);
            _serverBoundPackets = serverBoundPackets;
            _eventService = eventService;
        }

        public void Start(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;

            _server.Start();
            _networkingThread = new Thread(NetworkLoop);
            _networkingThread.Start();
        }

        public void Stop()
        {
            _server.Stop();
        }

        private void NetworkLoop()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                var processes = new List<Task>();
                foreach (var session in Sessions)
                {
                    processes.Add(Task.Run(() =>
                    {
                        // We do it this way so one client can't constantly send packets and lock up the rest of the
                        // clients' processes
                        var packetCount = session.PacketsEnqueued;
                        for (var i = 0; i < packetCount; i++)
                        {
                            ProcessPacket(session, session.DequeuePacket());
                        }
                    }, _cancellationToken));
                }

                Task.WaitAll(processes.ToArray());
            }
        }

        private void ProcessPacket(ServerConnection connection, byte[] packetBytes)
        {
            var packetId = packetBytes.DecodeVarInt(out packetBytes);
            Log.Verbose("[{ConnectionId}] Received packet {PacketId} in {State} state", connection.Id, packetId ,connection.ConnectionState);

            var packetBuffer = new PacketBuffer
            {
                Buffer = packetBytes
            };
            var packet = _serverBoundPackets.GetPacket<ServerBoundPacket>(connection.ConnectionState, packetId, packetBuffer);

            if (packet == null)
            {
                Log.Warning("[{ConnectionId}] Unknown packet id {Id} in state {state} received, disconnecting client", connection.Id, packetId, connection.ConnectionState);
                connection.Disconnect();
                return;
            }

            if (packetBuffer.Any)
            {
                Log.Warning("Packet {Id} has {Length} bytes left over after reading!", packetId, packetBuffer.Length);
                connection.Disconnect();
                return;
            } 

            var packetReceivedType = typeof(PacketReceived<>).MakeGenericType(packet.GetType());
            var args = new object[] { packet, connection };
            _eventService.EnqueueEvent((IEvent)Activator.CreateInstance(packetReceivedType, args));
            
            if (packet is IChangesProtocolState stateChangePacket)
            {
                Log.Debug("[{ConnectionId}] Changing connection state to {NewState}", connection.Id, stateChangePacket.NewState);
                connection.ConnectionState = stateChangePacket.NewState;
            }

            packet.Handle(connection);
        }
    }
}
