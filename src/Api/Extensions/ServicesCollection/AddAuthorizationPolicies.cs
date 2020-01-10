using Microsoft.Extensions.DependencyInjection;
using Template.Shared;

namespace Template.Api.Extensions.ServicesCollection
{
    public static partial class ServicesCollectionExtensions
    {
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(
                options =>
                {
                    options.AddPolicy(Constants.Roles.Admin, policy => policy.RequireRole(Constants.Roles.Admin));

                    options.AddPolicy(
                        Constants.Roles.User,
                        policy => policy.RequireRole(Constants.Roles.Admin, Constants.Roles.User));

                    options.AddPolicy(
                        Constants.Roles.Any,
                        policy => policy.RequireRole(Constants.Roles.Admin, Constants.Roles.User));
                });

            return services;
        }
    }
}
