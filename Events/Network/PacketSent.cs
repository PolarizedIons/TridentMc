using TridentMc.Networking;
using TridentMc.Networking.Packets;

namespace TridentMc.Events.Network
{
    public class PacketSent<T> : IEvent where T : ServerPacket
    {
        public T Packet { get; }
        public ServerConnection Connection { get; }

        public PacketSent(T packet, ServerConnection connection)
        {
            Packet = packet;
            Connection = connection;
        }
    }
}
