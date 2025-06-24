using EShop.Basket.Domain.Basket.Entities;
using EShop.Shared.Domain;

namespace EShop.Basket.Domain.Basket.Repositories;

public interface IBasketRepository : IRepository<ShoppingCartEntity>
{
    Task<ShoppingCartEntity> GetBasket(
        string userName, 
        bool asNoTracking = true, 
        CancellationToken cancellationToken = default
    );
    
    Task<ShoppingCartEntity> CreateBasket(ShoppingCartEntity basket, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default);
    
    Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default);
}