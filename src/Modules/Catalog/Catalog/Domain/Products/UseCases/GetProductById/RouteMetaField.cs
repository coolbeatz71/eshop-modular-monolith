using EShop.Shared.Metadata;

namespace EShop.Catalog.Domain.Products.UseCases.GetProductById;

/// <summary>
/// Contains metadata information for the product retrieval by id route,
/// such as name, summary, and detailed description.
/// </summary>
public static class RouteMetaField
{
    public static readonly RouteMetadata GetById = new(
        name: "GetProductById",
        summary: "Retrieve a product by its unique identifier.",
        description: """
             Accepts a product ID and returns the product details if found.
         
             On success, returns a 200 OK response with the product data.
         
             If the product does not exist or validation fails, a suitable error response is returned.
         """
    );
}