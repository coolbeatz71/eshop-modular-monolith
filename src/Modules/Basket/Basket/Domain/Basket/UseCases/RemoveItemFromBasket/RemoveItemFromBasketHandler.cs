using EShop.Basket.Domain.Basket.Repositories;
using EShop.Shared.CQRS;

namespace EShop.Basket.Domain.Basket.UseCases.RemoveItemFromBasket;

/// <summary>
/// Represents a command to remove a specific item from a user's basket.
/// </summary>
/// <param name="UserName">The username of the user whose basket is being modified.</param>
/// <param name="ProductId">The unique identifier of the product to remove from the basket.</param>
/// <example>
/// Example usage:
/// <code>
/// var command = new RemoveItemFromBasketCommand("john.doe", Guid.Parse("a3b14dbe-521f-4ae6-9c75-78f9f5e5c2a2"));
/// var result = await sender.Send(command);
/// </code>
/// </example>
public record RemoveItemFromBasketCommand(string UserName, Guid ProductId)
    : ICommand<RemoveItemFromBasketResult>;

/// <summary>
/// Represents the result of a remove item operation from a shopping basket.
/// </summary>
/// <param name="Id">The identifier of the updated shopping cart after item removal.</param>
/// <example>
/// Example response:
/// <code>
/// var response = new RemoveItemFromBasketResult(Guid.Parse("a3b14dbe-521f-4ae6-9c75-78f9f5e5c2a2"));
/// </code>
/// </example>
public record RemoveItemFromBasketResult(Guid Id);

/// <summary>
/// Handles the <see cref="RemoveItemFromBasketCommand"/> by removing the specified item from the user's basket.
/// </summary>
public class RemoveItemFromBasketHandler(IBasketRepository repository)
    : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
{
    /// <summary>
    /// Executes the logic to remove an item from the shopping basket.
    /// </summary>
    /// <param name="command">The command containing the user and product identifiers.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>
    /// A <see cref="RemoveItemFromBasketResult"/> containing the updated basket ID after item removal.
    /// </returns>
    /// <example>
    /// Example handler usage:
    /// <code>
    /// var command = new RemoveItemFromBasketCommand("john.doe", productId);
    /// var result = await handler.Handle(command, CancellationToken.None);
    /// Console.WriteLine(result.Id);
    /// </code>
    /// </example>
    public async Task<RemoveItemFromBasketResult> Handle(
        RemoveItemFromBasketCommand command,
        CancellationToken cancellationToken
    )
    {
        var shoppingCart = await repository.GetBasket(command.UserName, false, cancellationToken);

        shoppingCart.RemoveItem(command.ProductId);

        await repository.SaveChangesAsync(command.UserName, cancellationToken);

        return new RemoveItemFromBasketResult(shoppingCart.Id);
    }
}
