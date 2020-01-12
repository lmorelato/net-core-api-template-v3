using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Template.Core.Exceptions;
using Template.Core.Helpers;
using Template.Core.Models;
using Template.Core.Models.Dtos;
using Template.Core.Services.Interfaces;
using Template.Core.Services.Internals.Interfaces;
using Template.Data.Context;
using Template.Data.Entities.Identity;
using Template.Localization;
using Template.Shared;

namespace Template.Core.Services
{
    public sealed class UserService : IUserService
    {
        private readonly AppDbContext context;
        private readonly UserManager<User> userManager;
        private readonly IUserSharedService userSharedService;
        private readonly IUserAccountService userAccountService;
        private readonly IMapper mapper;

        public UserService(
            AppDbContext context,
            UserManager<User> userManager,
            IUserSharedService userSharedService,
            IUserAccountService userAccountService,
            IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.userSharedService = userSharedService;
            this.userAccountService = userAccountService;
            this.mapper = mapper;
        }

        public async Task<UserDto> GetAsync(int userId)
        {
            var user = await this.userSharedService.FindAsync(userId);
            return this.mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> AddAsync(CredentialsDto credentials)
        {
            var newUser = this.mapper.Map<User>(credentials);
            newUser.Culture = LocalizationHelper.GetClosestSupportedCultureName();

            await using (var transaction = await this.context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await this.userManager.CreateAsync(newUser, credentials.Password);
                    this.ThrowIfNotSucceed(result);
                    await this.AddToRoleAsync(newUser, Constants.Roles.User);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            await this.userAccountService.SendConfirmationEmailAsync(newUser.UserName);

            return this.mapper.Map<UserDto>(newUser);
        }

        public async Task UpdateAsync(UserDto userDto)
        {
            var userEntry = await this.userSharedService.FindAsync(userDto.Id);
            userEntry.FullName = userDto.FullName;
            userEntry.Culture = LocalizationHelper.GetClosestSupportedCultureName();

            this.context.EnsureAudit();
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateCultureAsync(int userId)
        {
            var userEntry = await this.userSharedService.FindAsync(userId);
            userEntry.Culture = LocalizationHelper.GetClosestSupportedCultureName();

            this.context.EnsureAudit();
            await this.context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int userId)
        {
            var userEntry = await this.userSharedService.FindAsync(userId);
            this.context.Users.Remove(userEntry);
            this.context.EnsureAudit();
            await this.context.SaveChangesAsync();
        }

        private async Task AddToRoleAsync(User user, string role)
        {
            var result = await this.userManager.AddToRoleAsync(user, role);
            this.ThrowIfNotSucceed(result);
        }

        private void ThrowIfNotSucceed(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new IdentityResultException(result);
            }
        }
    }
}
