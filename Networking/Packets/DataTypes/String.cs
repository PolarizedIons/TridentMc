using System.Linq;
using System.Text;

namespace TridentMc.Networking.Packets.DataTypes
{
    public static class String
    {
        public static string Decode(byte[] data, out byte[] remaining)
        {
            var length = VarInt.Decode(data, out data);
            remaining = data.Skip(length).ToArray();
            var stringBytes = data.Take(length).ToArray();

            return Encoding.UTF8.GetString(stringBytes);
        }

        public static byte[] Encode(string value)
        {
            var lengthBytes = VarInt.Encode(value.Length);
            var stringBytes = Encoding.UTF8.GetBytes(value);

            return lengthBytes.Concat(stringBytes).ToArray();
        }
    }
}
