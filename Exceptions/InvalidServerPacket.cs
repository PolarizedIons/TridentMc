using System;
using System.Runtime.Serialization;

namespace TridentMc.Exceptions
{   
    [Serializable]
    public class InvalidServerPacket : Exception
    {
        public InvalidServerPacket() { }
        public InvalidServerPacket(string message) : base(message) { }
        public InvalidServerPacket(string message, Exception inner) : base(message, inner) { }
        protected InvalidServerPacket(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
