using System;

using Microsoft.AspNetCore.Identity;
using Template.Data.Entities.Interfaces;

namespace Template.Data.Entities.Identity
{
    public sealed class User : IdentityUser<int>, ISoftDelete, IDateTimeTracked
    {
        public string FullName { get; set; }

        public string Culture { get; set; } = "en-US";

        public bool UpdatePasswordRequired { get; set; } 

        public DateTime? LastAccessOn { get; set; } 
    }
}