using System;
using System.Collections.Generic;
using TridentMc.Exceptions;
using TridentMc.Networking.Packets.Handshaking;
using TridentMc.Networking.Packets.Status;
using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets
{
    public class PacketManager
    {
        private Dictionary<ConnectionState, Dictionary<int, Type>> _serverPackets;

        public PacketManager()
        {
            _serverPackets = new Dictionary<ConnectionState, Dictionary<int, Type>>();
            
            Put(ConnectionState.Handshaking, 0, typeof(PacketHandshaking));
            
            Put(ConnectionState.Status, 0, typeof(PacketRequest));
            Put(ConnectionState.Status, 1, typeof(PacketPing));
        }

        private void Put(ConnectionState state, int id, Type packetType)
        {
            if (!typeof(ServerPacket).IsAssignableFrom(packetType))
            {
                throw new InvalidServerPacket();
            }

            if (!_serverPackets.ContainsKey(state))
            {
                _serverPackets.Add(state, new Dictionary<int, Type>());
            }

            _serverPackets[state].Add(id, packetType);
        }

        public ServerPacket CreateServerPacket(ConnectionState state, int id, byte[] bytes)
        {
            var packetBuffer = new PacketBuffer(bytes);
            var packetType = _serverPackets[state][id];

            if (packetType == null)
            {
                return null;
            }

            var constructor = packetType.GetConstructor(new[] {typeof(PacketBuffer)});
            if (constructor == null)
            {
                throw new InvalidServerPacket();
            }

            return (ServerPacket) constructor.Invoke(new object[] { packetBuffer });
        }
    }
}
