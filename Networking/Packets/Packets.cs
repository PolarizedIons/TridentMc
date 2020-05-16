using System;
using System.Collections.Generic;
using System.Linq;
using TridentMc.Networking.Packets.Handshaking;
using TridentMc.Networking.Packets.Status;
using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets
{
    public abstract class BasePacket
    {
    }

    public abstract class ServerPacket : BasePacket
    {
        protected ServerPacket() {}

        protected ServerPacket(PacketBuffer bytes)
        {
        }

        public abstract void Handle(ServerSession session);
    }

    public abstract class ClientPacket : BasePacket
    {
        public virtual int Id { get; }

        public abstract PacketBuffer Encode();
    }

    public interface IChangesState
    {
        ConnectionState NextState { get; }
    }
}
