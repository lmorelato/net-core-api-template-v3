using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Template.Api.Extensions.ClaimsPrincipal;
using Template.Shared;
using Template.Shared.Session;

namespace Template.Api.Middleware
{
    // see @ https://trailheadtechnology.com/aspnetcore-multi-tenant-tips-and-tricks/
    // see @ https://www.jerriepelser.com/blog/aspnetcore-geo-location-from-ip-address/
    public class ConfigureSessionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string userNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        private readonly string rolesClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        public ConfigureSessionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserSession currentUser)
        {
            currentUser.IpAddress = context.Connection.RemoteIpAddress.ToString();

            if (context.User.Identities.Any(user => user.IsAuthenticated))
            {
                currentUser.UserId = context.User.GetClaim<int>(Constants.ClaimTypes.Id);
                currentUser.UserName = context.User.GetClaim<string>(this.userNameClaimType);
                currentUser.Roles = context.User.GetClaims<string>(this.rolesClaimType);

                if (currentUser.IsInRole(Constants.Roles.Tenant))
                {
                    currentUser.TenantId = currentUser.UserId;
                }
                else
                {
                    currentUser.DisableTenantFilter = true;
                }
            }

            await this.next.Invoke(context);
        }
    }
}
