using Microsoft.EntityFrameworkCore;
using SmartPOS.Domain.Entities;

namespace SmartPOS.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }
        public DbSet<Return> Returns { get; set; }
        public DbSet<ReturnItem> ReturnItems { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.CostPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Sale>()
                .Property(s => s.GrandTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Sale>()
                .Property(s => s.Discount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Sale>()
                .Property(s => s.Tax)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Sale>()
                .Property(s => s.NetTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Sale>()
                .Property(s => s.PaidAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Sale>()
                .Property(s => s.DueAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SaleItem>()
                .Property(s => s.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SaleItem>()
                .Property(s => s.TotalPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SaleItem>()
                .Property(s => s.Profit)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Purchase>()
                .Property(p => p.GrandTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseItem>()
                .Property(p => p.CostPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseItem>()
                .Property(p => p.TotalPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Return>()
                .Property(r => r.RefundAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ReturnItem>()
                .Property(r => r.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ReturnItem>()
                .Property(r => r.TotalPrice)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}