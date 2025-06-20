namespace EShop.Basket.Domain.Basket.Dtos;

/// <summary>
/// Data Transfer Object representing an item in a shopping cart.
/// </summary>
public record ShoppingCartItemDto
{
    /// <summary>
    /// Unique identifier for the shopping cart item.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Identifier of the shopping cart this item belongs to.
    /// </summary>
    public Guid ShoppingCartId { get; init; }

    /// <summary>
    /// Identifier of the product being added to the cart.
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// Quantity of the product in the cart.
    /// </summary>
    public int Quantity { get; init; }

    /// <summary>
    /// Selected color or variant of the product.
    /// </summary>
    public string Color { get; init; } = null!;

    /// <summary>
    /// Price per unit of the product.
    /// </summary>
    public decimal Price { get; init; }

    /// <summary>
    /// Display name or title of the product.
    /// </summary>
    public string ProductName { get; init; } = null!;
}
