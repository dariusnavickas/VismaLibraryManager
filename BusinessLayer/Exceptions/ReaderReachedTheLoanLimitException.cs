using System;
using System.Runtime.Serialization;

namespace BusinessLayer
{
    [Serializable]
    public class ReaderReachedTheLoanLimitException : Exception
    {
        public ReaderReachedTheLoanLimitException()
        {
        }

        public ReaderReachedTheLoanLimitException(string message) : base(message)
        {
        }

        public ReaderReachedTheLoanLimitException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReaderReachedTheLoanLimitException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}