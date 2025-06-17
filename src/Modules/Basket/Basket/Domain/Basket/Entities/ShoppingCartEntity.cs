using EShop.Shared.Domain;

namespace EShop.Basket.Domain.Basket.Entities;

/// <summary>
/// Represents a shopping cart associated with a specific user.
/// </summary>
public class ShoppingCartEntity: Aggregate<Guid>
{
    /// <summary>
    /// Gets the username associated with the shopping cart.
    /// </summary>
    public string UserName { get; private set; } = null!;
    
    private readonly List<ShoppingCartItemEntity> _items = [];
    
    /// <summary>
    /// Gets the read-only list of items in the shopping cart.
    /// </summary>
    public IReadOnlyList<ShoppingCartItemEntity> Items => _items.AsReadOnly();
    
    /// <summary>
    /// Gets the total price of all items in the shopping cart.
    /// </summary>
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
    
    /// <summary>
    /// Creates a new <see cref="ShoppingCartEntity"/> with the specified ID and username.
    /// </summary>
    /// <param name="id">The unique identifier for the cart.</param>
    /// <param name="userName">The username associated with the cart.</param>
    /// <returns>A new instance of <see cref="ShoppingCartEntity"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="userName"/> is null or empty.</exception>
    /// <example>
    /// <code>
    /// var cart = ShoppingCartEntity.Create(Guid.NewGuid(), "john_doe");
    /// </code>
    /// </example>
    public static ShoppingCartEntity Create(Guid id, string userName)
    {
        ArgumentException.ThrowIfNullOrEmpty(userName);

        var shoppingCart = new ShoppingCartEntity
        {
            Id = id,
            UserName = userName
        };
        
        return shoppingCart;
    }
    
    /// <summary>
    /// Adds a product item to the shopping cart or updates the quantity if it already exists.
    /// </summary>
    /// <param name="productId">The unique identifier for the product.</param>
    /// <param name="quantity">The quantity of the product to add.</param>
    /// <param name="color">The color of the product.</param>
    /// <param name="price">The price per unit of the product.</param>
    /// <param name="productName">The name of the product.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="quantity"/> or <paramref name="price"/> is zero or negative.</exception>
    /// <example>
    /// <code>
    /// cart.AddItem(productId, 2, "Red", 49.99m, "Wireless Mouse");
    /// </code>
    /// </example>
    public void AddItem(
        Guid productId, 
        int quantity, 
        string color, 
        decimal price, 
        string productName
    )
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
        
        var existingItem = Items.FirstOrDefault(x => x.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            var newItem = new ShoppingCartItemEntity(Id, productId, quantity, color, price, productName)
            {
                Id = Guid.Empty
            };
            _items.Add(newItem);
        }
    }
    
    /// <summary>
    /// Removes a product item from the shopping cart by product ID.
    /// </summary>
    /// <param name="productId">The ID of the product to remove.</param>
    /// <example>
    /// <code>
    /// cart.RemoveItem(productId);
    /// </code>
    /// </example>
    public void RemoveItem(Guid productId)
    {
        var existingItem = Items.FirstOrDefault(x => x.ProductId == productId);

        if (existingItem != null)
        {
            _items.Remove(existingItem);
        }
    }
}