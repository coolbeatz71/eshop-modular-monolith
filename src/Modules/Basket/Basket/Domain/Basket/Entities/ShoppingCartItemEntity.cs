using EShop.Shared.Domain;

namespace EShop.Basket.Domain.Basket.Entities;

public class ShoppingCartItemEntity: Entity<Guid>
{
    public Guid ShoppingCartId { get; private set; } = Guid.Empty!;
    
    public Guid ProductId { get; private set; } = Guid.Empty!;
    
    public int Quantity { get; internal set; } = 0!;
    
    public string Color { get; private set; }

    // will come from Catalog module
    public decimal Price { get; private set; } = 0!;
    
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
}