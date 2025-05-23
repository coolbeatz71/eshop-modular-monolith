using EShop.Catalog.DataSource;
using EShop.Catalog.Products.Dtos;
using Eshop.Shared.CQRS;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace EShop.Catalog.Products.UseCases.GetProductsByCategory;

/// <summary>
/// Query to retrieve all products that belong to a specified category.
/// </summary>
/// <param name="Category">The category name to filter the products by.</param>
/// <remarks>
/// This query enables category-based filtering of catalog products.
/// </remarks>
/// <example>
/// <code>
/// var query = new GetProductsByCategoryQuery("Electronics");
/// var result = await mediator.Send(query);
/// </code>
/// </example>
public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;

/// <summary>
/// The response containing a list of products filtered by category.
/// </summary>
/// <param name="Products">A collection of product DTOs belonging to the given category.</param>
/// <example>
/// <code>
/// var response = new GetProductsByCategoryResult(filteredProducts);
/// </code>
/// </example>
public record GetProductsByCategoryResult(IEnumerable<ProductDto> Products);

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
        // Get products by category using dbContext and fuzzy searching
        var products = await dbContext.Products
            .AsNoTracking()
            .Where(p =>
                    p.Category.Any(c => EF.Functions.ILike(c, $"%{query.Category}%"))
                )
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
        
        // Map product entity to ProductDto using Mapster
        var productsDtos = products.Adapt<List<ProductDto>>();
        
        // Return response
        return new GetProductsByCategoryResult(productsDtos);
    }
}
