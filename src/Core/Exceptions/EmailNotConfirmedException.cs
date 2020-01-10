using System;
using System.Runtime.Serialization;
using Template.Core.Exceptions.Interfaces;

namespace Template.Core.Exceptions
{
    [Serializable]
    public sealed class EmailNotConfirmedException : BaseException, IKnownException
    {
        public EmailNotConfirmedException(string message)
            : base(message)
        {
        }

        private EmailNotConfirmedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
