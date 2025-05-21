using System.Reflection;
using EShop.Catalog.Products.Entities;
using Microsoft.EntityFrameworkCore;

namespace EShop.Catalog.DataSource;

/// <summary>
/// Represents the Entity Framework database context for the catalog service.
/// </summary>
/// <param name="options">The options to configure the database context.</param>
public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets the <see cref="DbSet{TEntity}"/> for accessing <see cref="ProductEntity"/> records.
    /// </summary>
    public DbSet<ProductEntity> Products => Set<ProductEntity>();
    
    /// <summary>
    /// Configures the model for the context using Fluent API.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
    /// <remarks>
    /// Sets the default schema to "catalog" and applies all entity configurations from the current assembly.
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("catalog");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}