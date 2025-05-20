using EShop.Catalog.DataSource;
using EShop.Catalog.Products.Dtos;
using EShop.Catalog.Products.Entities;
using Eshop.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace EShop.Catalog.Products.UseCases.GetProducts;

public abstract record GetProductsQuery(): IQuery<GetProductsResponse>;

public record GetProductsResponse(IEnumerable<ProductDto> Products);

public class GetProductsHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsQuery, GetProductsResponse>
{
    public async Task<GetProductsResponse> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        // get products using dbContext
        // return results
        var products = await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
        
        var productsDtos = MapToProductDtos(products);
        
        return new GetProductsResponse(productsDtos);
    }

    private static IEnumerable<ProductDto> MapToProductDtos(List<ProductEntity> products)
    {
        foreach (var product in products)
        {
            
        }

        return [];
    }
}