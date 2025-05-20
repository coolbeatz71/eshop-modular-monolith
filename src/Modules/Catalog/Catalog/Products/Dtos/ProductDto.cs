namespace EShop.Catalog.Products.Dtos;

public abstract record ProductDto(
    Guid Id,
    string Name,
    string Description,
    string ImageFile,
    decimal Price,
    List<string> Category
);