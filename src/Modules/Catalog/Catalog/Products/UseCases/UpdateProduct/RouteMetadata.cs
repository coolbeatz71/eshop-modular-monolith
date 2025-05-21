using EShop.Shared.Metadata;

namespace EShop.Catalog.Products.UseCases.UpdateProduct;

public static class ProductRoutes
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