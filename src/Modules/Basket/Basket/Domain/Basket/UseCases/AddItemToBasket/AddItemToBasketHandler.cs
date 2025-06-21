using EShop.Basket.Domain.Basket.Dtos;
using EShop.Basket.Domain.Basket.Repositories;
using EShop.Catalog.Domain.Products.UseCases.GetProductById;
using EShop.Shared.CQRS;
using MediatR;

namespace EShop.Basket.Domain.Basket.UseCases.AddItemToBasket;

/// <summary>
/// Command to add an item to a shopping basket for a specific user.
/// </summary>
/// <param name="UserName">The authenticated user's username.</param>
/// <param name="ShoppingCartItem">The item to be added to the shopping cart.</param>
/// <example>
/// Example usage:
/// <code>
/// var item = new ShoppingCartItemDto
/// {
///     ProductId = Guid.Parse("8a1d9f4a-bc66-47ab-91a3-c24566718b5b"),
///     Quantity = 2,
///     Color = "Red"
/// };
///
/// var command = new AddItemToBasketCommand("john.doe", item);
/// var result = await mediator.Send(command);
///
/// Console.WriteLine($"Basket ID: {result.Id}");
/// </code>
/// </example>
public record AddItemToBasketCommand(
    string UserName, 
    ShoppingCartItemDto ShoppingCartItem
): ICommand<AddItemToBasketResult>;

/// <summary>
/// Result returned after adding an item to the basket.
/// </summary>
/// <param name="Id">The unique identifier of the user's basket.</param>
public record AddItemToBasketResult(Guid Id);

/// <summary>
/// Handles the logic for adding an item to a user's basket.
/// </summary>
/// <remarks>
/// - Retrieves the user's basket, or creates one if it doesn't exist.
/// - Fetches product details (name, price) from the Catalog module.
/// - Adds the item to the basket and saves changes.
///
/// Example request handler setup:
/// <code>
/// var handler = new AddItemToBasketHandler(repository, sender);
/// var result = await handler.Handle(command, CancellationToken.None);
/// </code>
/// </remarks>
public class AddItemToBasketHandler(IBasketRepository repository, ISender sender)
    : ICommandHandler<AddItemToBasketCommand, AddItemToBasketResult>
{
    /// <summary>
    /// Handles the <see cref="AddItemToBasketCommand"/> by adding an item to the basket
    /// and retrieving the latest product info from the Catalog service.
    /// </summary>
    /// <param name="command">The command containing the username and item to add.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    /// <returns>The result containing the basket's ID.</returns>
    public async Task<AddItemToBasketResult> Handle(AddItemToBasketCommand command, CancellationToken cancellationToken)
    {
        // Add shopping cart item into shopping cart
        var shoppingCart = await repository.GetBasket(command.UserName, false, cancellationToken);
        
        // Before AddItem into ShoppingCart, we should call Catalog Module GetProductByIdQuery method
        // Get latest product information and set Price and ProductName when adding item into Basket
        var productQuery = new GetProductByIdQuery(command.ShoppingCartItem.ProductId.ToString());
        var result = await sender.Send(productQuery, cancellationToken);
        
        shoppingCart.AddItem(
            command.ShoppingCartItem.ProductId,
            command.ShoppingCartItem.Quantity,
            command.ShoppingCartItem.Color,
            result.Product.Price,
            result.Product.Name
        );
        
        // Save updated basket
        await repository.SaveChangesAsync(command.UserName, cancellationToken);
        
        return new AddItemToBasketResult(shoppingCart.Id);
    }
}