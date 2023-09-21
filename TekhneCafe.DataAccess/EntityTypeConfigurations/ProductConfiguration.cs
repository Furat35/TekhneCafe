using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.DataAccess.EntityTypeConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.ToTable(_ => _.HasCheckConstraint("Product_Price_NonNegative", "Price >= 0"));
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Name).IsRequired().HasMaxLength(100);

            builder.Property(_ => _.Price).IsRequired();

            builder.Property(_ => _.CreatedDate).IsRequired();
            builder.Property(_ => _.Description).HasMaxLength(200);

            builder.Property(_ => _.ImagePath).HasMaxLength(300);

            builder.HasMany(_ => _.ProductAttributes)
                   .WithOne(_ => _.Product)
                   .HasForeignKey(_ => _.ProductId);

            builder.HasOne(_ => _.Category)
                .WithMany(_ => _.Products)
                .HasForeignKey(_ => _.CategoryId);
        }
    }
}
