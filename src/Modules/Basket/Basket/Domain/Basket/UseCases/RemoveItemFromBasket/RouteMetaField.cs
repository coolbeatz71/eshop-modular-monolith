using EShop.Shared.Metadata;

namespace EShop.Basket.Domain.Basket.UseCases.RemoveItemFromBasket;

/// <summary>
/// Contains metadata information for the remove item from basket route,
/// such as name, summary, and detailed description.
/// </summary>
public static class RouteMetaField
{
    public static readonly RouteMetadata RemoveItem = new(
        name: "RemoveItemFromBasket",
        summary: "Remove an item from the authenticated user's shopping basket.",
        description: """
             Removes a specific product from the shopping basket associated with the 
             authenticated user, identified by its unique product ID.
         
             On success, returns a 200 OK response along with the updated basket's identifier.
         
             If the product does not exist in the basket or the request is invalid, 
             a 400 Bad Request response is returned.
         """
    );
}