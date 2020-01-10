using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Template.Data.Context;
using Template.Data.Entities.Identity;
using Template.Localization;

namespace Template.Api.Extensions.ServicesCollection
{
    public static partial class ServicesCollectionExtensions
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            var identity = services.AddIdentityCore<User>(
                setup =>
                {
                    setup.Password.RequireDigit = false;
                    setup.Password.RequireLowercase = false;
                    setup.Password.RequireUppercase = false;
                    setup.Password.RequireNonAlphanumeric = false;
                    setup.Password.RequiredLength = 6;
                    setup.SignIn.RequireConfirmedEmail = true;
                });

            identity = new IdentityBuilder(
                identity.UserType,
                typeof(Role),
                identity.Services);

            identity
                .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services
                .Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromDays(7));

            return services;
        }
    }
}
