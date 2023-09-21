using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.DataAccess.EntityTypeConfigurations
{
    public class ProductAttributeConfiguration : IEntityTypeConfiguration<ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductAttribute> builder)
        {
            builder.ToTable("ProductAttribute");
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.ProductId).IsRequired();
            builder.Property(_ => _.AttributeId).IsRequired();
            builder.Property(_ => _.IsRequired).IsRequired();
            builder.Property(_ => _.IsDeleted).IsRequired();

            builder.HasOne(_ => _.Product)
                   .WithMany(_ => _.ProductAttributes)
                   .HasForeignKey(_ => _.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(_ => _.Attribute)
                   .WithMany(_ => _.ProductAttributes)
                   .HasForeignKey(_ => _.AttributeId);
        }
    }
}
