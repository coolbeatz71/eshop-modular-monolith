using EShop.Basket.Domain.Basket.Dtos;
using EShop.Basket.Domain.Basket.Entities;
using EShop.Basket.Domain.Basket.Repositories;
using EShop.Catalog.Domain.Products.UseCases.GetProductById;
using EShop.Shared.CQRS;
using EShop.Shared.Exceptions;
using MediatR;

namespace EShop.Basket.Domain.Basket.UseCases.CreateBasket;

/// <summary>
/// Command to create a new basket with the provided shopping cart data.
/// </summary>
/// <example>
/// Example usage:
/// <code>
/// <![CDATA[
/// var command = new CreateBasketCommand(new ShoppingCartDto
/// {
///     UserName = "john_doe",
///     Items = new List<ShoppingCartItemDto>
///     {
///         new ShoppingCartItemDto
///         {
///             ProductId = Guid.Parse("123e4567-e89b-12d3-a456-426614174000"),
///             Quantity = 2,
///             Color = "Red"
///         }
///     }
/// });
/// ]]>
/// </code>
/// </example>
public record CreateBasketCommand(ShoppingCartDto ShoppingCart) : ICommand<CreateBasketResults>;

/// <summary>
/// Result of a successful CreateBasket operation, containing the new basket's ID.
/// </summary>
/// <param name="Id">The ID of the newly created basket.</param>
/// <example>
/// Example result:
/// <code>
/// var result = new CreateBasketResults(Guid.NewGuid());
/// Console.WriteLine(result.Id);
/// </code>
/// </example>
public record CreateBasketResults(Guid Id);

/// <summary>
/// Handler responsible for creating a shopping basket from a command.
/// </summary>
/// <param name="repository">Repository interface to persist the basket entity.</param>
/// <param name="sender">MediatR sender used to query product data from the catalog.</param>
public class CreateBasketHandler(IBasketRepository repository, ISender sender) 
    : ICommandHandler<CreateBasketCommand, CreateBasketResults>
{
    /// <summary>
    /// Handles the CreateBasket command by building and persisting a basket.
    /// </summary>
    /// <param name="command">Command containing shopping cart details.</param>
    /// <param name="cancellationToken">Cancellation token for cooperative cancellation.</param>
    /// <returns>A result containing the ID of the newly created basket.</returns>
    /// <exception cref="NotFoundException">Thrown if any product in the cart does not exist.</exception>
    /// <example>
    /// <code>
    /// var handler = new CreateBasketHandler(repository, sender);
    /// var result = await handler.Handle(command, CancellationToken.None);
    /// Console.WriteLine($"Created basket with ID: {result.Id}");
    /// </code>
    /// </example>
    public async Task<CreateBasketResults> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = await CreateBasket(command.ShoppingCart, cancellationToken);
        await repository.CreateBasket(shoppingCart, cancellationToken);

        return new CreateBasketResults(shoppingCart.Id);
    }

    /// <summary>
    /// Constructs a <see cref="ShoppingCartEntity"/> from the provided DTO,
    /// retrieving product details and adding validated items.
    /// </summary>
    /// <param name="shoppingCartDto">DTO representing user and item details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The constructed <see cref="ShoppingCartEntity"/>.</returns>
    /// <exception cref="NotFoundException">
    /// Thrown if a product referenced in the cart is not found in the catalog.
    /// </exception>
    /// <example>
    /// <code>
    /// var entity = await CreateBasket(shoppingCartDto, cancellationToken);
    /// Console.WriteLine(entity.UserName);
    /// </code>
    /// </example>
    private async Task<ShoppingCartEntity> CreateBasket(
        ShoppingCartDto shoppingCartDto,
        CancellationToken cancellationToken)
    {
        // Create a new basket entity with a generated unique ID
        var basket = ShoppingCartEntity.Create(
            Guid.NewGuid(),
            shoppingCartDto.UserName
        );

        // Add validated items to the basket by querying the catalog
        foreach (var item in shoppingCartDto.Items)
        {
            var productResult = await sender.Send(
                new GetProductByIdQuery(item.ProductId.ToString()), cancellationToken
            );

            if (productResult is null)
            {
                throw new NotFoundException(nameof(ShoppingCartEntity), item.ProductId);
            }

            basket.AddItem(
                productId: item.ProductId,
                quantity: item.Quantity,
                color: item.Color,
                price: productResult.Product.Price,
                productName: productResult.Product.Name
            );
        }

        return basket;
    }
}