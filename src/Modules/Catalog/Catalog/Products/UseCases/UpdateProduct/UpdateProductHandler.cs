using EShop.Catalog.DataSource;
using EShop.Catalog.Products.Dtos;
using EShop.Catalog.Products.Models;
using Eshop.Shared.CQRS;

namespace EShop.Catalog.Products.UseCases.UpdateProduct;

public abstract record UpdateProductCommand(ProductDto Product): ICommand<UpdateProductResponse>;

public record UpdateProductResponse(bool IsSuccess);

public class UpdateProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<UpdateProductCommand, UpdateProductResponse>
{
    public async Task<UpdateProductResponse> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        
        var  product = await dbContext.Products.FindAsync(
            [command.Product.Id], 
            cancellationToken: cancellationToken
        );
        
        if (product is null)
        {
            throw new Exception($"Could not find product with id: {command.Product.Id}");
        }
        
        // create Product entity from command object
        // only if the product already exist in the Database
        UpdateProductWithNewValues(product,  command.Product);
        
        // save to Database
        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync(cancellationToken);
    
        // return response
        return new UpdateProductResponse(true);
    }

    private static void UpdateProductWithNewValues(Product actualProduct, ProductDto newProductDto)
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