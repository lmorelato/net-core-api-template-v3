using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Template.Data.Entities.Interfaces;

namespace Template.Data.Context
{
    // see @ https://trailheadtechnology.com/entity-framework-core-2-1-automate-all-that-boring-boiler-plate/
    public partial class AppDbContext
    {
        private void ApplyGlobalQueryFilters(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                if (typeof(ISoftDelete).IsAssignableFrom(clrType))
                {
                    this.InvokeMethod(modelBuilder, clrType, nameof(this.SetGlobalQueryForSoftDelete));
                }

                if (typeof(ITenant).IsAssignableFrom(clrType))
                {
                    this.InvokeMethod(modelBuilder, clrType, nameof(this.SetGlobalQueryForTenant));
                }
            }
        }

        private void SetGlobalQueryForSoftDelete<T>(ModelBuilder builder) where T : class, ISoftDelete
        {
            builder.Entity<T>().HasQueryFilter(item => this.userSession.DisableSoftDeleteFilter ||
                                                       !EF.Property<bool>(item, "IsDeleted"));
        }

        private void SetGlobalQueryForTenant<T>(ModelBuilder builder) where T : class, ITenant
        {
            builder.Entity<T>().HasQueryFilter(item => this.userSession.DisableTenantFilter ||
                                                       EF.Property<int>(item, "TenantId") == this.userSession.TenantId.GetValueOrDefault());
        }

        private void InvokeMethod(object obj, Type type, string methodName)
        {
            var method = this.GetMethodInfo(methodName).MakeGenericMethod(type);
            method.Invoke(this, new[] { obj });
        }

        private MethodInfo GetMethodInfo(string methodName)
        {
            return typeof(AppDbContext)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Single(t => t.IsGenericMethod && (t.Name == methodName));
        }
    }
}