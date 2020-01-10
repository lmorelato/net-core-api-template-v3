using System.Text.Json;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Template.Api.Filters;
using Template.Core.Models.Validators;

namespace Template.Api.Extensions.ServicesCollection
{
    public static partial class ServicesCollectionExtensions
    {
        public static IServiceCollection AddDefaultServices(this IServiceCollection services)
        {
            services
                .AddControllers(
                    setup =>
                    {
                        setup.Filters.Add(typeof(ModelStateFilter));
                        setup.SuppressAsyncSuffixInActionNames = false;
                    })
                .AddFluentValidation(
                    fv =>
                    {
                        fv.RegisterValidatorsFromAssemblyContaining<CredentialsDtoValidator>();
                    })
                .AddJsonOptions(
                    options =>
                    {
                        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    });

            return services;
        }
    }
}
