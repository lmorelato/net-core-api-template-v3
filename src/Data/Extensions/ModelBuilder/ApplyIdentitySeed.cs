using System;
using Template.Data.Entities.Identity;
using Template.Shared;

namespace Template.Data.Extensions.ModelBuilder
{
    public static partial class ModelBuilderExtensions
    {
        public static Microsoft.EntityFrameworkCore.ModelBuilder ApplyIdentitySeed(this Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = Constants.Roles.Admin,
                    NormalizedName = Constants.Roles.Admin.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D")
                },
                new Role
                {
                    Id = 2,
                    Name = Constants.Roles.User,
                    NormalizedName = Constants.Roles.User.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D")
                },
                new Role
                {
                    Id = 3,
                    Name = Constants.Roles.Tenant,
                    NormalizedName = Constants.Roles.Tenant.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D")
                });

            return modelBuilder;
        }
    }
}
