using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets.Status
{
    [ClientPacket(ConnectionState.Status, 0x01)]
    public class Pong : ClientBoundPacket
    {
        public long Payload { get; set; }
        
        public override void Encode(PacketBuffer packet)
        {
            packet.WriteLong(Payload);
        }
    }
}
