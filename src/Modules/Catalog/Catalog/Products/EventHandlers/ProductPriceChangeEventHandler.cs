using MediatR;
using Microsoft.Extensions.Logging;

using EShop.Catalog.Products.Events;

namespace EShop.Catalog.Products.EventHandlers;

public class ProductPriceChangeEventHandler(ILogger<ProductPriceChangeEventHandler> logger)
    : INotificationHandler<ProductPriceChangedEvent>
{
    public Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        // TODO: needs to publish product price change integration event to update basket prices
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}