using EShop.Basket.Domain.Basket.Entities;
using EShop.Basket.Domain.Basket.Repositories;
using EShop.Shared.DataSource.Extensions;
using EShop.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EShop.Basket.DataSource.Repositories;

/// <summary>
/// Repository implementation for managing shopping baskets in the database.
/// </summary>
/// <param name="dbContext">The basket database context used for data access.</param>
public class BasketRepository(BasketDbContext dbContext) : IBasketRepository
{
    /// <summary>
    /// Retrieves a user's basket by username. Throws a <see cref="NotFoundException"/> if not found.
    /// </summary>
    /// <param name="userName">The username associated with the basket.</param>
    /// <param name="asNoTracking">Whether to use AsNoTracking for read-only performance optimization.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The user's <see cref="ShoppingCartEntity"/>.</returns>
    /// <exception cref="NotFoundException">Thrown if no basket is found for the specified username.</exception>
    /// <example>
    /// <code>
    /// var basket = await basketRepository.GetBasket("john.doe");
    /// </code>
    /// </example>
    public async Task<ShoppingCartEntity> GetBasket(
        string userName,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        var query = dbContext.ShoppingCarts
            .Include(x => x.Items)
            .Where(x => x.UserName == userName);

        var basket = await query.SingleDefaultOrThrowAsync(
            asNoTracking: asNoTracking,
            cancellationToken: cancellationToken,
            keyName: "username",
            keyValue: userName
        );

        return basket;
    }

    /// <summary>
    /// Creates a new basket and persists it to the database.
    /// </summary>
    /// <param name="basket">The <see cref="ShoppingCartEntity"/> to create.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The newly created basket entity.</returns>
    /// <example>
    /// <code>
    /// var newBasket = ShoppingCartEntity.Create(Guid.NewGuid(), "john.doe");
    /// var created = await basketRepository.CreateBasket(newBasket);
    /// </code>
    /// </example>
    public async Task<ShoppingCartEntity> CreateBasket(
        ShoppingCartEntity basket,
        CancellationToken cancellationToken = default
    )
    {
        dbContext.ShoppingCarts.Add(basket);
        await dbContext.SaveChangesAsync(cancellationToken);
        return basket;
    }

    /// <summary>
    /// Deletes a user's basket based on their username.
    /// </summary>
    /// <param name="userName">The username whose basket is to be deleted.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns><c>true</c> if the basket was successfully deleted.</returns>
    /// <exception cref="NotFoundException">Thrown if no basket is found for the specified username.</exception>
    /// <example>
    /// <code>
    /// var deleted = await basketRepository.DeleteBasket("john.doe");
    /// </code>
    /// </example>
    public async Task<bool> DeleteBasket(
        string userName,
        CancellationToken cancellationToken = default
    )
    {
        var basket = await GetBasket(userName, false, cancellationToken);

        dbContext.ShoppingCarts.Remove(basket);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    /// <param name="userName">Optional username (reserved for future auditing/logging).</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The number of state entries written to the database.</returns>
    /// <example>
    /// <code>
    /// var affected = await basketRepository.SaveChangesAsync();
    /// </code>
    /// </example>
    public async Task<int> SaveChangesAsync(
        string? userName = null,
        CancellationToken cancellationToken = default
    )
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}
