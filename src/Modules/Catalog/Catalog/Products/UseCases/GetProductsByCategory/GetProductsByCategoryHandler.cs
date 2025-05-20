using EShop.Catalog.DataSource;
using EShop.Catalog.Products.Dtos;
using EShop.Catalog.Products.UseCases.GetProducts;
using Eshop.Shared.CQRS;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace EShop.Catalog.Products.UseCases.GetProductByCategory;

public abstract record GetProductsByCategoryQuery(string Category): IQuery<GetProductsByCategoryResponse>;

public record GetProductsByCategoryResponse(IEnumerable<ProductDto> Products);

public class GetProductsByCategoryHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResponse>
{
    public async Task<GetProductsByCategoryResponse> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        // get products by category using dbContext
        var products = await dbContext.Products
            .AsNoTracking()
            .Where(p => p.Category.Contains(query.Category))
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
        
        // map product entity to ProductDto using Mapster
        var productsDtos = products.Adapt<List<ProductDto>>();
        
        // return response
        return new GetProductsByCategoryResponse(productsDtos);
    }
}