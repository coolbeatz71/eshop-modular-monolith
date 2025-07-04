using EShop.Catalog.DataSource;
using EShop.Catalog.Domain.Products.Dtos;
using EShop.Catalog.Domain.Products.Entities;
using EShop.Shared.CQRS;

namespace EShop.Catalog.Domain.Products.UseCases.CreateProduct;

/// <summary>
/// Command to create a new product using the provided product data.
/// </summary>
/// <param name="Product">The data required to create the product.</param>
/// <remarks>
/// Implements <see cref="ICommand{TResult}"/> to be used with MediatR for CQRS-based handling.
/// </remarks>
/// <example>
/// <code>
/// var command = new CreateProductCommand(productDto);
/// var result = await mediator.Send(command);
/// </code>
/// </example>
public record CreateProductCommand(ProductDto Product) : ICommand<CreateProductResult>;

/// <summary>
/// The response returned after successfully creating a product.
/// </summary>
/// <param name="Id">The unique identifier of the newly created product.</param>
/// <example>
/// <code>
/// var response = new CreateProductResult(productId);
/// </code>
/// </example>
public record CreateProductResult(Guid Id);

/// <summary>
/// Handles <see cref="CreateProductCommand"/> by persisting the product in the database and returning its ID.
/// </summary>
/// <param name="dbContext">The catalog database context for interacting with the data store.</param>
/// <remarks>
/// Responsible for mapping the DTO to the entity, saving the product, and returning the new ID.
/// </remarks>
/// <example>
/// <code>
/// var handler = new CreateProductHandler(dbContext);
/// var result = await handler.Handle(command, CancellationToken.None);
/// </code>
/// </example>
public class CreateProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    /// <summary>
    /// Handles the <see cref="CreateProductCommand"/> by creating and saving a new product.
    /// </summary>
    /// <param name="command">The command containing product details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="CreateProductResult"/> with the new product's ID.</returns>
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Create Product entity from command object
        var product = CreateNewProduct(command.Product);

        // Save to database
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Return response
        return new CreateProductResult(product.Id);
    }

    /// <summary>
    /// Converts the <see cref="ProductDto"/> to a <see cref="ProductEntity"/> using factory logic.
    /// </summary>
    /// <param name="productDto">The DTO containing product creation data.</param>
    /// <returns>A new <see cref="ProductEntity"/> instance.</returns>
    private static ProductEntity CreateNewProduct(ProductDto productDto)
    {
        var product = ProductEntity.Create(
            id: Guid.NewGuid(),
            name: productDto.Name,
            description: productDto.Description,
            imageFile: productDto.ImageFile,
            price: productDto.Price,
            category: productDto.Category
        );

        return product;
    }
}
