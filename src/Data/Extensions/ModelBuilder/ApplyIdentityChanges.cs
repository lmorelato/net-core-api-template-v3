using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Template.Data.Entities.Identity;
using Template.Shared;

namespace Template.Data.Extensions.ModelBuilder
{
    public static partial class ModelBuilderExtensions
    {
        public static void ApplyIdentityConfiguration(this Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(
                entity =>
                {
                    entity.ToTable("IdentityUsers");

                    entity
                        .Property(e => e.PasswordHash)
                        .HasMaxLength(Constants.Database.IdentityVarcharMaxLength);
                    entity
                        .Property(e => e.SecurityStamp)
                        .HasMaxLength(Constants.Database.IdentityVarcharMaxLength);
                    entity
                        .Property(e => e.ConcurrencyStamp)
                        .HasMaxLength(Constants.Database.IdentityVarcharMaxLength);
                    entity
                        .Property(e => e.Culture)
                        .IsRequired()
                        .HasMaxLength(10);
                });

            modelBuilder.Entity<Role>(
                entity =>
                {
                    entity.ToTable("IdentityRoles");

                    entity
                        .Property(p => p.ConcurrencyStamp)
                        .HasMaxLength(Constants.Database.IdentityVarcharMaxLength);
                });

            modelBuilder.Entity<IdentityUserClaim<int>>(
                entity =>
                {
                    entity.ToTable("IdentityUserClaims");

                    entity
                        .Property(p => p.ClaimType)
                        .HasMaxLength(Constants.Database.IdentityVarcharMaxLength);
                    entity
                        .Property(p => p.ClaimValue)
                        .HasMaxLength(Constants.Database.IdentityVarcharMaxLength);
                });

            modelBuilder.Entity<IdentityRoleClaim<int>>(
                entity =>
                {
                    entity.ToTable("IdentityRoleClaims");

                    entity
                        .Property(p => p.ClaimType)
                        .HasMaxLength(Constants.Database.IdentityVarcharMaxLength);
                    entity
                        .Property(p => p.ClaimValue)
                        .HasMaxLength(Constants.Database.IdentityVarcharMaxLength);
                });

            modelBuilder.Entity<IdentityUserRole<int>>(
                entity =>
                {
                    entity.ToTable("IdentityUserRoles");

                    entity
                        .HasKey(key => new { key.UserId, key.RoleId });
                });

            modelBuilder.Entity<IdentityUserLogin<int>>(
                entity =>
                {
                    entity.ToTable("IdentityUserLogins");

                    entity
                        .HasKey(key => new { key.ProviderKey, key.LoginProvider });
                    entity
                        .Property(p => p.LoginProvider)
                        .HasMaxLength(Constants.Database.IdentityVarcharMaxLength);
                    entity
                        .Property(p => p.ProviderKey)
                        .HasMaxLength(Constants.Database.IdentityVarcharMaxLength);
                    entity
                        .Property(p => p.ProviderDisplayName)
                        .HasMaxLength(Constants.Database.IdentityVarcharMaxLength);
                });

            modelBuilder.Entity<IdentityUserToken<int>>(
                entity =>
                {
                    entity.ToTable("IdentityUserTokens");

                    entity
                        .HasKey(key => new { key.UserId, key.LoginProvider, key.Name });
                    entity
                        .Property(p => p.LoginProvider)
                        .HasMaxLength(Constants.Database.IdentityVarcharMaxLength);
                    entity
                        .Property(p => p.Name)
                        .HasMaxLength(Constants.Database.IdentityVarcharMaxLength);
                    entity
                        .Property(p => p.Value)
                        .HasMaxLength(Constants.Database.IdentityVarcharMaxLength);
                });
        }
    }
}
