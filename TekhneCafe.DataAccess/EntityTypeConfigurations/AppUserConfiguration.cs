using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.DataAccess.EntityTypeConfigurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUser");
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.LdapId).IsRequired();

            builder.Property(_ => _.FullName).HasMaxLength(100).IsRequired();

            builder.Property(_ => _.Username).HasMaxLength(100).IsRequired();
            builder.HasIndex(_ => _.Username).IsUnique();

            builder.Property(_ => _.Email).IsRequired().HasMaxLength(100);

            builder.Property(_ => _.Department).HasMaxLength(100);

            builder.Property(_ => _.Phone).HasMaxLength(15);
            builder.Property(_ => _.InternalPhone).HasMaxLength(20);

            builder.Property(_ => _.ImagePath).HasMaxLength(300);

            builder.Property(x => x.CreatedDate).HasDefaultValue(DateTime.Now);

            builder.HasMany(_ => _.TransactionHistories)
                   .WithOne(_ => _.AppUser)
                   .HasForeignKey(_ => _.AppUserId);
        }
    }
}
