using System;
using System.Runtime.Serialization;

namespace Permissions.Tests
{
    [Serializable]
    internal class CircularException<T> : Exception
    {
        public CircularException()
        {
        }

        public CircularException(string message) : base(message)
        {
        }

        public CircularException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CircularException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}