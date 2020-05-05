using TridentMc.Networking.Packets.Handshaking;
using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets.Status
{
    public class PacketRequest : IServerPacket
    {
        public ConnectionState ConnectionState => ConnectionState.Status;
        public int Id => 0x00;

        public IServerPacket Decode(byte[] data)
        {
            return new PacketRequest();
        }
    }
}
