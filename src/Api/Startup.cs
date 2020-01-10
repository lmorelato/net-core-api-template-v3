using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Template.Api.Extensions.ApplicationBuilder;
using Template.Api.Extensions.HostingEnvironment;
using Template.Api.Extensions.ServicesCollection;
using Template.Api.Middleware;
using Template.Core.Profiles;
using Template.Core.Settings;
using Template.Data.Context;

namespace Template.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .Configure<MailjetApiSettings>(this.Configuration.GetSection(nameof(MailjetApiSettings)))
                .Configure<FacebookAuthSettings>(this.Configuration.GetSection(nameof(FacebookAuthSettings)))
                .AddDbContext(this.Configuration.GetConnectionString("DefaultConnection"))
                .AddIdentity()
                .AddAuthenticationToken(this.Configuration)
                .AddAuthorizationPolicies()
                .AddServicesMApping()
                .AddAutoMapper(typeof(AutoMapperProfile))
                .AddMemoryCache()
                .AddSwagger()
                .AddLocalization(o => o.ResourcesPath = "Resources")
                .AddDefaultServices()
                .ModifyApiBehaviour();
        }

        public void Configure(IApplicationBuilder application, IWebHostEnvironment env, AppDbContext appDbContext)
        {
            appDbContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                application.UseForwardedHeaders(
                    new ForwardedHeadersOptions
                    {
                        ForwardedHeaders = ForwardedHeaders.XForwardedFor,
                        ForwardLimit = 2
                    });
            }

            application
                .UseExceptionHandler(env.IsTrusted())
                .UseSerilogRequestLogging()
                .UseHsts()
                .UseHttpsRedirection()
                .UseRouting()
                .UseCorsMiddleware()
                .UseAuthentication()
                .UseAuthorization()
                .UseMiddleware<ConfigureSessionMiddleware>()
                .UseLocalization()
                .UseSwaggerApi()
                .UseEndpoints(
                    endpoints =>
                    {
                        endpoints.MapControllers();
                    });
        }
    }
}
