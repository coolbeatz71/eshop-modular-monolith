using EShop.Shared.Metadata;

namespace EShop.Basket.Domain.Basket.UseCases.DeleteBasket;

/// <summary>
/// Contains metadata information for the get basket route,
/// such as name, summary, and detailed description.
/// </summary>
public static class RouteMetaField
{
    public static readonly RouteMetadata Delete = new(
        name: "DeleteBasket",
        summary: "Delete the authenticated user's shopping basket.",
        description: """
             Deletes the shopping basket for the authenticated user by username.
         
             On success, returns a 200 OK response with a success status.
         
             If the basket cannot be deleted or the request is unauthorized, a 400 Bad Request or
             appropriate error response is returned.
         """
    );
}