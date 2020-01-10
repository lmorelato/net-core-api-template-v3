using System;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Template.Data.Entities;
using Template.Data.Entities.Interfaces;

namespace Template.Data.Context
{
    public partial class AppDbContext
    {
        // see @ https://trailheadtechnology.com/entity-framework-core-2-1-automate-all-that-boring-boiler-plate/
        private void InspectBeforeSave()
        {
            this.CheckReadOnlyEntries();

            var timestamp = DateTime.UtcNow;
            if (this.ensureAutoHistory)
            {
                this.EnsureAutoHistory(timestamp);
            }

            foreach (var entry in this.ChangeTracker.Entries())
            {
                if (entry.Entity is ISoftDelete)
                {
                    this.SetSoftDeleteEntity(entry);
                }

                if (entry.Entity is IFullTracked || entry.Entity is IDateTimeTracked)
                {
                    this.SetTrackedEntity(entry, timestamp);
                }
            }
        }

        private void EnsureAutoHistory(DateTime timestamp)
        {
            this.EnsureAutoHistory(
                () => new AuditLog
                {
                    ModifiedById = this.userSession.UserId,
                    ModifiedOn = timestamp
                });
        }

        private void SetSoftDeleteEntity(EntityEntry entry)
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Property("IsDeleted").CurrentValue = true;
            }
        }

        private void SetTrackedEntity(EntityEntry entry, DateTime timestamp)
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
            {
                entry.CurrentValues["ModifiedOn"] = timestamp;

                if (entry.Entity is IFullTracked)
                {
                    entry.CurrentValues["ModifiedById"] = this.userSession.UserId;
                }
            }

            if (entry.State == EntityState.Added)
            {
                entry.CurrentValues["CreatedOn"] = timestamp;

                if (entry.Entity is IFullTracked)
                {
                    entry.CurrentValues["CreatedById"] = this.userSession.UserId;
                }

                if (entry.Entity is ITenant)
                {
                    entry.Property("TenantId").CurrentValue = this.userSession.TenantId.GetValueOrDefault();
                }
            }
        }

        private void CheckReadOnlyEntries()
        {
            if (this.ChangeTracker.Entries<IReadOnly>().Any(entry =>
                entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted))
            {
                throw new ReadOnlyException("Attempt to change read-only entity");
            }
        }
    }
}
