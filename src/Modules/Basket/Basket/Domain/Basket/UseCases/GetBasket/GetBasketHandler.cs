using EShop.Basket.Domain.Basket.Dtos;
using EShop.Basket.Domain.Basket.Repositories;
using EShop.Shared.CQRS;
using Mapster;

namespace EShop.Basket.Domain.Basket.UseCases.GetBasket;

/// <summary>
/// Query used to retrieve the shopping basket for a specific user.
/// </summary>
/// <param name="UserName">The username of the authenticated user.</param>
/// <remarks>
/// Example usage:
/// <code>
/// var query = new GetBasketQuery("jane.doe");
/// var result = await mediator.Send(query);
/// </code>
/// </remarks>
public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;

/// <summary>
/// Result of the <see cref="GetBasketQuery"/> containing the shopping cart.
/// </summary>
/// <param name="ShoppingCart">The shopping cart data for the user.</param>
/// <remarks>
/// Returned by the handler upon successful retrieval of the basket.
/// </remarks>
public record GetBasketResult(ShoppingCartDto ShoppingCart);

/// <summary>
/// Handles the <see cref="GetBasketQuery"/> to retrieve a user's basket.
/// </summary>
/// <param name="repository">The repository interface to access basket storage.</param>
public class GetBasketHandler(IBasketRepository repository): IQueryHandler<GetBasketQuery, GetBasketResult>
{
    /// <summary>
    /// Handles the query by fetching the user's basket and mapping it to a DTO.
    /// </summary>
    /// <param name="query">The query containing the username.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A <see cref="GetBasketResult"/> containing the shopping cart DTO.</returns>
    /// <exception cref="ArgumentNullException">Thrown if no basket is found for the username.</exception>
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        // Get basket by userName
        var basket = await repository.GetBasket(query.UserName, true, cancellationToken);
        
        // Map basket entity to ShoppingCartDto
        var shoppingCartDto = basket.Adapt<ShoppingCartDto>();
        
        return new GetBasketResult(shoppingCartDto);
    }
}
