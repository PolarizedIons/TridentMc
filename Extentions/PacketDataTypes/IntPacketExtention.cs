using System.Linq;

namespace TridentMc.Extentions.PacketDataTypes
{
    public static class IntExtention
    {
        public static byte[] EncodeVarint(this int value) {
            var byteCount = 0;
            var varchar = new byte[5];

            do {
                var temp = (byte)(value & 0b01111111);
                value = (int)((uint)value >> 7);
                if (value != 0) {
                    temp |= 0b10000000;
                }
                varchar[byteCount] = temp;
                byteCount++;
            } while (value != 0);

            return varchar.Take(byteCount).ToArray();
        }
    }
}
