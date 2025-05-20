using EShop.Catalog.DataSource;
using Eshop.Shared.CQRS;
using EShop.Shared.DataSource.Extensions;

namespace EShop.Catalog.Products.UseCases.DeleteProduct;

public abstract record DeleteProductCommand(Guid ProductId): ICommand<DeleteProductResponse>;

public record DeleteProductResponse(bool IsSuccess);

public class DeleteProductHandler(CatalogDbContext dbContext) 
    : ICommandHandler<DeleteProductCommand, DeleteProductResponse>
{
    public async Task<DeleteProductResponse> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        // check for entity in the Database
        var product = await dbContext.Products.FindOrThrowAsync([command.ProductId], cancellationToken);
        
        // delete product entity only if the product already exist in the Database
        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        // return response
        return new DeleteProductResponse(true);
    }
}
