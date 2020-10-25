using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using TridentMc.Extentions;
using TridentMc.Extentions.PacketDataTypes;

namespace TridentMc.Networking
{
    public class NetworkingService : ISingletonDiService
    {
        private readonly Server _server;
        private Thread _networkingThread;
        private CancellationToken _cancellationToken;
        private ICollection<ServerSession> Sessions => _server.ConnectionSessions.Values;

        public NetworkingService(IConfiguration config)
        {
            _server = new Server(config);
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

        private void ProcessPacket(ServerSession session, byte[] packetBytes)
        {
            var packetId = packetBytes.DecodeVarint(out packetBytes);
            Log.Verbose("[{SessionId}] Received packet {PacketId} in {State} state", session.Id, packetId ,session.ConnectionState);

        }
    }
}
