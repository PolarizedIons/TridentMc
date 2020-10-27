using System;
using System.Linq;
using TridentMc.Networking.Packets;

namespace TridentMc.Extentions
{
    public static class PacketBoundExtention
    {
        public static PacketAttribute GetPacketAttribute<T>(this T packet)
        {
            return Attribute.GetCustomAttributes(packet.GetType(), typeof(PacketAttribute), true).FirstOrDefault() as PacketAttribute;
        }
    }
}
