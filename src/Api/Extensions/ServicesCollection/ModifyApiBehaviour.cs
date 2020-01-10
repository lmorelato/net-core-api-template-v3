using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Template.Api.Controllers.Bases;

namespace Template.Api.Extensions.ServicesCollection
{
    public static partial class ServicesCollectionExtensions
    {
        public static IServiceCollection ModifyApiBehaviour(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(
                options =>
                {
                    options.InvalidModelStateResponseFactory = 
                        ctx => new ObjectResultBase(HttpStatusCode.BadRequest, ctx.ModelState);
                });

            return services;
        }
    }
}
