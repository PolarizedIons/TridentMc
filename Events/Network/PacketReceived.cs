using TridentMc.Networking;
using TridentMc.Networking.Packets;

namespace TridentMc.Events.Network
{
    public class PacketReceived<T> : IEvent where T : ServerBoundPacket
    {
        public T Packet { get; }
        public ServerConnection Connection { get; }

        public PacketReceived(T packet, ServerConnection connection)
        {
            Packet = packet;
            Connection = connection;
        }
    }
}
