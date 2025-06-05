namespace EShop.Basket.Domain.Basket.Entities;

public class ShoppingCartItemEntity
{
    public Guid ShoppingCartId { get; private set; } = Guid.Empty!;
    
    public Guid ProductId { get; private set; } = Guid.Empty!;
    
    public int Quantity { get; internal set; } = 0!;
    
    public string Color { get; private set; } = null!;

    // will come from Catalog module
    public decimal Price { get; private set; } = 0!;
    
    public string ProductName { get; private set; } = null!;
}