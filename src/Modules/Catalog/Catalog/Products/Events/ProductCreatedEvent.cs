using EShop.Catalog.Products.Entities;
using EShop.Shared.Domain;

namespace EShop.Catalog.Products.Events;

public record ProductCreatedEvent(ProductEntity ProductEntity): IDomainEvent;