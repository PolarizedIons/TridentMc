using System;
using System.Linq;
using System.Text;
using TridentMc.Exceptions;

namespace TridentMc.Extentions.PacketDataTypes
{
    public static class ByteArrayExtention
    {
        public static int DecodeVarInt(this byte[] data, out byte[] remaining) {
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
        
        public static string DecodeString(this byte[] data, out byte[] remaining)
        {
            var length = data.DecodeVarInt(out data);
            remaining = data.Skip(length).ToArray();
            var stringBytes = data.Take(length).ToArray();

            return Encoding.UTF8.GetString(stringBytes);
        }

        public static long DecodeLong(this byte[] data, out byte[] remaining)
        {
            remaining = data.Skip(64).ToArray();
            return BitConverter.ToInt64(data.Take(64).ToArray());
        }

        public static ushort DecodeUShort(this byte[] data, out byte[] remaining)
        {
            remaining = data.Skip(2).ToArray();
            return BitConverter.ToUInt16(data.Take(2).ToArray());
        }
    }
}
