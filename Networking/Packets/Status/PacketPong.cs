using TridentMc.Networking.Packets.DataTypes;
using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets.Status
{
    public class PacketPong : IClientPacket
    {
        public ConnectionState ConnectionState => ConnectionState.Status; 
        public int Id => 0x01;

        public long Payload { get; private set; }

        public PacketPong()
        {
        }

        public PacketPong(long payload)
        {
            Payload = payload;
        }

        public byte[] Encode(IClientPacket packet)
        {
            return Long.Encode(Payload);
        }
    }
}
