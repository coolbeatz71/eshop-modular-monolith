using System.Text.Json.Serialization;
using EShop.Shared.Domain;

namespace EShop.Basket.Domain.Basket.Entities;

/// <summary>
/// Represents a product item in the shopping cart.
/// </summary>
public class ShoppingCartItemEntity: Entity<Guid>
{
    /// <summary>
    /// Gets the ID of the shopping cart this item belongs to.
    /// </summary>
    public Guid ShoppingCartId { get; private set; }
    
    /// <summary>
    /// Gets the ID of the product.
    /// </summary>
    public Guid ProductId { get; private set; }
    
    /// <summary>
    /// Gets or sets the quantity of the product in the cart.
    /// </summary>
    public int Quantity { get; internal set; }
    
    /// <summary>
    /// Gets the color of the product.
    /// </summary>
    public string Color { get; private set; }

    /// <summary>
    /// Gets the unit price of the product. Value is expected to come from the Catalog module.
    /// </summary>
    public decimal Price { get; private set; }
    
    /// <summary>
    /// Gets the name of the product. Value is expected to come from the Catalog module.
    /// </summary>
    public string ProductName { get; private set; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ShoppingCartItemEntity"/> class with internal access.
    /// </summary>
    /// <param name="shoppingCartId">The ID of the associated shopping cart.</param>
    /// <param name="productId">The ID of the product.</param>
    /// <param name="quantity">The quantity of the product.</param>
    /// <param name="color">The color of the product.</param>
    /// <param name="price">The price per unit.</param>
    /// <param name="productName">The name of the product.</param>
    internal ShoppingCartItemEntity(
        Guid shoppingCartId,
        Guid productId,
        int quantity,
        string color,
        decimal price,
        string productName
    )
    {
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        Quantity = quantity;
        Color = color;
        Price = price;
        ProductName = productName;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ShoppingCartItemEntity"/> class for JSON deserialization.
    /// </summary>
    /// <param name="id">The entity ID.</param>
    /// <param name="shoppingCartId">The ID of the associated shopping cart.</param>
    /// <param name="productId">The ID of the product.</param>
    /// <param name="quantity">The quantity of the product.</param>
    /// <param name="color">The color of the product.</param>
    /// <param name="price">The price per unit.</param>
    /// <param name="productName">The name of the product.</param>
    [JsonConstructor]
    public ShoppingCartItemEntity(
        Guid id, 
        Guid shoppingCartId, 
        Guid productId, 
        int quantity, 
        string color, 
        decimal price, 
        string productName
    )
    {
        Id = id;
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        Quantity = quantity;
        Color = color;
        Price = price;
        ProductName = productName;
    }
    
    /// <summary>
    /// Updates the price of the product in the cart.
    /// </summary>
    /// <param name="newPrice">The new price to set.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="newPrice"/> is less than or equal to zero.</exception>
    /// <example>
    /// <code>
    /// item.UpdatePrice(59.99m);
    /// </code>
    /// </example>
    public void UpdatePrice(decimal newPrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(newPrice);
        Price = newPrice;
    }
}