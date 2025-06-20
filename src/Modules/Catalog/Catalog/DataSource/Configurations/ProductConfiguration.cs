using EShop.Catalog.Domain.Products.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Catalog.DataSource.Configurations;

/// <summary>
/// Configures the entity properties and constraints for <see cref="ProductEntity"/>.
/// </summary>
public class ProductConfiguration: IEntityTypeConfiguration<ProductEntity>
{
    /// <summary>
    /// Configures the schema for the <see cref="ProductEntity"/> table.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    /// <remarks>
    /// Defines primary key, required fields, maximum lengths, and data types for the product entity.
    /// </remarks>
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Category).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(200).IsRequired();
        builder.Property(p => p.ImageFile).HasMaxLength(100);
        builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
    }
}