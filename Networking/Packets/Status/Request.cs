using System;
using Microsoft.Extensions.DependencyInjection;
using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets.Status
{
    [ServerPacket(ConnectionState.Status, 0x00)]
    public class Request : ServerBoundPacket
    {
        public Request(IServiceProvider services, PacketBuffer packet) : base(services, packet)
        {
            // No fields
        }

        public override void Handle(ServerConnection connection)
        {
            connection.SendPacket(new Response());
        }
    }
}
