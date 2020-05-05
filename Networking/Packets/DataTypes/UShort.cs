using System;
using System.Linq;

namespace TridentMc.Networking.Packets.DataTypes
{
    public static class UShort
    {
        public static ushort Decode(byte[] data, out byte[] remaining)
        {
            remaining = data.Skip(2).ToArray();
            return BitConverter.ToUInt16(data.Take(2).ToArray());
        }

        public static byte[] Encode(ushort value)
        {
            return BitConverter.GetBytes(value);
        }
    }
}
