using TridentMc.Events;
using TridentMc.Events.Networking;
using TridentMc.Networking.Packets.Status;

namespace TridentMc.Networking.Packets
{
    public class PacketResponders
    {
        public PacketResponders()
        {
            EventManager em = TridentMc.Instance.EventManager;
            
            em.Listen<PacketReceived<PacketRequest>>(EventPriority.Immediate, OnStatusRequest);
            em.Listen<PacketReceived<PacketPing>>(EventPriority.Immediate, OnStatusPing);
        }

        private void OnStatusRequest(PacketReceived<PacketRequest> theEvent)
        {
            theEvent.Session.SendPacket(PacketResponse.FromSystem());
        }

        private void OnStatusPing(PacketReceived<PacketPing> theEvent)
        {
            theEvent.Session.SendPacket(new PacketPong(theEvent.Packet.Payload));
        }
    }
}