using EShop.Shared.Metadata;

namespace EShop.Catalog.Domain.Products.UseCases.UpdateProduct;

/// <summary>
/// Contains metadata information for the product update route,
/// such as name, summary, and detailed description.
/// </summary>
public static class RouteMetaField
{
    public static readonly RouteMetadata Update = new(
        name: "UpdateProduct",
        summary: "Updates an existing product in the catalog.",
        description: """
             Accepts a product ID and updated details. If the product exists, its data is updated.

             On success, returns a 200 OK response with the updated product information.

             If the product does not exist or validation fails, a suitable error response is returned.
         """
    );
}