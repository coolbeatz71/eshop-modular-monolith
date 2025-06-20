using EShop.Shared.Metadata;

namespace EShop.Basket.Domain.Basket.UseCases.CreateBasket;

/// <summary>
/// Contains metadata information for the basket creation route,
/// such as name, summary, and detailed description.
/// </summary>
public static class RouteMetaField
{
    public static readonly RouteMetadata Create = new(
        name: "CreateBasket",
        summary: "Create a new shopping basket for the authenticated user.",
        description: """
             Accepts a shopping cart payload containing at least one item and creates a new basket 
             entry associated with the logged-in user.
         
             On success, returns a 201 Created response along with the newly created basket's identifier.
         
             If the request is invalid, fails validation, or the basket is empty, a 400 Bad Request 
             response is returned.
         """
    );
}