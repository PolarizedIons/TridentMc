using System;
using TridentMc.Networking.Packets.DataTypes;
using TridentMc.Networking.State;
using String = TridentMc.Networking.Packets.DataTypes.String;

namespace TridentMc.Networking.Packets.Handshaking
{
    public class PacketHandshaking : ServerPacket, IChangesState
    {
        public int ProtocolVersion { get; private set; }
        public string ServerAddress { get; private set; }
        public ushort ServerPort { get; private set; }
        public ConnectionState NextState { get; private set; }

        public PacketHandshaking(PacketBuffer buffer)
        {
            ProtocolVersion = buffer.ReadVarint();
            ServerAddress = buffer.ReadString();
            ServerPort = buffer.ReadUshort();

            var nextStateInt = buffer.ReadVarint();
            NextState = (ConnectionState)Enum.ToObject(typeof(ConnectionState), nextStateInt);
        }

        public override void Handle(ServerSession session)
        {
            // NOOP
        }
    }
}
