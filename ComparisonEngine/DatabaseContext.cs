using Domain;
using Microsoft.EntityFrameworkCore;

namespace ComparisonEngine
{
    public class DatabaseContext : DbContext
    {
        private const string _schema = "comparison_engine";
        public DbSet<Product> Products { get; set; }
        public DbSet<RealProduct> RealProducts { get; set; }
        public DbSet<EShop> EShops { get; set; }
        public DbSet<Attribute> Attributes { get; set; }
        public DbSet<ProductAttribute> ProductsAttributes { get; set; }

        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=postgres");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products", _schema);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityAlwaysColumn(); 

                entity.HasIndex(e => e.Title)
                    .IsUnique();
            });

            modelBuilder.Entity<ProductAttribute>(entity =>
            {
                entity.ToTable("products_attributes", _schema);
                entity.HasKey(e => new { e.ProductId, e.AttributeId });

                entity.HasOne(e => e.Product)
                    .WithMany(e => e.Attributes)
                    .HasForeignKey(e => e.ProductId);

                entity.HasOne(e => e.Attribute)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.AttributeId);
            });

            modelBuilder.Entity<Attribute>(entity =>
            {
                entity.ToTable("attributes", _schema);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.HasIndex(e => new { e.Name, e.Value })
                    .IsUnique();
            });

            modelBuilder.Entity<RealProduct>(entity =>
            {
                entity.ToTable("real_products", _schema);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.HasOne(e => e.AbstractProduct)
                    .WithMany(e => e.RealProducts)
                    .HasForeignKey(e => e.AbstractProductId);

                //entity.HasOne(e => e.EShop)
                //    .WithMany(e => e.RealProducts)
                //    .HasForeignKey(e => e.EShopId);
            });

            modelBuilder.Entity<EShop>(entity =>
            {
                entity.ToTable("eshops", _schema);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.HasIndex(e => e.Title)
                    .IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
