using System;
using System.Runtime.Serialization;

namespace BusinessLayer
{
    [Serializable]
    public class BookIsNotLoanedToTheReaderException : Exception
    {
        public BookIsNotLoanedToTheReaderException()
        {
        }

        public BookIsNotLoanedToTheReaderException(string message) : base(message)
        {
        }

        public BookIsNotLoanedToTheReaderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BookIsNotLoanedToTheReaderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}