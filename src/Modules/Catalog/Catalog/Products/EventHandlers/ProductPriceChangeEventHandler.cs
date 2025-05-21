using MediatR;
using Microsoft.Extensions.Logging;
using EShop.Catalog.Products.Events;

namespace EShop.Catalog.Products.EventHandlers;

/// <summary>
/// Handles the <see cref="ProductPriceChangedEvent"/> by logging the event.
/// </summary>
/// <remarks>
/// Intended for future extension where a price change integration event will be published (e.g., to update basket prices).
/// </remarks>
public class ProductPriceChangeEventHandler(ILogger<ProductPriceChangeEventHandler> logger)
    : INotificationHandler<ProductPriceChangedEvent>
{
    /// <summary>
    /// Handles the <see cref="ProductPriceChangedEvent"/> and logs its occurrence.
    /// </summary>
    /// <param name="notification">The product price changed domain event.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A completed <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <example>
    /// <code>
    /// // Triggered automatically by MediatR when the product price is updated
    /// await mediator.Publish(new ProductPriceChangedEvent(product));
    /// </code>
    /// </example>
    public Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        // TODO: needs to publish product price change integration event to update basket prices
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}