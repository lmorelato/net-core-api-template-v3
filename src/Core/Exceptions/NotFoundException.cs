using System;
using System.Runtime.Serialization;
using Template.Core.Exceptions.Interfaces;

namespace Template.Core.Exceptions
{
    [Serializable]
    public sealed class NotFoundException : BaseException, IKnownException
    {
        public NotFoundException(string message) : base(message)
        {
        }

        private NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
