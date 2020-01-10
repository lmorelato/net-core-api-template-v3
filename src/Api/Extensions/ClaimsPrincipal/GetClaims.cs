using System;
using System.Collections.Generic;
using System.Linq;

using NToolbox.Extensions.Strings;

namespace Template.Api.Extensions.ClaimsPrincipal
{
    public static partial class ClaimsPrincipalExtensions
    {
        public static List<T> GetClaims<T>(this System.Security.Claims.ClaimsPrincipal claimsPrincipal, string type)
        {
            var claims = claimsPrincipal.Claims
                .Where(c => c.Type == type && c.Value.IsNotNullOrEmpty())
                .ToList();

            return claims.Any() ?
                       claims.Select(c => (T)Convert.ChangeType(c.Value, typeof(T))).ToList() :
                       new List<T>();
        }
    }
}