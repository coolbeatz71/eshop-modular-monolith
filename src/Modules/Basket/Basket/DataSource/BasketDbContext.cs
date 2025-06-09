using System.Reflection;
using EShop.Basket.Domain.Basket.Entities;
using Microsoft.EntityFrameworkCore;

namespace EShop.Basket.DataSource;

/// <summary>
/// Represents the Entity Framework database context for the Basket module.
/// </summary>
public class BasketDbContext(DbContextOptions<BasketDbContext> options): DbContext(options)
{
    /// <summary>
    /// Gets the <see cref="DbSet{TEntity}"/> for <see cref="ShoppingCartEntity"/>.
    /// </summary>
    public DbSet<ShoppingCartEntity> ShoppingCarts => Set<ShoppingCartEntity>();
    
    /// <summary>
    /// Gets the <see cref="DbSet{TEntity}"/> for <see cref="ShoppingCartItemEntity"/>.
    /// </summary>
    public DbSet<ShoppingCartItemEntity> ShoppingCartItems => Set<ShoppingCartItemEntity>();
    
    /// <summary>
    /// Configures the model for the context using Fluent API.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
    /// <remarks>
    /// Sets the default schema to "basket" and applies all entity configurations from the current assembly.
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("basket");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}