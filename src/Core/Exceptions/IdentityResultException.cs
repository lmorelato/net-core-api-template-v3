using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;
using Template.Core.Exceptions.Interfaces;

namespace Template.Core.Exceptions
{
    [Serializable]
    public sealed class IdentityResultException : BaseException, IKnownException
    {
        public IdentityResultException(IdentityResult result)
        {
            this.Errors = result.Errors;
        }

        private IdentityResultException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IEnumerable<IdentityError> Errors { get; }
    }
}
