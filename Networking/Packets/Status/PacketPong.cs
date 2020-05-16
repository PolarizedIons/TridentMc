using TridentMc.Networking.Packets.DataTypes;
using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets.Status
{
    public class PacketPong : ClientPacket
    {
        public override int Id => 1;

        public long Payload { get; private set; }

        public PacketPong(long payload)
        {
            Payload = payload;
        }

        public override PacketBuffer Encode()
        {
            var buffer = new PacketBuffer();
            buffer.WriteLong(Payload);

            return buffer;
        }
    }
}
