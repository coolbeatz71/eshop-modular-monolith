using EShop.Shared.Metadata;

namespace EShop.Basket.Domain.Basket.UseCases.AddItemToBasket;

/// <summary>
/// Contains metadata information for the add item to basket route,
/// such as name, summary, and detailed description.
/// </summary>
public static class RouteMetaField
{
    public static readonly RouteMetadata AddItem = new(
        name: "AddItemToBasket",
        summary: "Add an item to the authenticated user's shopping basket.",
        description: """
             Accepts a single shopping cart item and adds it to the existing basket 
             associated with the authenticated user. The product's latest price and name 
             are retrieved from the Catalog module before insertion.

             On success, returns a 201 Created response along with the basket's identifier.

             If the request is invalid, contains an unsupported product, or fails validation,
             a 400 Bad Request response is returned.
         """
    );
}