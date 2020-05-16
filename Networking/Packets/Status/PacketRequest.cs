using TridentMc.Networking.Packets.Handshaking;
using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets.Status
{
    public class PacketRequest : ServerPacket
    {
        public PacketRequest(PacketBuffer buffer)
        {
            // NOOP
        }

        public override void Handle(ServerSession session)
        {
            session.SendPacket(PacketResponse.FromSystem());
        }
    }
}
