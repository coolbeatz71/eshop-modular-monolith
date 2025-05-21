using EShop.Catalog.Products.Entities;
using EShop.Shared.Domain;

namespace EShop.Catalog.Products.Events;

/// <summary>
/// Represents a domain event that is raised when a new <see cref="ProductEntity"/> is created.
/// </summary>
/// <param name="ProductEntity">The newly created product entity.</param>
/// <remarks>
/// This event is typically published to notify other parts of the system that a new product has been added to the catalog.
/// </remarks>
/// <example>
/// <code>
/// var product = ProductEntity.Create(...);
/// var domainEvent = new ProductCreatedEvent(product);
/// </code>
/// </example>
public record ProductCreatedEvent(ProductEntity ProductEntity) : IDomainEvent;