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
    public sealed class UserAccountService : IUserAccountService
    {
        private readonly AppDbContext context;
        private readonly UserManager<User> userManager;
        private readonly LinkGenerator linkGenerator;
        private readonly IUserSharedService userSharedService;
        private readonly IMailjetService mailjetService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ISharedResources localizer;

        public UserAccountService(
            AppDbContext context,
            UserManager<User> userManager,
            LinkGenerator linkGenerator,
            IUserSharedService userSharedService,
            IMailjetService mailjetService,
            IHttpContextAccessor httpContextAccessor,
            ISharedResources localizer)
        {
            this.context = context;
            this.userManager = userManager;
            this.linkGenerator = linkGenerator;
            this.userSharedService = userSharedService;
            this.mailjetService = mailjetService;
            this.httpContextAccessor = httpContextAccessor;
            this.localizer = localizer;
        }

        public async Task UpdatePasswordAsync(int userId, PasswordDto passwordDto)
        {
            if (passwordDto.Password.Equals(passwordDto.NewPassword))
            {
                throw new InvalidPasswordException(this.localizer.Get("InvalidNewPassword"));
            }

            var user = await this.userSharedService.FindAsync(userId);
            var result = await this.userManager.ChangePasswordAsync(user, passwordDto.Password, passwordDto.NewPassword);
            this.ThrowIfNotSucceed(result);
        }

        public async Task ResetPasswordAsync(string userName)
        {
            await using var transaction = await this.context.Database.BeginTransactionAsync();

            try
            {
                var user = await this.userSharedService.FindByEmailAsync(userName);
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

        public async Task SendConfirmationEmailAsync(string userName)
        {
            var user = await this.userSharedService.FindByEmailAsync(userName);
            if (!user.EmailConfirmed)
            {
                this.localizer.Get("EmailAlreadyConfirmed");
            }

            var token = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = this.GenerateConfirmationEmailLink(user, token);

            var settings = new EmailOptions
            {
                Subject = this.localizer.Get("ConfirmationEmailSubject"),
                ToEmail = user.Email,
                ToName = null,
                TemplateId = this.localizer.Get("ConfirmationEmailBody")
            };
            settings.Variables.Add(Constants.Mailjet.Keys.ConfirmationEmailLink, link);

            await this.mailjetService.Send(settings);
        }

        public async Task ConfirmEmailAsync(int userId, string token)
        {
            var user = await this.userSharedService.FindAsync(userId);
            if (user.EmailConfirmed)
            {
                this.localizer.Get("EmailAlreadyConfirmed");
            }

            var result = await this.userManager.ConfirmEmailAsync(user, token);
            this.ThrowIfNotSucceed(result);
        }

        private string GenerateConfirmationEmailLink(User user, string token)
        {
            return this.linkGenerator.GetUriByName(
                       this.httpContextAccessor.HttpContext,
                       Constants.Api.Actions.ConfirmEmail,
                       new { user.Id, token }) +
                   LocalizationHelper.GetQueryStringCultureInfo();
        }

        private async Task SendPasswordResetEmailAsync(string email, string password)
        {
            var settings = new EmailOptions
            {
                Subject = this.localizer.Get("PasswordResetEmailSubject"),
                ToEmail = email,
                ToName = null,
                Body = this.localizer.GetAndApplyValues("PasswordResetEmailBody", password)
            };

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
