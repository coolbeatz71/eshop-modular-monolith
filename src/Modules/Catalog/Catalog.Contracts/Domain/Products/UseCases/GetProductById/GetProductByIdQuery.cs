using EShop.Catalog.Contracts.Domain.Products.Dtos;
using EShop.Shared.Contracts.CQRS;

namespace EShop.Catalog.Contracts.Domain.Products.UseCases.GetProductById;

/// <summary>
/// Query to retrieve a product by its unique identifier.
/// </summary>
/// <param name="ProductId">The ID of the product to retrieve.</param>
/// <remarks>
/// Used in the CQRS pattern to encapsulate read logic for a single product.
/// </remarks>
/// <example>
/// <code>
/// var query = new GetProductByIdQuery(productId);
/// var result = await mediator.Send(query);
/// </code>
/// </example>
public record GetProductByIdQuery(string ProductId) : IQuery<GetProductByIdResult>;

/// <summary>
/// The response containing the requested product information.
/// </summary>
/// <param name="Product">The product details mapped to <see cref="ProductDto"/>.</param>
/// <example>
/// <code>
/// var response = new GetProductByIdResult(productDto);
/// </code>
/// </example>
public record GetProductByIdResult(ProductDto Product);