using EShop.Catalog.Products.Dtos;

namespace EShop.Catalog.Products.UseCases.CreateProduct;

public record CreateProductRequest(ProductDto Product);
public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint
{
    
}