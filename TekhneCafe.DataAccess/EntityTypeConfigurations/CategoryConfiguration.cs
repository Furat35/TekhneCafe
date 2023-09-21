using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.DataAccess.EntityTypeConfigurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Name)
                .IsRequired()
                .HasMaxLength(150);


            builder.HasMany(_ => _.Products)
                 .WithOne(_ => _.Category)
                 .HasForeignKey(_ => _.CategoryId);

            builder.HasIndex(_ => _.Name).IsUnique();
        }
    }
}
