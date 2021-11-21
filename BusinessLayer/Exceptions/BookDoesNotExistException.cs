using System;
using System.Runtime.Serialization;

namespace BusinessLayer
{
    [Serializable]
    public class BookDoesNotExistException : Exception
    {
        public BookDoesNotExistException()
        {
        }

        public BookDoesNotExistException(string message) : base(message)
        {
        }

        public BookDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BookDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}