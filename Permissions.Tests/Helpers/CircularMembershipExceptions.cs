using System;
using System.Runtime.Serialization;

namespace Permissions.Tests
{
    [Serializable]
    internal class CircularMembershipExceptions : Exception
    {
        public CircularMembershipExceptions()
        {
        }

        public CircularMembershipExceptions(string message) : base(message)
        {
        }

        public CircularMembershipExceptions(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CircularMembershipExceptions(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}