using System;
using Microsoft.Extensions.DependencyInjection;
using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets.Handshaking
{
    [ServerPacket(ConnectionState.Handshaking, 0x00)]
    public class Handshake : ServerBoundPacket, IChangesProtocolState
    {
        public int ProtocolVersion { get; set; }
        public string ServerAddress { get; set; }
        public ushort ServerPort { get; set; }
        public ConnectionState NewState { get; set; }

        public Handshake(IServiceProvider services, PacketBuffer packet) : base(services, packet)
        {
            ProtocolVersion = packet.ReadVarInt();
            ServerAddress = packet.ReadString();
            ServerPort = packet.ReadUshort();
            NewState = packet.ReadVarIntEnum<ConnectionState>();
        }

        public override void Handle(ServerConnection connection)
        {
            // No action required
        }
    }
}
