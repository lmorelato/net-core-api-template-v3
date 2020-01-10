using System;
using System.Linq;

namespace Template.Api.Extensions.ClaimsPrincipal
{
    public static partial class ClaimsPrincipalExtensions
    {
        public static T GetClaim<T>(this System.Security.Claims.ClaimsPrincipal claimsPrincipal, string type)
        {
            var claim = claimsPrincipal.Claims.SingleOrDefault(c => c.Type == type);

            if (claim == null)
            {
                return default;
            }

            return (T)Convert.ChangeType(claim.Value, typeof(T));
        }
    }
}
