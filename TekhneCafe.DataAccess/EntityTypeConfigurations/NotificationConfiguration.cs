using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.DataAccess.EntityTypeConfigurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification");
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.AppUserId).IsRequired();

            builder.Property(_ => _.Message).IsRequired().HasMaxLength(150);

            builder.Property(_ => _.CreatedDate).HasDefaultValue(DateTime.Now).IsRequired();
        }
    }
}
