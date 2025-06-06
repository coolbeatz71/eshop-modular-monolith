using EShop.Catalog.Products.Entities;
using EShop.Shared.Domain;

namespace EShop.Catalog.Products.Events;

/// <summary>
/// Represents a domain event that is raised when the price of a <see cref="ProductEntity"/> changes.
/// </summary>
/// <param name="ProductEntity">The product entity whose price was updated.</param>
/// <remarks>
/// This event can be used to trigger updates in dependent systems, such as shopping baskets or pricing history logs.
/// </remarks>
/// <example>
/// <code>
/// product.Update("Laptop", "Desc", "img.jpg", 1499.99m, new List&lt;string&gt; { "Electronics" });
/// var domainEvent = new ProductPriceChangedEvent(product);
/// </code>
/// </example>
public record ProductPriceChangedEvent(ProductEntity ProductEntity) : IDomainEvent;