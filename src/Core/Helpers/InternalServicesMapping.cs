using Microsoft.Extensions.DependencyInjection;
using Template.Core.Services.Internals;
using Template.Core.Services.Internals.Interfaces;

namespace Template.Core.Helpers
{
    public static class InternalServicesMapping
    {
        public static void AddServicesMapping(IServiceCollection services)
        {
            services.AddScoped<IUserSharedService, UserSharedService>();
        }
    }
}
