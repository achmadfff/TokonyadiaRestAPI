using Microsoft.EntityFrameworkCore;
using TokonyadiaRestAPII.Entities;

namespace TokonyadiaRestAPII.Repositories;

public class AppDBContext : DbContext
{

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Store> Stores  => Set<Store>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductPrice> ProductPrices => Set<ProductPrice>();
    public DbSet<Purchase> Purchases => Set<Purchase>();
    public DbSet<PurchaseDetail> PurchaseDetails => Set<PurchaseDetail>();
    public DbSet<User> Users => Set<User>();

    protected AppDBContext()
    {
    }

    public AppDBContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasIndex(customer => customer.PhoneNumber).IsUnique();
            entity.HasIndex(customer => customer.Email).IsUnique();
        });
        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasIndex(s => s.PhoneNumber).IsUnique();
            entity.HasIndex(s => s.SiupNumber).IsUnique();
        });
    }
}