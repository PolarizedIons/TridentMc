using System;
using System.Collections.Generic;
using System.Linq;
using TridentMc.Extentions.PacketDataTypes;
using TridentMc.Networking.State;

namespace TridentMc.Networking.Packets
{
    public class PacketBuffer
    {
        public bool Any => Buffer.Any();
        public int Length => Buffer.Length;

        public byte[] Buffer = new byte[0];

        public int ReadVarInt()
        {
            return Buffer.DecodeVarInt(out Buffer);
        }

        public void WriteVarInt(int value)
        {
            WriteToEnd(value.EncodeVarint());
        }

        public string ReadString()
        {
            return Buffer.DecodeString(out Buffer);
        }

        public void WriteString(string value)
        {
            WriteToEnd(value.EncodeToBytes());
        }

        public ushort ReadUshort()
        {
            return Buffer.DecodeUShort(out Buffer);
        }

        public void WriteUshort(ushort value)
        {
            WriteToEnd(value.EncodeToBytes());
        }

        public long ReadLong()
        {
            return Buffer.DecodeLong(out Buffer);
        }

        public void WriteLong(long value)
        {
            WriteToEnd(value.EncodeToBytes());
        }

        public T ReadVarIntEnum<T>()
        {
            var intValue = ReadVarInt();
            return (T) Enum.ToObject(typeof(T) , intValue);
        }

        public void WriteToStart(IEnumerable<byte> bytes)
        {
            Buffer = bytes.Concat(Buffer).ToArray();
        } 

        public void WriteToEnd(IEnumerable<byte> bytes)
        {
            Buffer = Buffer.Concat(bytes).ToArray();
        }
    }
}
