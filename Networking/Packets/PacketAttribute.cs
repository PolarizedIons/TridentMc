using System;
using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets
{
    public class PacketAttribute : Attribute
    {
        public PacketDirection Direction { get; }
        public ConnectionState State { get; }
        public int PacketId { get; }

        public PacketAttribute(PacketDirection direction, ConnectionState state, int packetId)
        {
            Direction = direction;
            State = state;
            PacketId = packetId;
        }
    }

    public class ServerPacket : PacketAttribute
    {
        public ServerPacket(ConnectionState state, int packetId) : base(PacketDirection.ServerBound, state, packetId)
        {
        }
    }
    
    public class ClientPacket : PacketAttribute
    {
        public ClientPacket(ConnectionState state, int packetId) : base(PacketDirection.ClientBound, state, packetId)
        {
        }
    }
}
