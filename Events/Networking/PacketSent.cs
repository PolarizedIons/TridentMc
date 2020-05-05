using TridentMc.Networking;
using TridentMc.Networking.Packets;

namespace TridentMc.Events.Networking
{
    public class PacketSent<T> : IEvent where T : IClientPacket
    {
        public T Packet { get; private set; }
        public ServerSession Session { get; private set; }

        public PacketSent(T packet, ServerSession session)
        {
            Packet = packet;
            Session = session;
        }
    }
}
