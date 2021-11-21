using System;
using System.Runtime.Serialization;

namespace BusinessLayer
{
    [Serializable]
    public class BookIsNotAvailableException : Exception
    {
        public BookIsNotAvailableException()
        {
        }

        public BookIsNotAvailableException(string message) : base(message)
        {
        }

        public BookIsNotAvailableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BookIsNotAvailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}