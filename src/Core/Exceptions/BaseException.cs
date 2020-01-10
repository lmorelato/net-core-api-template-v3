using System;
using System.Runtime.Serialization;

namespace Template.Core.Exceptions
{
    [Serializable]
    public abstract class BaseException : Exception
    {
        protected BaseException()
        {
        }

        protected BaseException(string message)
            : base(message)
        {
        }

        protected BaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected BaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
