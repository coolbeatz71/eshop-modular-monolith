namespace EShop.Basket.Domain.Basket.Dtos;

/// <summary>
/// Data Transfer Object representing a user's shopping cart.
/// </summary>
public record ShoppingCartDto
{
    /// <summary>
    /// Unique identifier for the shopping cart.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Username associated with the shopping cart.
    /// </summary>
    public string UserName { get; init; } = null!;

    /// <summary>
    /// List of items currently in the shopping cart.
    /// </summary>
    public List<ShoppingCartItemDto> Items { get; init; } = [];
}
