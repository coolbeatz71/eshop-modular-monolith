using System.Text.Json;
using System.Text.Json.Serialization;
using EShop.Basket.DataSource.JsonConverters;
using EShop.Basket.DataSource.Specifications;
using EShop.Basket.Domain.Basket.Entities;
using EShop.Basket.Domain.Basket.Repositories;
using EShop.Shared.Domain.Specifications;
using Microsoft.Extensions.Caching.Distributed;

namespace EShop.Basket.DataSource.Repositories;

public class CachedBasketRepository(
    IBasketRepository repository,
    IDistributedCache cache
): IBasketRepository
{
    
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new ShoppingCartConverter(), new ShoppingCartItemConverter() }
    };
    
    public async Task<ShoppingCartEntity> GetBasket(
        Specification<ShoppingCartEntity> specification, 
        bool asNoTracking = true, 
        CancellationToken cancellationToken = default
    )
    {
        if (!asNoTracking)
        {
            return await repository.GetBasket(specification, false, cancellationToken);
        }
        
        var userName = ExtractUserName(specification);
        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket))
        {            
            return JsonSerializer.Deserialize<ShoppingCartEntity>(cachedBasket, _options)!;
        }            

        var basket = await repository.GetBasket(specification, asNoTracking, cancellationToken);
        
        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket, _options), cancellationToken);
        
        return basket;
    }

    public async Task<ShoppingCartEntity> CreateBasket(
        ShoppingCartEntity basket, 
        CancellationToken cancellationToken = default
    )
    {
        await repository.CreateBasket(basket, cancellationToken);
        await cache.SetStringAsync(
            basket.UserName, 
            JsonSerializer.Serialize(basket, _options), 
            cancellationToken
        );

        return basket;
    }

    public async Task<bool> DeleteBasket(
        Specification<ShoppingCartEntity> specification, 
        CancellationToken cancellationToken = default
    )
    {
        await repository.DeleteBasket(specification, cancellationToken);
        
        var userName = ExtractUserName(specification);
        await cache.RemoveAsync(userName, cancellationToken);

        return true;
    }

    public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default)
    {
        var result = await repository.SaveChangesAsync(userName, cancellationToken);
        
        if (userName is not null)
        {
            await cache.RemoveAsync(userName, cancellationToken);
        }

        return result;
    }
    
    private static string ExtractUserName(Specification<ShoppingCartEntity> specification)
    {
        if (
            specification is not BasketByUserNameSpecification userNameSpec ||
            string.IsNullOrWhiteSpace(userNameSpec.UserName)
        )
        {
            throw new ArgumentException("Invalid specification: expected non-null username.", nameof(specification));
        }

        return userNameSpec.UserName;
    }
}