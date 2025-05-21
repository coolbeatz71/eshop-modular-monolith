using MediatR;
using Microsoft.Extensions.Logging;
using EShop.Catalog.Products.Events;

namespace EShop.Catalog.Products.EventHandlers;

/// <summary>
/// Handles the <see cref="ProductCreatedEvent"/> by logging when a new product is created.
/// </summary>
public class ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    : INotificationHandler<ProductCreatedEvent>
{
    /// <summary>
    /// Handles the <see cref="ProductCreatedEvent"/> and logs the event name.
    /// </summary>
    /// <param name="notification">The product created domain event.</param>
    /// <param name="cancellationToken">A token for cancelling the operation.</param>
    /// <returns>A completed <see cref="Task"/> representing the operation.</returns>
    /// <example>
    /// <code>
    /// // Automatically triggered by MediatR when ProductCreatedEvent is published
    /// await mediator.Publish(new ProductCreatedEvent(product));
    /// </code>
    /// </example>
    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}