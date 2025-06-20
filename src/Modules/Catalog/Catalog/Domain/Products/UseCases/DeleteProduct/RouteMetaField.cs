using EShop.Shared.Metadata;

namespace EShop.Catalog.Domain.Products.UseCases.DeleteProduct;

/// <summary>
/// Contains metadata information for the product deletion route,
/// such as name, summary, and detailed description.
/// </summary>
public static class RouteMetaField
{
    public static readonly RouteMetadata Delete = new(
        name: "DeleteProduct",
        summary: "Delete an existing product in the catalog.",
        description: """
              Accepts a product ID. If the product exists, it will be deleted from the catalog.
         
              On success, returns 200 OK response.
         
              If the product does not exist or validation fails, a suitable error response is returned.
         """
    );
}