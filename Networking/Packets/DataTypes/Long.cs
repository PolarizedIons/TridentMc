using System;
using System.Linq;

namespace TridentMc.Networking.Packets.DataTypes
{
    public static class Long
    {
        public static long Decode(byte[] data, out byte[] remaining)
        {
            remaining = data.Skip(64).ToArray();
            return BitConverter.ToInt64(data.Take(64).ToArray());
        }

        public static byte[] Encode(long value)
        {
            return BitConverter.GetBytes(value);
        }
    }
}
