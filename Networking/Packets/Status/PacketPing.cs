using Microsoft.VisualBasic.CompilerServices;
using TridentMc.Networking.Packets.DataTypes;
using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets.Status
{
    public class PacketPing : ServerPacket
    {
        public long Payload { get; private set; }

        public PacketPing(PacketBuffer buffer)
        {
            Payload = buffer.ReadLong();
        }

        public override void Handle(ServerSession session)
        {
            session.SendPacket(new PacketPong(Payload));
        }
    }
}
