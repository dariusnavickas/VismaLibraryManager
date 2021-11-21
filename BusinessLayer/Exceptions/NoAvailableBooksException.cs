using System;
using System.Runtime.Serialization;

namespace BusinessLayer
{
    [Serializable]
    public class NoAvailableBooksException : Exception
    {
        public NoAvailableBooksException()
        {
        }

        public NoAvailableBooksException(string message) : base(message)
        {
        }

        public NoAvailableBooksException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoAvailableBooksException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}