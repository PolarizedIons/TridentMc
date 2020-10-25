using System;

namespace TridentMc.Exceptions
{
    public class DecodingException : Exception
    {
        public DecodingException(string message) : base(message)
        {
        }
    }
}
