using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Template.Core.Exceptions;
using Template.Core.Services.Internals.Interfaces;
using Template.Data.Entities.Identity;
using Template.Localization;

namespace Template.Core.Services.Internals
{
    internal sealed class UserSharedService : IUserSharedService
    {
        private readonly UserManager<User> userManager;
        private readonly ISharedResources localizer;

        public UserSharedService(
            UserManager<User> userManager,
            ISharedResources localizer)
        {
            this.userManager = userManager;
            this.localizer = localizer;
        }


        public async Task<User> FindAsync(int userId)
        {
            var user = await this.userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                this.ThrowUserNotFound();
            }

            return user;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user == null)
            {
                this.ThrowUserNotFound();
            }

            return user;
        }

        private void ThrowUserNotFound()
        {
            var message = this.localizer.GetAndApplyKeys("NotFound", "User");
            throw new NotFoundException(message);
        }
    }
}
