using System;
using Microsoft.Extensions.DependencyInjection;

namespace TridentMc.Networking.Packets
{
    public abstract class ServerBoundPacket 
    {
        protected readonly IServiceProvider Services;

        protected ServerBoundPacket(IServiceProvider services, PacketBuffer packet)
        {
            Services = services;
        }

        public abstract void Handle(ServerConnection connection);
    }
}
