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

    public void RemoveItem(Guid productId)
    {
        var existingItem = Items.FirstOrDefault(x => x.ProductId == productId);

        if (existingItem != null)
        {
            _items.Remove(existingItem);
        }
    }
}