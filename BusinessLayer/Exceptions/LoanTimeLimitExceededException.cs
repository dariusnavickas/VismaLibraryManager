using System;
using System.Runtime.Serialization;

namespace BusinessLayer
{
    [Serializable]
    public class LoanTimeLimitExceededException : Exception
    {
        public LoanTimeLimitExceededException()
        {
        }

        public LoanTimeLimitExceededException(string message) : base(message)
        {
        }

        public LoanTimeLimitExceededException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LoanTimeLimitExceededException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}