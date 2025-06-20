using EShop.Catalog.DataSource;
using EShop.Catalog.Domain.Products.Dtos;
using EShop.Shared.CQRS;
using EShop.Shared.Pagination;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace EShop.Catalog.Domain.Products.UseCases.GetProductsByCategory;

/// <summary>
/// Query to retrieve all products that belong to a specified category.
/// </summary>
/// <param name="Category">The category name to filter the products by.</param>
/// <param name="PaginatedRequest">Pagination parameters used to control page size and index.</param>
/// <remarks>
/// This query enables category-based filtering of catalog products with pagination support.
/// </remarks>
/// <example>
/// <code>
/// var query = new GetProductsByCategoryQuery("Electronics", new PaginatedRequest(page: 1, pageSize: 10));
/// var result = await mediator.Send(query);
/// </code>
/// </example>
public record GetProductsByCategoryQuery(
    string Category, 
    PaginatedRequest PaginatedRequest
) : IQuery<GetProductsByCategoryResult>;

/// <summary>
/// The response containing a paginated list of products filtered by category.
/// </summary>
/// <param name="Products">
/// A paginated result containing product DTOs that belong to the specified category.
/// Includes metadata like total item count and current page index.
/// </param>
/// <example>
/// <code>
/// var response = new GetProductsByCategoryResult(paginatedProducts);
/// </code>
/// </example>
public record GetProductsByCategoryResult(PaginatedResult<ProductDto> Products);

/// <summary>
/// Handles the <see cref="GetProductsByCategoryQuery"/> by retrieving products from the database that belong to a specified category.
/// </summary>
/// <param name="dbContext">The <see cref="CatalogDbContext"/> used to query the product data.</param>
/// <remarks>
/// The handler uses EF Core's <c>Where</c> clause to filter products by category,
/// performs the query in a no-tracking context for read-only performance,
/// orders results alphabetically by name, and maps entities to DTOs using Mapster.
/// </remarks>
/// <example>
/// <code>
/// var handler = new GetProductsByCategoryHandler(dbContext);
/// var result = await handler.Handle(new GetProductsByCategoryQuery("Books"), CancellationToken.None);
/// </code>
/// </example>
public class GetProductsByCategoryHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    /// <summary>
    /// Executes the query to retrieve products based on the provided category.
    /// </summary>
    /// <param name="query">The category filter query.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the query if needed.</param>
    /// <returns>
    /// A <see cref="GetProductsByCategoryResult"/> containing filtered products.
    /// </returns>
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginatedRequest.PageIndex;
        var pageSize = query.PaginatedRequest.PageSize;
        
        var baseQuery = dbContext.Products
            .AsNoTracking()
            .Where(p => p.Category.Any(c => EF.Functions.ILike(c, $"%{query.Category}%")));
        
        var count = await baseQuery.LongCountAsync(cancellationToken);
        
        // Get products by category using dbContext and fuzzy searching
        var products = await baseQuery
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
        return new GetProductsByCategoryResult(paginatedResults);
    }
}
