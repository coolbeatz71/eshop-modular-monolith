using EShop.Catalog.DataSource;
using EShop.Catalog.Products.Dtos;
using EShop.Catalog.Products.Entities;
using Eshop.Shared.CQRS;
using EShop.Shared.DataSource.Extensions;

namespace EShop.Catalog.Products.UseCases.UpdateProduct;

public abstract record UpdateProductCommand(ProductDto Product): ICommand<UpdateProductResponse>;

public record UpdateProductResponse(bool IsSuccess);

public class UpdateProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<UpdateProductCommand, UpdateProductResponse>
{
    public async Task<UpdateProductResponse> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        // check for entity in the Database
        var product = await dbContext.FindOrThrowAsync<ProductEntity>([command.Product.Id], cancellationToken);
        
        // create Product entity from command object
        // only if the product exist in the Database
        UpdateProductWithNewValues(product,  command.Product);
        
        // save to Database
        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync(cancellationToken);
    
        // return response
        return new UpdateProductResponse(true);
    }

    private static void UpdateProductWithNewValues(ProductEntity actualProduct, ProductDto newProductDto)
    {
        actualProduct.Update(
            name: newProductDto.Name,
            description: newProductDto.Description,
            imageFile: newProductDto.ImageFile,
            price: newProductDto.Price,
            category: newProductDto.Category
        );
    }
}