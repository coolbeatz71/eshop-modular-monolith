using EShop.Catalog.DataSource;
using EShop.Catalog.Products.Dtos;
using EShop.Catalog.Products.Entities;
using Eshop.Shared.CQRS;

namespace EShop.Catalog.Products.UseCases.CreateProduct;

public abstract record CreateProductCommand(ProductDto Product): ICommand<CreateProductResponse>;

public record CreateProductResponse(Guid Id);

public class CreateProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<CreateProductCommand, CreateProductResponse>
{
    public async Task<CreateProductResponse> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // create Product entity from command object
        var product = CreateNewProduct(command.Product);
        
        // save to Database
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        // return response
        return new CreateProductResponse(product.Id);
    }

    private static ProductEntity CreateNewProduct(ProductDto productDto)
    {
        var product = ProductEntity.Create(
           id: Guid.NewGuid(),
           name: productDto.Name,
           description: productDto.Description,
           imageFile:  productDto.ImageFile,
           price: productDto.Price,
           category:  productDto.Category
        );

        return product;
    }
}