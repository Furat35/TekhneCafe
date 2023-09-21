using Microsoft.EntityFrameworkCore;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.DataAccess.Concrete.EntityFramework.Context
{
    public class EfTekhneCafeContext : DbContext
    {
        public EfTekhneCafeContext(DbContextOptions<EfTekhneCafeContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public EfTekhneCafeContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Server=192.168.254.224;Database=EfTekhneCafe;User Id=TekhneStars;Password=Tekhne1234;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfTekhneCafeContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<OrderProduct> OrderProduct { get; set; }
        public DbSet<OrderProductAttribute> OrderProductAttribute { get; set; }
        public DbSet<OrderHistory> OrderHistory { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<TekhneCafe.Entity.Concrete.Attribute> Attribute { get; set; }
        public DbSet<ProductAttribute> ProductAttribute { get; set; }
        public DbSet<TransactionHistory> TransactionHistory { get; set; }
    }
}
