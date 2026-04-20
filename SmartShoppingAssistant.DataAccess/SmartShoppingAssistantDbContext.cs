using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess;

public class SmartShoppingAssistantDbContext(DbContextOptions<SmartShoppingAssistantDbContext> options)
    : DbContext(options)
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<ProductCategory> ProductCategories { get; set; } = null!;
    public DbSet<CartItem> CartItems { get; set; } = null!;
    public DbSet<Promotion> Promotions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartShoppingAssistantDbContext).Assembly);
    }
}