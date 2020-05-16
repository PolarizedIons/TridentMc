using System.Linq;
using TridentMc.Networking.Packets.DataTypes;

namespace TridentMc.Networking.Packets
{
    public class PacketBuffer
    {       
        public int Length => _buffer.Length;
        public byte[] Bytes => _buffer;

        private byte[] _buffer;

        public PacketBuffer()
        {
            _buffer = new byte[0];
        }

        public PacketBuffer(byte[] buffer)
        {
            _buffer = buffer;
        }

        public int ReadVarint()
        {
            return VarInt.Decode(_buffer, out _buffer);
        }

        public void WriteVarInt(int value)
        {
            Write(VarInt.Encode(value));
        }

        public string ReadString()
        {
            return String.Decode(_buffer, out _buffer);
        }
        
        public void WriteString(string value)
        {
            Write(String.Encode(value));
        }

        public ushort ReadUshort()
        {
            return UShort.Decode(_buffer, out _buffer);
        }

        public void WriteUshort(ushort value)
        {
            Write(UShort.Encode(value));
        }

        public long ReadLong()
        {
            return Long.Decode(_buffer, out _buffer);
        }

        public void WriteLong(long value)
        {
            Write(Long.Encode(value));
        }

        private void Write(byte[] bytes)
        {
            _buffer = _buffer.Concat(bytes).ToArray();
        }
    }
}
