using System.Linq;

namespace TridentMc.Extentions
{
    public static class ByteArrayExtention
    {
        // Python-like byte array representation
        public static string GetPrintableBytes(this byte[] bytes, bool printableAscii = false) {
            return string.Concat(bytes.Select(x => 
                printableAscii && (x >= (byte)'a' && x <= (byte)'z') || (x > (byte)'A' && x < (byte)'Z') ? ((char)x).ToString() : "\\x" + x.ToString("X2").ToLower()
            ));
        }
    }
}
