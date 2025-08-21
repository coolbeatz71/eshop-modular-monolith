using EShop.Catalog.Contracts.Domain.Products.Dtos;
using EShop.Catalog.Contracts.Domain.Products.UseCases.GetProductById;
using EShop.Catalog.DataSource;
using EShop.Shared.Contracts.CQRS;
using EShop.Shared.DataSource.Extensions;
using EShop.Shared.Exceptions;
using Mapster;

namespace EShop.Catalog.Domain.Products.UseCases.GetProductById;

/// <summary>
/// Handles the <see cref="GetProductByIdQuery"/> by retrieving and mapping the product from the database.
/// </summary>
/// <param name="dbContext">The catalog database context used for querying.</param>
/// <remarks>
/// Utilizes <c>SingleDefaultOrThrowAsync</c> to fetch the product and Mapster for mapping to DTO.
/// </remarks>
/// <example>
/// <code>
/// var handler = new GetProductByIdHandler(dbContext);
/// var result = await handler.Handle(query, CancellationToken.None);
/// </code>
/// </example>
public class GetProductByIdHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    /// <summary>
    /// Handles the query by retrieving the product from the database and mapping it to a DTO.
    /// </summary>
    /// <param name="query">The query object containing the product ID.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="GetProductByIdResult"/> containing the mapped product.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Thrown if the product with the specified ID is not found.
    /// </exception>
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var productId = Guid.Parse(query.ProductId);
        
        // Get product by id using dbContext
        var product = await dbContext.Products
            .SingleDefaultOrThrowAsync(
                p => p.Id == productId,
                asNoTracking: true,
                cancellationToken: cancellationToken,
                keyValue: query.ProductId
            );

        // Map product entity to ProductDto using Mapster
        var productDto = product.Adapt<ProductDto>();

        return new GetProductByIdResult(productDto);
    }
}
