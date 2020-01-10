using Microsoft.AspNetCore.Builder;

namespace Template.Api.Extensions.ApplicationBuilder
{
    public static partial class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerApi(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    options.SwaggerEndpoint(
                        "/swagger/v1/swagger.json", 
                        "Api v1");
                });

            return app;
        }
    }
}
