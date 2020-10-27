using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets
{
    public interface IChangesProtocolState
    {
        public ConnectionState NewState { get; }
    }
}
