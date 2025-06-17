using EShop.Basket.Domain.Basket.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Basket.DataSource.Configurations;

/// <summary>
/// Configures the entity properties and constraints for <see cref="ShoppingCartEntity"/>.
/// </summary>
public class ShoppingCartConfiguration: IEntityTypeConfiguration<ShoppingCartEntity>
{
    /// <summary>
    /// Configures the schema for the <see cref="ShoppingCartEntity"/> table.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    /// <remarks>
    /// Defines primary key, one-to-many foreign key,
    /// index, required fields, maximum lengths, and data types for the shopping cart entity.
    /// </remarks>
    public void Configure(EntityTypeBuilder<ShoppingCartEntity> builder)
    {
        builder.HasKey(s => s.Id);
        builder.HasIndex(s => s.UserName).IsUnique();
        builder.Property(s => s.UserName).HasMaxLength(100).IsRequired();
        builder.HasMany(s => s.Items)
            .WithOne()
            .HasForeignKey(s => s.ShoppingCartId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}