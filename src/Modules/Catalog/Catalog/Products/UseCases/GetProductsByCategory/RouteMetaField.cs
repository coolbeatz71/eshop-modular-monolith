using EShop.Shared.Metadata;

namespace EShop.Catalog.Products.UseCases.GetProductsByCategory;

public abstract class RouteMetaField
{
    public static readonly RouteMetadata GetByCategory = new(
        name: "GetProductsByCategory",
        summary: "Retrieve products filtered by category.",
        description: """
             Accepts a category name or list of categories and returns all matching products.
         
             On success, returns a 200 OK response with the filtered list of products.
         
             If no products are found for the specified category, returns an empty collection.
         """
    );
}