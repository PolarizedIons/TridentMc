using System;
using Microsoft.Extensions.DependencyInjection;
using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets.Status
{
    [ServerPacket(ConnectionState.Status, 0x01)]
    public class Ping : ServerBoundPacket
    {
        public long Payload { get; set; }

        public Ping(IServiceProvider services, PacketBuffer packet) : base(services, packet)
        {
            Payload = packet.ReadLong();
        }

        public override void Handle(ServerConnection connection)
        {
            connection.SendPacket(new Pong
            {
                Payload = Payload
            });
        }
    }
}
