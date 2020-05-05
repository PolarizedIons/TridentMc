using System;
using System.Linq;
using TridentMc.Exceptions;

namespace TridentMc.Networking.Packets.DataTypes
{
    static class VarInt {

        public static Byte[] Encode(int value) {
            int byteCount = 0;
            Byte[] varchar = new Byte[5];

            do {
                byte temp = (byte)(value & 0b01111111);
                value = (int)((uint)value >> 7);
                if (value != 0) {
                    temp |= 0b10000000;
                }
                varchar[byteCount] = temp;
                byteCount++;
            } while (value != 0);

            return varchar.Take(byteCount).ToArray();
        }

        public static int Decode(Byte[] data, out Byte[] remaining) {
            int cursor = 0;
            int numRead = 0;
            int result = 0;
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
