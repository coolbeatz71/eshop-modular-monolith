
using Mapster;
using Microsoft.EntityFrameworkCore;

using EShop.Catalog.DataSource;
using EShop.Catalog.Products.Dtos;
using Eshop.Shared.CQRS;

namespace EShop.Catalog.Products.UseCases.GetProducts;

public abstract record GetProductsQuery(): IQuery<GetProductsResponse>;

public record GetProductsResponse(IEnumerable<ProductDto> Products);

public class GetProductsHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsQuery, GetProductsResponse>
{
    public async Task<GetProductsResponse> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        // get products using dbContext
        var products = await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
        
        // map product entity to ProductDto using Mapster
        var productsDtos = products.Adapt<List<ProductDto>>(); 
        
        // return response
        return new GetProductsResponse(productsDtos);
    }
}