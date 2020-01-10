using System;
using System.Runtime.Serialization;

namespace Template.Core.Exceptions
{
    [Serializable]
    public sealed class MailjetException : BaseException
    {
        public MailjetException(string message) : base(message)
        {
        }

        private MailjetException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
