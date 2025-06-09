using System.Text.Json.Serialization;
using EShop.Shared.Domain;

namespace EShop.Basket.Domain.Basket.Entities;

public class ShoppingCartItemEntity: Entity<Guid>
{
    public Guid ShoppingCartId { get; private set; }

    public Guid ProductId { get; private set; }
    
    public int Quantity { get; internal set; }
    
    public string Color { get; private set; }

    // will come from Catalog module
    public decimal Price { get; private set; }
    
    public string ProductName { get; private set; }

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

    public void UpdatePrice(decimal newPrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(newPrice);
        Price = newPrice;
    }
}