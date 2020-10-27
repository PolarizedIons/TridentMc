using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets.Status
{
    [ClientPacket(ConnectionState.Status, 0x00)]
    public class Response : ClientBoundPacket
    {
        public override void Encode(PacketBuffer packet)
        {
            packet.WriteString("{\"version\":{\"name\":\"1.8.7\",\"protocol\":47},\"players\":{\"max\":100,\"online\":5,\"sample\":[{\"name\":\"thinkofdeath\",\"id\":\"4566e69f-c907-48ee-8d71-d7ba5aa00d20\"}]},\"description\":{\"text\":\"Hello world\"},\"favicon\":\"data:image/png;base64,<data>\"}");
        }
    }
}
