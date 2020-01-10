using System;
using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Template.Core.Exceptions;
using Template.Core.Helpers;
using Template.Core.Models;
using Template.Core.Models.Dtos;
using Template.Core.Services.Interfaces;
using Template.Data.Context;
using Template.Data.Entities.Identity;
using Template.Localization;
using Template.Shared;

namespace Template.Core.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext context;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IMailjetService mailjetService;
        private readonly LinkGenerator linkGenerator;
        private readonly ISharedResources localizer;
        private readonly IMapper mapper;

        public UserService(
            AppDbContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IMailjetService mailjetService,
            LinkGenerator linkGenerator,
            ISharedResources localizer,
            IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mailjetService = mailjetService;
            this.linkGenerator = linkGenerator;
            this.localizer = localizer;
            this.mapper = mapper;
        }

        public async Task<UserDto> GetAsync(int userId)
        {
            var user = await this.FindAsync(userId);
            return this.mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> AddAsync(CredentialsDto credentials)
        {
            var newUser = this.mapper.Map<User>(credentials);
            newUser.Culture = LocalizationHelper.GetClosestSupportedCultureName();

            using (var transaction = await this.context.Database.BeginTransactionAsync())
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

            await this.SendConfirmationEmailAsync(newUser.UserName);

            return this.mapper.Map<UserDto>(newUser);
        }

        public async Task UpdateAsync(UserDto userDto)
        {
            var userEntry = await this.FindAsync(userDto.Id);
            userEntry.FullName = userDto.FullName;
            userEntry.Culture = LocalizationHelper.GetClosestSupportedCultureName();

            this.context.EnsureAudit();
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateCultureAsync(int userId)
        {
            var userEntry = await this.FindAsync(userId);
            userEntry.Culture = LocalizationHelper.GetClosestSupportedCultureName();

            this.context.EnsureAudit();
            await this.context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int userId)
        {
            var userEntry = await this.FindAsync(userId);
            this.context.Users.Remove(userEntry);
            this.context.EnsureAudit();
            await this.context.SaveChangesAsync();
        }

        public async Task UpdatePasswordAsync(int userId, PasswordDto passwordDto)
        {
            if (passwordDto.Password.Equals(passwordDto.NewPassword))
            {
                throw new InvalidPasswordException(this.localizer.Get("InvalidNewPassword"));
            }

            var user = await this.FindAsync(userId);
            var result = await this.userManager.ChangePasswordAsync(user, passwordDto.Password, passwordDto.NewPassword);
            this.ThrowIfNotSucceed(result);
        }

        public async Task ResetPasswordAsync(string userName)
        {
            using (var transaction = await this.context.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = await this.FindByEmailAsync(userName);
                    user.UpdatePasswordRequired = true;

                    var result = await this.userManager.RemovePasswordAsync(user);
                    this.ThrowIfNotSucceed(result);

                    var newPassword = UserHelper.GeneratePassword();
                    result = await this.userManager.AddPasswordAsync(user, newPassword);
                    this.ThrowIfNotSucceed(result);

                    await this.SendPasswordResetEmailAsync(user.Email, newPassword);

                    await this.context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task SendConfirmationEmailAsync(string userName)
        {
            var user = await this.FindByEmailAsync(userName);
            var token = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

            var link = this.linkGenerator.GetPathByName(
                               Constants.Api.Actions.ConfirmEmail,
                               new { user.Id, token }) +
                                LocalizationHelper.GetQueryStringCultureInfo();
            
            var settings = new EmailSettings();
            settings.Subject = this.localizer.Get("ConfirmationEmailSubject");
            settings.ToEmail = user.Email;
            settings.ToName = null;
            settings.TemplateId = this.localizer.Get("ConfirmationEmailBody");
            settings.Variables.Add(Constants.Mailjet.Keys.ConfirmationEmailLink, link);

            await this.mailjetService.Send(settings);
        }

        public async Task ConfirmEmailAsync(int userId, string token)
        {
            var user = await this.FindAsync(userId);
            var result = await this.userManager.ConfirmEmailAsync(user, token);
            this.ThrowIfNotSucceed(result);
        }

        private async Task<User> FindAsync(int userId)
        {
            var user = await this.userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                return user;
            }

            var message = this.localizer.GetAndApplyKeys("NotFound", "User");
            throw new NotFoundException(message);
        }

        private async Task<User> FindByEmailAsync(string email)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return user;
            }

            var message = this.localizer.GetAndApplyKeys("NotFound", "User");
            throw new NotFoundException(message);
        }

        private async Task AddToRoleAsync(User user, string role)
        {
            IdentityResult result;

            if (!await this.roleManager.RoleExistsAsync(role))
            {
                result = await this.roleManager.CreateAsync(new Role(role));
                this.ThrowIfNotSucceed(result);
            }

            result = await this.userManager.AddToRoleAsync(user, role);
            this.ThrowIfNotSucceed(result);
        }

        private async Task SendPasswordResetEmailAsync(string email, string password)
        {
            var settings = new EmailSettings();
            settings.Subject = this.localizer.Get("PasswordResetEmailSubject");
            settings.ToEmail = email;
            settings.ToName = null;
            settings.Body = this.localizer.GetAndApplyValues("PasswordResetEmailBody", password);

            await this.mailjetService.Send(settings, true);
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
