using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Template.Api.Extensions.HostingEnvironment
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsTrusted(this IWebHostEnvironment env)
        {
            return env.IsDevelopment() || env.IsStaging();
        }
    }
}
