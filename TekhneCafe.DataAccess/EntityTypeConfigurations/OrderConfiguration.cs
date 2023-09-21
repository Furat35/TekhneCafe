using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.DataAccess.EntityTypeConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");
            builder.ToTable(_ => _.HasCheckConstraint("Order_Price_NonNegative", "TotalPrice >= 0"));
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.TotalPrice).IsRequired();
            builder.Property(_ => _.CreatedDate).IsRequired();
            builder.Property(_ => _.Description).HasMaxLength(200);

            builder.Property(_ => _.AppUserId).IsRequired();

            builder.HasMany(_ => _.OrderHistories)
                   .WithOne(_ => _.Order)
                   .HasForeignKey(_ => _.OrderId);

            builder.HasMany(_ => _.TransactionHistories)
                   .WithOne(_ => _.Order)
                   .HasForeignKey(_ => _.OrderId);

            builder.HasMany(_ => _.OrderProducts)
                   .WithOne(_ => _.Order)
                   .HasForeignKey(_ => _.OrderId);
        }
    }
}
