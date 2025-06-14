using EShop.Basket.Domain.Basket.Dtos;

namespace EShop.Basket.Domain.Basket.Repositories;

public interface IBasketRepository
{
    Task<ShoppingCartDto> GetBasket(
        string userName, 
        bool asNoTracking = true, 
        CancellationToken cancellationToken = default
    );
    
    Task<ShoppingCartDto> CreateBasket(ShoppingCartDto basket, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default);
    
    Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default);
}