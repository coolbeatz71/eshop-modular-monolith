using EShop.Basket.Domain.Basket.Entities;
using EShop.Shared.Domain;
using EShop.Shared.Domain.Specifications;

namespace EShop.Basket.Domain.Basket.Repositories;

public interface IBasketRepository : IRepository<ShoppingCartEntity>
{
    Task<ShoppingCartEntity> GetBasket(
        Specification<ShoppingCartEntity> specification, 
        bool asNoTracking = true, 
        CancellationToken cancellationToken = default
    );
    
    Task<ShoppingCartEntity> CreateBasket(ShoppingCartEntity basket, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteBasket(Specification<ShoppingCartEntity> specification, CancellationToken cancellationToken = default);
    
    Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default);
}