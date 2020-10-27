using System.Text;
using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets
{
    public abstract class ClientBoundPacket
    {
        public abstract void Encode(PacketBuffer packet);
    }
}
