using System;
using System.Runtime.Serialization;

namespace BusinessLayer
{
    [Serializable]
    public class BookCannotHaveNullOrWhiteSpaceValuesException : Exception
    {
        public BookCannotHaveNullOrWhiteSpaceValuesException()
        {
        }

        public BookCannotHaveNullOrWhiteSpaceValuesException(string message) : base(message)
        {
        }

        public BookCannotHaveNullOrWhiteSpaceValuesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BookCannotHaveNullOrWhiteSpaceValuesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}