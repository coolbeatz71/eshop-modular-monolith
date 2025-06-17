using EShop.Basket.Domain.Basket.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Basket.DataSource.Configurations;

/// <summary>
/// Configures the entity properties and constraints for <see cref="ShoppingCartItemEntity"/>.
/// </summary>
public class ShoppingCartItemConfiguration: IEntityTypeConfiguration<ShoppingCartItemEntity>
{
    /// <summary>
    /// Configures the schema for the <see cref="ShoppingCartItemEntity"/> table.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    /// <remarks>
    /// Defines primary key, required fields, maximum lengths, and data types
    /// for the shopping cart item entity.
    /// </remarks>
    public void Configure(EntityTypeBuilder<ShoppingCartItemEntity> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.ProductId).IsRequired();
        builder.Property(i => i.Quantity).IsRequired();
        builder.Property(i => i.Color).HasMaxLength(30);
        builder.Property(i => i.Price).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(i => i.ProductName).HasMaxLength(100).IsRequired();
    }
}