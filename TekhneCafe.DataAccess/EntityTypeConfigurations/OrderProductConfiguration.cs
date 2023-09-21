using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.DataAccess.EntityTypeConfigurations
{
    public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.ToTable("OrderProduct");
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.ProductId).IsRequired();
            builder.Property(_ => _.Name).IsRequired().HasMaxLength(100);
            builder.Property(_ => _.Price).IsRequired();
            builder.Property(_ => _.Quantity).IsRequired();
            builder.Property(_ => _.Description).IsRequired().HasMaxLength(200);

            builder.HasOne(_ => _.Order)
                   .WithMany(_ => _.OrderProducts)
                   .HasForeignKey(_ => _.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(_ => _.OrderProductAttributes)
                   .WithOne(_ => _.OrderProduct)
                   .HasForeignKey(_ => _.OrderProductId);
        }
    }
}
