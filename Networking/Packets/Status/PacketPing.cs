using Microsoft.VisualBasic.CompilerServices;
using TridentMc.Networking.Packets.DataTypes;
using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets.Status
{
    public class PacketPing : IServerPacket
    {
        public ConnectionState ConnectionState => ConnectionState.Status;
        public int Id => 0x01;

        public long Payload { get; private set; }
        
        public PacketPing() {}

        public PacketPing(long payload)
        {
            Payload = payload;
        }
        
        public IServerPacket Decode(byte[] data)
        {
            return new PacketPing(Long.Decode(data, out data));
        }
    }
}
