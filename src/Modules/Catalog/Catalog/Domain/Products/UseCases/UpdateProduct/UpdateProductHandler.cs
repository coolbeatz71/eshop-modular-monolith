using EShop.Catalog.DataSource;
using EShop.Catalog.Products.Dtos;
using EShop.Catalog.Products.Entities;
using EShop.Shared.CQRS;
using EShop.Shared.DataSource.Extensions;

namespace EShop.Catalog.Products.UseCases.UpdateProduct;

/// <summary>
/// Command to update an existing product in the catalog.
/// </summary>
/// <param name="ProductId">The ID of the product to be updated.</param>
/// <param name="Product">The product DTO containing updated product data.</param>
/// <example>
/// <code>
/// var updateCommand = new UpdateProductCommand(id, productDto);
/// var response = await mediator.Send(updateCommand);
/// </code>
/// </example>
public record UpdateProductCommand(string ProductId, ProductDto Product) : ICommand<UpdateProductResult>;

/// <summary>
/// Response indicating whether the update operation was successful.
/// </summary>
/// <param name="IsSuccess">True if the update was successful; otherwise, false.</param>
/// <example>
/// <code>
/// if (response.IsSuccess)
/// {
///     Console.WriteLine("Product updated successfully.");
/// }
/// </code>
/// </example>
public record UpdateProductResult(bool IsSuccess);

/// <summary>
/// Handles updating an existing product in the database.
/// </summary>
/// <param name="dbContext">The database context for accessing product entities.</param>
/// <remarks>
/// The handler fetches the existing product by ID, updates its properties using the provided product DTO,
/// and persists the changes to the database.
/// </remarks>
/// <example>
/// <code>
/// var handler = new UpdateProductHandler(dbContext);
/// var response = await handler.Handle(new UpdateProductCommand(productDto), CancellationToken.None);
/// </code>
/// </example>
public class UpdateProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    /// <summary>
    /// Handles the update command by applying new values to an existing product and saving changes.
    /// </summary>
    /// <param name="command">The update product command with the new product data.</param>
    /// <param name="cancellationToken">Token to cancel the operation if needed.</param>
    /// <returns>A response indicating success of the update operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the product with the specified ID is not found in the database.</exception>
    /// <exception cref="ArgumentException">Thrown when required properties in the product DTO are null or empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the product price is negative.</exception>
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var productId = Guid.Parse(command.ProductId);
        
        // check for entity in the Database
        var product = await dbContext.Products.FindOrThrowAsync([productId], cancellationToken);

        // update existing product entity with new values
        UpdateProductWithNewValues(product, command.Product);

        // save changes to the Database
        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        // return response indicating success
        return new UpdateProductResult(true);
    }

    /// <summary>
    /// Updates the properties of the existing product entity with values from the new product DTO.
    /// </summary>
    /// <param name="actualProduct">The existing product entity to update.</param>
    /// <param name="newProductDto">The product DTO containing updated values.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="newProductDto"/> contains null or empty <c>Name</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="newProductDto"/> contains a negative <c>Price</c>.</exception>
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
