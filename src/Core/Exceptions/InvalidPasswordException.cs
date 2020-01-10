using System;
using System.Runtime.Serialization;
using Template.Core.Exceptions.Interfaces;

namespace Template.Core.Exceptions
{
    [Serializable]
    public sealed class InvalidPasswordException : BaseException, IKnownException
    {
        public InvalidPasswordException(string message) : base(message)
        {
        }

        private InvalidPasswordException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
