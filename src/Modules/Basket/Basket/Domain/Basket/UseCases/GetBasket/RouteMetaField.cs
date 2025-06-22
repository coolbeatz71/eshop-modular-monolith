using EShop.Shared.Metadata;

namespace EShop.Basket.Domain.Basket.UseCases.GetBasket;

/// <summary>
/// Contains metadata information for the get basket route,
/// such as name, summary, and detailed description.
/// </summary>
public static class RouteMetaField
{
    public static readonly RouteMetadata Get = new(
        name: "GetBasket",
        summary: "Retrieve the shopping basket for the authenticated user by username.",
        description: """
             Retrieves the shopping basket associated with the provided username.
             
             This endpoint requires the user to be authenticated. The username must correspond
             to the currently logged-in user.
             
             On success, returns a 200 OK response with the basket details.
             
             If the basket is not found, returns a 404 Not Found response.
             
             If the request is unauthorized, returns a 401 Unauthorized response.
         """
    );
}