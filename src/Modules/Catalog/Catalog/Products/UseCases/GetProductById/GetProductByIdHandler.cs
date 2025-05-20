using Mapster;

using EShop.Catalog.DataSource;
using EShop.Catalog.Products.Dtos;
using Eshop.Shared.CQRS;
using EShop.Shared.DataSource.Extensions;

namespace EShop.Catalog.Products.UseCases.GetProductById;

public abstract record GetProductByIdQuery(Guid ProductId): IQuery<GetProductByIdResponse>;

public record GetProductByIdResponse(ProductDto Product);

public class GetProductByIdHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResponse>
{
    public async Task<GetProductByIdResponse> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        // get product by id using dbContext
        var product = await dbContext.Products
            .SingleDefaultOrThrowAsync(
                p => p.Id == query.ProductId, 
                asNoTracking: true,
                cancellationToken: cancellationToken
            );
        
        // map product entity to ProductDto using Mapster
        var productDto = product.Adapt<ProductDto>();
        
        return new GetProductByIdResponse(productDto);
    }
}