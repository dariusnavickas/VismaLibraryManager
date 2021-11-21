using System;
using System.Runtime.Serialization;

namespace BusinessLayer
{
    [Serializable]
    public class BookAlreadyExistsWithTheSameIsbnException : Exception
    {
        public BookAlreadyExistsWithTheSameIsbnException()
        {
        }

        public BookAlreadyExistsWithTheSameIsbnException(string message) : base(message)
        {
        }

        public BookAlreadyExistsWithTheSameIsbnException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BookAlreadyExistsWithTheSameIsbnException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}