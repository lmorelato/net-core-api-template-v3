using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Template.Core.Helpers;

namespace Template.Api.Extensions.ApplicationBuilder
{
    public static partial class ApplicationBuilderExtensions
    {
        // https://www.jeffogata.com/asp-net-core-localization-culture/
        // https://joonasw.net/view/aspnet-core-localization-deep-dive
        public static IApplicationBuilder UseLocalization(this IApplicationBuilder app)
        {
            var supportedCultures = LocalizationHelper.SupportedCultures.Values.ToList();

            app.UseRequestLocalization(
                new RequestLocalizationOptions
                {
                    DefaultRequestCulture = new RequestCulture(LocalizationHelper.DefaultCultureName),
                    SupportedCultures = supportedCultures,
                    SupportedUICultures = supportedCultures
                });

            return app;
        }
    }
}
