using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Template.Core.Settings;

namespace Template.Api.Extensions.ServicesCollection
{
    public static partial class ServicesCollectionExtensions
    {
        public static IServiceCollection AddAuthenticationToken(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenOptions = configuration.GetSection(nameof(TokenSettings));
            var signingKey = SetTokenSettings(services, tokenOptions);

            AddAuthentication(services, tokenOptions, signingKey);
            return services;
        }

        private static SymmetricSecurityKey SetTokenSettings(
            IServiceCollection services,
            IConfiguration configuration)
        {
            // https://www.grc.com/passwords.htm
            var apiSecretKey = configuration["SigningKey"];
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(apiSecretKey));

            services.Configure<TokenSettings>(
                options =>
                {
                    options.Issuer = configuration[nameof(TokenSettings.Issuer)];
                    options.Audience = configuration[nameof(TokenSettings.Audience)];
                    options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                    options.LifetimeInMinutes = Convert.ToInt32(configuration[nameof(TokenSettings.LifetimeInMinutes)]);
                });

            return signingKey;
        }

        private static void AddAuthentication(
             IServiceCollection services,
             IConfiguration jwtAppSettingOptions,
             SecurityKey signingKey)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
                    options.ClaimsIssuer = jwtAppSettingOptions[nameof(TokenSettings.Issuer)];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtAppSettingOptions[nameof(TokenSettings.Issuer)],
                        ValidateAudience = true,
                        ValidAudience = jwtAppSettingOptions[nameof(TokenSettings.Audience)],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,
                        RequireExpirationTime = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    options.SaveToken = true;
                });
        }
    }
}
