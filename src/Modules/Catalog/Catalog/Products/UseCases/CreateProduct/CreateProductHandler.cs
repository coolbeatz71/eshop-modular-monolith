using EShop.Catalog.DataSource;
using EShop.Catalog.Products.Dtos;
using EShop.Catalog.Products.Entities;
using Eshop.Shared.CQRS;

namespace EShop.Catalog.Products.UseCases.CreateProduct;

/// <summary>
/// Command to create a new product using the provided product data.
/// </summary>
/// <param name="Product">The data required to create the product.</param>
/// <remarks>
/// Implements <see cref="ICommand{TResponse}"/> to be used with MediatR for CQRS-based handling.
/// </remarks>
/// <example>
/// <code>
/// var command = new CreateProductCommand(productDto);
/// var result = await mediator.Send(command);
/// </code>
/// </example>
public abstract record CreateProductCommand(ProductDto Product) : ICommand<CreateProductResponse>;

/// <summary>
/// The response returned after successfully creating a product.
/// </summary>
/// <param name="Id">The unique identifier of the newly created product.</param>
/// <example>
/// <code>
/// var response = new CreateProductResponse(productId);
/// </code>
/// </example>
public record CreateProductResponse(Guid Id);

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
    : ICommandHandler<CreateProductCommand, CreateProductResponse>
{
    /// <summary>
    /// Handles the <see cref="CreateProductCommand"/> by creating and saving a new product.
    /// </summary>
    /// <param name="command">The command containing product details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="CreateProductResponse"/> with the new product's ID.</returns>
    public async Task<CreateProductResponse> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Create Product entity from command object
        var product = CreateNewProduct(command.Product);

        // Save to database
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Return response
        return new CreateProductResponse(product.Id);
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
