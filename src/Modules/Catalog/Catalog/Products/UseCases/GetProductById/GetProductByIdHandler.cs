using Mapster;

using EShop.Catalog.DataSource;
using EShop.Catalog.Products.Dtos;
using Eshop.Shared.CQRS;
using EShop.Shared.DataSource.Extensions;
using Eshop.Shared.Exceptions;

namespace EShop.Catalog.Products.UseCases.GetProductById;

/// <summary>
/// Query to retrieve a product by its unique identifier.
/// </summary>
/// <param name="ProductId">The ID of the product to retrieve.</param>
/// <remarks>
/// Used in the CQRS pattern to encapsulate read logic for a single product.
/// </remarks>
/// <example>
/// <code>
/// var query = new GetProductByIdQuery(productId);
/// var result = await mediator.Send(query);
/// </code>
/// </example>
public abstract record GetProductByIdQuery(Guid ProductId) : IQuery<GetProductByIdResult>;

/// <summary>
/// The response containing the requested product information.
/// </summary>
/// <param name="Product">The product details mapped to <see cref="ProductDto"/>.</param>
/// <example>
/// <code>
/// var response = new GetProductByIdResult(productDto);
/// </code>
/// </example>
public record GetProductByIdResult(ProductDto Product);

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
        // Get product by id using dbContext
        var product = await dbContext.Products
            .SingleDefaultOrThrowAsync(
                p => p.Id == query.ProductId,
                asNoTracking: true,
                cancellationToken: cancellationToken,
                key: query.ProductId.ToString()
            );

        // Map product entity to ProductDto using Mapster
        var productDto = product.Adapt<ProductDto>();

        return new GetProductByIdResult(productDto);
    }
}
