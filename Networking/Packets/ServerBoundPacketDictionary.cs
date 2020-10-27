using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TridentMc.Extentions;
using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets
{
    public class ServerBoundPacketDictionary : ISingletonDiService
    {
        private readonly Dictionary<(ConnectionState, int), Type> _dictionary = new Dictionary<(ConnectionState, int), Type>();
        private readonly IServiceProvider _services;

        public ServerBoundPacketDictionary(IServiceProvider services)
        {
            _services = services;

            foreach (var packetType in typeof(ServerBoundPacket).FindAllInAssembly())
            {
                var packetAttr = packetType.GetPacketAttribute();
                if (packetAttr == null)
                {
                    Log.Warning("Packet {PacketType} doesn't have a packet attribute!");
                    continue;
                }

                _dictionary[(packetAttr.State, packetAttr.PacketId)] = packetType;
            }
        }

        public T GetPacket<T>(ConnectionState state, int packetId, PacketBuffer packetBuffer) where T : class
        {
            var type = _dictionary[(state, packetId)];
            var args = new object[] { _services, packetBuffer };
            return Activator.CreateInstance(type, args) as T;
        }
    }
}
