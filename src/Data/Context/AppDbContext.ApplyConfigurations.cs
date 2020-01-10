using Microsoft.EntityFrameworkCore;
using Template.Data.Entities;
using Template.Data.Entities.Configurations;

namespace Template.Data.Context
{
    public partial class AppDbContext
    {
        public virtual DbSet<AccessLog> AccessLogs { get; set; }

        private void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccessLogConfig());
        }
    }
}