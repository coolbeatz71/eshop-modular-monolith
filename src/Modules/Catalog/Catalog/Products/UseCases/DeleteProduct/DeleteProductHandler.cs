using EShop.Catalog.DataSource;
using Eshop.Shared.CQRS;

namespace EShop.Catalog.Products.UseCases.DeleteProduct;

public abstract record DeleteProductCommand(Guid ProductId): ICommand<DeleteProductResponse>;

public record DeleteProductResponse(bool IsSuccess);

public class DeleteProductHandler(CatalogDbContext dbContext) 
    : ICommandHandler<DeleteProductCommand, DeleteProductResponse>
{
    public async Task<DeleteProductResponse> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var  product = await dbContext.Products.FindAsync(
            [command.ProductId], 
            cancellationToken: cancellationToken
        );
        
        if (product is null)
        {
            throw new Exception($"Could not find product with id: {command.ProductId}");
        }
        
        // delete product entity only if the product already exist in the Database
        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        // return response
        return new DeleteProductResponse(true);
    }
}
