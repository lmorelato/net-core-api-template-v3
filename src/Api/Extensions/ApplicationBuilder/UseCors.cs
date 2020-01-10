using Microsoft.AspNetCore.Builder;

namespace Template.Api.Extensions.ApplicationBuilder
{
    public static partial class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCorsMiddleware(this IApplicationBuilder app)
        {
            app.UseCors(
                policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            return app;
        }
    }
}
