using Mapster;

using EShop.Catalog.DataSource;
using EShop.Catalog.Products.Dtos;
using Eshop.Shared.CQRS;

namespace EShop.Catalog.Products.UseCases.GetProductById;

public abstract record GetProductByIdQuery(Guid ProductId): IQuery<GetProductByIdResponse>;

public record GetProductByIdResponse(ProductDto Product);

public class GetProductByIdHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResponse>
{
    public async Task<GetProductByIdResponse> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync(
            [query.ProductId],
            cancellationToken: cancellationToken
        );
        
        // map product entity to ProductDto using Mapster
        var productDto = product.Adapt<ProductDto>();
        
        return new GetProductByIdResponse(productDto);
    }
}