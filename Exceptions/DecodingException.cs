using System;
using System.Runtime.Serialization;

namespace TridentMc.Exceptions
{   
    [Serializable]
    public class DecodingException : Exception
    {
        public DecodingException() { }
        public DecodingException(string message) : base(message) { }
        public DecodingException(string message, Exception inner) : base(message, inner) { }
        protected DecodingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
