using TridentMc.Networking;
using TridentMc.Networking.Packets;

namespace TridentMc.Events.Networking
{
    public class PacketReceived<T>: IEvent where T : ServerPacket
    {
        public T Packet { get; private set; }
        public ServerSession Session { get; private set; }

        public PacketReceived(T packet, ServerSession session)
        {
            Packet = packet;
            Session = session;
        }
    }
}
