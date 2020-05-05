using System;
using TridentMc.Networking.Packets.DataTypes;
using TridentMc.Networking.State;
using String = TridentMc.Networking.Packets.DataTypes.String;

namespace TridentMc.Networking.Packets.Handshaking
{
    public class PacketHandshaking : IServerPacket, IChangesState
    {
        public ConnectionState ConnectionState => ConnectionState.Handshaking;
        public int Id => 0x00;

        public int ProtocolVersion { get; private set; }
        public string ServerAddress { get; private set; }
        public ushort ServerPort { get; private set; }
        public ConnectionState NextState { get; private set; }

        public PacketHandshaking() {}
        
        public PacketHandshaking(int protocolVersion, string address, ushort port, ConnectionState nextState)
        {
            ProtocolVersion = protocolVersion;
            ServerAddress = address;
            ServerPort = port;
            NextState = nextState;
        }

        public IServerPacket Decode(byte[] data)
        {
            var protocolVersion = VarInt.Decode(data, out data);
            var address = String.Decode(data, out data);
            var port = UShort.Decode(data, out data);
            var nextStateInt = VarInt.Decode(data, out data);
            var nextState = (ConnectionState)Enum.ToObject(typeof(ConnectionState), nextStateInt);
            
            return new PacketHandshaking(protocolVersion, address, port, nextState);
        }
    }
}
