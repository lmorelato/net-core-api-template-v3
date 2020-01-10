using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Template.Data.Entities.Configurations
{
    public class AccessLogConfig : IEntityTypeConfiguration<AccessLog>
    {
        public void Configure(EntityTypeBuilder<AccessLog> builder)
        {
            // Key

            // Index

            // Columns
            builder.Property(e => e.Latitude).HasColumnType("decimal(12,8)");
            builder.Property(e => e.Longitude).HasColumnType("decimal(12,8)");
            builder.Property(e => e.IpAddress).HasMaxLength(20);

            // Owns

            // Relationships
        }
    }
}