using System;

namespace TridentMc.Extentions.PacketDataTypes
{
    public static class UShortPacketExtention
    {
        public static byte[] EncodeToBytes(this ushort value)
        {
            return BitConverter.GetBytes(value);
        }
    }
}
