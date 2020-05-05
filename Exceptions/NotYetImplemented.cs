using System;
using System.Runtime.Serialization;

namespace TridentMc.Exceptions
{   
    [Serializable]
    public class NotYetImplemented : Exception
    {
        public NotYetImplemented() { }
        public NotYetImplemented(string message) : base(message) { }
        public NotYetImplemented(string message, Exception inner) : base(message, inner) { }
        protected NotYetImplemented(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
