using System;
using System.Runtime.Serialization;

namespace BusinessLayer
{
    [Serializable]
    public class ReturnDateIsInPastException : Exception
    {
        public ReturnDateIsInPastException()
        {
        }

        public ReturnDateIsInPastException(string message) : base(message)
        {
        }

        public ReturnDateIsInPastException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReturnDateIsInPastException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}