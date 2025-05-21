using EShop.Shared.Metadata;

namespace EShop.Catalog.Products.UseCases.CreateProduct;

public static class ProductRoutes
{
    public static readonly RouteMetadata Create = new(
        name: "CreateProduct",
        summary: "Register a new product in the catalog.",
        description: """
             Accepts product details as input and creates a new product entry in the system.

             On success, returns a 201 Created response along with the newly created product's data,
             including its unique identifier.

             If the request is invalid or fails validation, a 400 Bad Request response is returned.
        """
    );
}