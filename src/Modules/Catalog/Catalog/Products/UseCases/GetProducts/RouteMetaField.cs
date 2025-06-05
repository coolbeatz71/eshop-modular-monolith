using EShop.Shared.Metadata;

namespace EShop.Catalog.Products.UseCases.GetProducts;

/// <summary>
/// Contains metadata information for the products retrieval route,
/// such as name, summary, and detailed description.
/// </summary>
public static class RouteMetaField
{
    public static readonly RouteMetadata GetAll = new(
        name: "GetAllProducts",
        summary: "Retrieve all products from the catalog with pagination support.",
        description: """
             Returns a paginated list of all products available in the catalog.

             Accepts pagination parameters such as page number and page size.

             On success, returns a 200 OK response with the paginated list of products and pagination metadata.

             If no products are found, returns an empty list.
        """
    );
}