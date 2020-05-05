using System;
using System.Collections.Generic;
using System.Linq;
using TridentMc.Networking.Packets.Handshaking;
using TridentMc.Networking.Packets.Status;
using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets
{
    public interface IPacket
    {
        ConnectionState ConnectionState { get; }
        int Id { get; }
        PacketDirection DirectionTo { get; }
    }

    public interface IServerPacket : IPacket
    {
        PacketDirection IPacket.DirectionTo => PacketDirection.ServerBound;

        IServerPacket Decode(byte[] data);
    }

    public interface IClientPacket : IPacket
    {
        PacketDirection IPacket.DirectionTo => PacketDirection.ClientBound;

        byte[] Encode(IClientPacket packet);
    }

    public interface IChangesState
    {
        ConnectionState NextState { get; }
    }

    class Packets
    {
        public static List<IPacket> PacketsList { get; private set; }

        public Packets()
        {
            PacketsList = new List<IPacket>();

            // Handshaking
            PacketsList.Add(new PacketHandshaking());
            
            // Status
            PacketsList.Add(new PacketRequest());
            PacketsList.Add(new PacketResponse());
            PacketsList.Add(new PacketPing());
            PacketsList.Add(new PacketPong());
        }

        public T Get<T>(PacketDirection directionTo, ConnectionState state, int id) where T : IPacket
        {
            return (T) PacketsList.Find(packet => packet.DirectionTo == directionTo && packet.Id == id && packet.ConnectionState == state);
        }
    }
}
