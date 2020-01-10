using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NToolbox.Extensions.Enumerables;
using Template.Core.Exceptions;
using Template.Core.Models.Dtos;
using Template.Core.Services.Interfaces;
using Template.Core.Settings;
using Template.Data.Context;
using Template.Data.Entities;
using Template.Data.Entities.Identity;
using Template.Localization;
using Template.Shared;
using Template.Shared.Session;

namespace Template.Core.Services
{
    public class TokenService : ITokenService
    {
        private readonly AppDbContext context;
        private readonly UserManager<User> userManager;
        private readonly IUserSession userSession;
        private readonly TokenSettings tokenSettings;
        private readonly ISharedResources localizer;

        public TokenService(
            AppDbContext context,
            UserManager<User> userManager,
            IUserSession userSession,
            IOptions<TokenSettings> tokenSettings,
            ISharedResources localizer)
        {
            this.context = context;
            this.userManager = userManager;
            this.userSession = userSession;
            this.tokenSettings = tokenSettings.Value;
            this.localizer = localizer;

            this.ThrowIfInvalidSettings(this.tokenSettings);
        }

        public async Task<TokenDto> AuthenticateAsync(CredentialsDto credentials)
        {
            var user = await this.userManager.FindByNameAsync(credentials.UserName);
            if (user == null)
            {
                var message = this.localizer.GetAndApplyKeys("NotFound", "User");
                throw new NotFoundException(message);
            }

            if (!user.EmailConfirmed)
            {
                throw new EmailNotConfirmedException(this.localizer.Get("EmailNotConfirmed"));
            }

            var passwordIsValid = await this.userManager.CheckPasswordAsync(user, credentials.Password);
            if (!passwordIsValid)
            {
                throw new InvalidPasswordException(this.localizer.Get("InvalidPassword"));
            }

            var roles = await this.userManager.GetRolesAsync(user);
            var encodedToken = this.GenerateEncodedToken(user, roles);
            var tokenResult = this.GenerateTokenResult(encodedToken, user, roles);

            await this.LogAccess(user);

            return tokenResult;
        }

        private async Task LogAccess(User user)
        {
            var log = new AccessLog { UserId = user.Id, IpAddress = this.userSession.IpAddress };
            this.context.AccessLogs.Add(log);

            user.LastAccessOn = log.Date;
            await this.context.SaveChangesAsync();
        }

        private string GenerateEncodedToken(User user, IEnumerable<string> roles)
        {
            var issuedAt = this.ToUnixEpochDate(this.tokenSettings.IssuedAt);

            var claims = new[]
             {
                 new Claim(Constants.ClaimTypes.Id, user.Id.ToString(), ClaimValueTypes.Integer32),
                 new Claim(Constants.ClaimTypes.Role, roles.JoinByComma()),
                 new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                 new Claim(JwtRegisteredClaimNames.Jti, this.tokenSettings.JtiGenerator),
                 new Claim(JwtRegisteredClaimNames.Iat, issuedAt.ToString(), ClaimValueTypes.Integer64)
             };

            var token = new JwtSecurityToken(
                issuer: this.tokenSettings.Issuer,
                audience: this.tokenSettings.Audience,
                claims: claims,
                notBefore: this.tokenSettings.NotBefore,
                expires: this.tokenSettings.Expiration,
                signingCredentials: this.tokenSettings.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private TokenDto GenerateTokenResult(string encodedToken, User user, IEnumerable<string> roles)
        {
            var tokenResult = new TokenDto
            {
                Token = encodedToken,
                IssuedAt = this.tokenSettings.IssuedAt,
                Expires = this.tokenSettings.Expiration,
                Id = user.Id,
                Name = user.FullName,
                UserName = user.UserName,
                Role = roles.JoinByComma()
            };

            return tokenResult;
        }

        private long ToUnixEpochDate(DateTime date)
        {
            return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }

        private void ThrowIfInvalidSettings(TokenSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (settings.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException(nameof(TokenSettings.ValidFor));
            }

            if (settings.SigningCredentials == null)
            {
                throw new ArgumentException(nameof(TokenSettings.SigningCredentials));
            }

            if (settings.JtiGenerator == null)
            {
                throw new ArgumentException(nameof(TokenSettings.JtiGenerator));
            }
        }
    }
}
