using Microsoft.AspNetCore.Authorization;
using Template.Shared.Session;

namespace Template.Api.Controllers.Bases
{
    [Authorize]
    public abstract class AuthControllerBase : AppControllerBase
    {
        protected AuthControllerBase(IUserSession currentUser)
        {
            this.CurrentUser = currentUser;
        }

        protected IUserSession CurrentUser { get; }
    }
}