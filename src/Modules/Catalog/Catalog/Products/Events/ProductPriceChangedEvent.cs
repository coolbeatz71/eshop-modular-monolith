using EShop.Catalog.Products.Models;
using EShop.Shared.Domain;

namespace EShop.Catalog.Products.Events;

public record ProductPriceChangedEvent(Product Product): IDomainEvent;