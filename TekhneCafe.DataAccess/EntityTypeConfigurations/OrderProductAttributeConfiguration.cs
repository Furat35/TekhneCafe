using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.DataAccess.EntityTypeConfigurations
{
    public class OrderProductAttributeConfiguration : IEntityTypeConfiguration<OrderProductAttribute>
    {
        public void Configure(EntityTypeBuilder<OrderProductAttribute> builder)
        {
            builder.ToTable("OrderProductAttribute");
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.ProductAttributeId).IsRequired();
            builder.Property(_ => _.Name).IsRequired().HasMaxLength(100);
            builder.Property(_ => _.Price).IsRequired();
            builder.Property(_ => _.Quantity).IsRequired();

            builder.HasOne(_ => _.OrderProduct)
                   .WithMany(_ => _.OrderProductAttributes)
                   .HasForeignKey(_ => _.OrderProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
