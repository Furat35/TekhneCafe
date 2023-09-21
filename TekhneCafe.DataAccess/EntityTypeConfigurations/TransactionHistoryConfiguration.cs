using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.DataAccess.EntityTypeConfigurations
{
    public class TransactionHistoryConfiguration : IEntityTypeConfiguration<TransactionHistory>
    {
        public void Configure(EntityTypeBuilder<TransactionHistory> builder)
        {
            builder.ToTable("TransactionHistory");
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Description).HasMaxLength(200);
            builder.Property(_ => _.Amount).IsRequired();
            builder.Property(_ => _.CreatedDate).IsRequired();
            builder.Property(_ => _.AppUserId).IsRequired();

            builder.HasOne(_ => _.AppUser)
                   .WithMany(_ => _.TransactionHistories)
                   .HasForeignKey(_ => _.AppUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(_ => _.Order)
                   .WithMany(_ => _.TransactionHistories)
                   .HasForeignKey(_ => _.OrderId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
