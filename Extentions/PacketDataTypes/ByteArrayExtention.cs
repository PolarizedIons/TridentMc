using System.Linq;
using TridentMc.Exceptions;

namespace TridentMc.Extentions.PacketDataTypes
{
    public static class ByteArrayExtention
    {
        public static int DecodeVarint(this byte[] data, out byte[] remaining) {
            var cursor = 0;
            var numRead = 0;
            var result = 0;
            byte read;
            do {
                read = data[cursor];
                cursor++;

                int value = (read & 0b01111111);
                result |= (value << (7 * numRead));

                numRead++;
                if (numRead > 5) {
                    throw new DecodingException("VarInt is too big");
                }
            } while ((read & 0b10000000) != 0);
            
            remaining = data.Skip(cursor).ToArray();
            return result;
        }
    }
}
