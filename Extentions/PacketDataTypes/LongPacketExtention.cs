using System;

namespace TridentMc.Extentions.PacketDataTypes
{
    public static class LongPacketExtention
    {
        public static byte[] EncodeToBytes(this long value)
        {
            return BitConverter.GetBytes(value);
        }
    }
}
