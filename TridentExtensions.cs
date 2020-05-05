using System.Linq;

namespace TridentMc
{
    static class TridentExtensions {
        public static string GetPrintableBytes(this byte[] bytes, bool printableAscii = false) {
            return string.Concat(bytes.Select(x => 
                printableAscii && (x >= (byte)'a' && x <= (byte)'z') || (x > (byte)'A' && x < (byte)'Z') ? ((char)x).ToString() : "\\x" + x.ToString("X2").ToLower()
            ));
        }
    }
}
