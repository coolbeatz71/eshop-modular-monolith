using EShop.Shared.Domain;

namespace EShop.Basket.Domain.Basket.Entities;

public class ShoppingCartEntity: Aggregate<Guid>
{
    public string UserName { get; private set; } = null!;
    
    private readonly List<ShoppingCartItemEntity> _items = [];
    
    public IReadOnlyList<ShoppingCartItemEntity> Items => _items.AsReadOnly();

    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

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

    public void AddItem()
    {
        
    }
}