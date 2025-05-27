using EShop.Catalog.DataSource;
using Eshop.Shared.CQRS;
using EShop.Shared.DataSource.Extensions;
using Eshop.Shared.Exceptions;

namespace EShop.Catalog.Products.UseCases.DeleteProduct;

/// <summary>
/// Command to delete a product by its unique identifier.
/// </summary>
/// <param name="ProductId">The ID of the product to be deleted.</param>
/// <remarks>
/// Used in a CQRS pattern to encapsulate delete logic.
/// </remarks>
/// <example>
/// <code>
/// var command = new DeleteProductCommand(productId);
/// var result = await mediator.Send(command);
/// </code>
/// </example>
public record DeleteProductCommand(string ProductId) : ICommand<DeleteProductResult>;

/// <summary>
/// Response indicating whether the product was successfully deleted.
/// </summary>
/// <param name="IsSuccess">True if the product was deleted; otherwise, false.</param>
/// <example>
/// <code>
/// var response = new DeleteProductResult(true);
/// </code>
/// </example>
public record DeleteProductResult(bool IsSuccess);

/// <summary>
/// Handles <see cref="DeleteProductCommand"/> by removing the product from the database.
/// </summary>
/// <param name="dbContext">The catalog database context.</param>
/// <remarks>
/// Uses <c>FindOrThrowAsync</c> to ensure the product exists before deletion.
/// </remarks>
/// <example>
/// <code>
/// var handler = new DeleteProductHandler(dbContext);
/// var result = await handler.Handle(command, CancellationToken.None);
/// </code>
/// </example>
public class DeleteProductHandler(CatalogDbContext dbContext) 
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    /// <summary>
    /// Handles the <see cref="DeleteProductCommand"/> by locating and removing the product from the database.
    /// </summary>
    /// <param name="command">The command containing the product ID.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>
    /// A <see cref="DeleteProductResult"/> indicating the result of the delete operation.
    /// </returns>
    /// <exception cref="NotFoundException">Thrown if the product is not found in the database.</exception>
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        // Check if entity exists in the database
        var product = await dbContext.Products.FindOrThrowAsync([command.ProductId], cancellationToken);

        // Delete the product entity
        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Return success response
        return new DeleteProductResult(true);
    }
}
