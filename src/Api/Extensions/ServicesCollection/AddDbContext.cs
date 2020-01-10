using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Template.Data.Context;

namespace Template.Api.Extensions.ServicesCollection
{
    public static partial class ServicesCollectionExtensions
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, string connection)
        {
            services.AddDbContext<AppDbContext>(
            options =>
            {
                options.UseSqlServer(
                    connection,
                    sql =>
                    {
                        sql.MigrationsAssembly("Template.Data");
                    });
            });

            return services;
        }
    }
}
