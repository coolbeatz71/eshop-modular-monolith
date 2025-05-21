using Mapster;
using Microsoft.EntityFrameworkCore;

using EShop.Catalog.DataSource;
using EShop.Catalog.Products.Dtos;
using Eshop.Shared.CQRS;

namespace EShop.Catalog.Products.UseCases.GetProducts;

/// <summary>
/// Query to retrieve a list of all products in the catalog.
/// </summary>
/// <remarks>
/// Implements the CQRS query contract to encapsulate read logic for fetching all products.
/// </remarks>
/// <example>
/// <code>
/// var query = new GetProductsQuery();
/// var result = await mediator.Send(query);
/// </code>
/// </example>
public abstract record GetProductsQuery() : IQuery<GetProductsResult>;

/// <summary>
/// The response containing a collection of all products.
/// </summary>
/// <param name="Products">A list of product DTOs returned from the query.</param>
/// <example>
/// <code>
/// var response = new GetProductsResult(productList);
/// </code>
/// </example>
public record GetProductsResult(IEnumerable<ProductDto> Products);

/// <summary>
/// Handles the <see cref="GetProductsQuery"/> by retrieving all products from the database and mapping them to DTOs.
/// </summary>
/// <param name="dbContext">The database context used to access product data.</param>
/// <remarks>
/// This handler queries the database in a read-only mode using <c>AsNoTracking()</c> for performance,
/// orders the products alphabetically by name, and uses Mapster for DTO projection.
/// </remarks>
/// <example>
/// <code>
/// var handler = new GetProductsHandler(dbContext);
/// var result = await handler.Handle(new GetProductsQuery(), CancellationToken.None);
/// </code>
/// </example>
public class GetProductsHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    /// <summary>
    /// Handles the query to retrieve all products from the database.
    /// </summary>
    /// <param name="query">The query request object (no parameters required).</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A <see cref="GetProductsResult"/> containing a list of product DTOs.
    /// </returns>
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        // Get products using dbContext
        var products = await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
        
        // Map product entity to ProductDto using Mapster
        var productsDtos = products.Adapt<List<ProductDto>>(); 
        
        // Return response
        return new GetProductsResult(productsDtos);
    }
}