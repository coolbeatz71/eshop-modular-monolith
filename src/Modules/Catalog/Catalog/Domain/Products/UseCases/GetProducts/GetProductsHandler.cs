using EShop.Catalog.DataSource;
using EShop.Catalog.Domain.Products.Dtos;
using EShop.Shared.CQRS;
using EShop.Shared.Pagination;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace EShop.Catalog.Domain.Products.UseCases.GetProducts;

/// <summary>
/// Query to retrieve a list of all products in the catalog.
/// </summary>
/// <param name="PaginatedRequest">Pagination parameters used to control page size and index.</param>
/// <remarks>
/// Implements the CQRS query contract to encapsulate read logic for fetching all products,
/// including support for pagination.
/// </remarks>
/// <example>
/// <code>
/// var query = new GetProductsQuery(new PaginatedRequest(page: 1, pageSize: 10));
/// var result = await mediator.Send(query);
/// </code>
/// </example>
public record GetProductsQuery(PaginatedRequest PaginatedRequest) : IQuery<GetProductsResult>;

/// <summary>
/// The response containing a collection of all products.
/// </summary>
/// <param name="Products">A paginated result containing product DTOs.
/// Includes metadata like total item count and current page index.
/// </param>
/// <example>
/// <code>
/// var response = new GetProductsResult(paginatedProducts);
/// </code>
/// </example>
public record GetProductsResult(PaginatedResult<ProductDto> Products);

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
        var pageIndex = query.PaginatedRequest.PageIndex;
        var pageSize = query.PaginatedRequest.PageSize;
        var count = await dbContext.Products.LongCountAsync(cancellationToken);
        
        // Get paginated products using dbContext
        var products = await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        // Map product entity to ProductDto using Mapster
        var productsDtoList = products.Adapt<List<ProductDto>>();
        var paginatedResults = new PaginatedResult<ProductDto>(
            pageIndex, 
            pageSize, 
            count, 
            productsDtoList
        );
        
        // Return response
        return new GetProductsResult(paginatedResults);
    }
}