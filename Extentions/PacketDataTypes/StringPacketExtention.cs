using System.Linq;
using System.Text;

namespace TridentMc.Extentions.PacketDataTypes
{
    public static class StringPacketExtention
    {
        public static byte[] EncodeToBytes(this string value)
        {
            var lengthBytes = value.Length.EncodeVarint();
            var stringBytes = Encoding.UTF8.GetBytes(value);

            return lengthBytes.Concat(stringBytes).ToArray();
        }
    }
}
