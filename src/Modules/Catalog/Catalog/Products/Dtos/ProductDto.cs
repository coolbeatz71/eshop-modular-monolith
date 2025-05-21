namespace EShop.Catalog.Products.Dtos;

/// <summary>
/// Represents a data transfer object for product information.
/// </summary>
/// <param name="Id">The unique identifier of the product.</param>
/// <param name="Name">The name of the product.</param>
/// <param name="Description">The product's description.</param>
/// <param name="ImageFile">The URL or path to the product image.</param>
/// <param name="Price">The price of the product.</param>
/// <param name="Category">The list of categories associated with the product.</param>
public abstract record ProductDto(
    Guid Id,
    string Name,
    string Description,
    string ImageFile,
    decimal Price,
    List<string> Category
);