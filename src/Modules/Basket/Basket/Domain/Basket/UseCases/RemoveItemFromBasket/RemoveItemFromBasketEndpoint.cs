using System.Security.Claims;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EShop.Basket.Domain.Basket.UseCases.RemoveItemFromBasket;

/// <summary>
/// Represents the response returned after successfully removing an item from the basket.
/// </summary>
/// <param name="Id">The unique identifier of the updated shopping basket.</param>
public record RemoveItemFromBasketResponse(Guid Id);

/// <summary>
/// Defines the endpoint module for removing an item from the user's shopping basket.
/// Maps an HTTP DELETE route to handle item removal requests.
/// </summary>
public class RemoveItemFromBasketEndpoint : ICarterModule
{
    /// <summary>
    /// Adds the item removal endpoint to the application's route builder.
    /// Handles DELETE requests to "/basket/items/{productId}", sends a command via MediatR,
    /// and returns a 200 OK response with the updated basket's ID.
    /// </summary>
    /// <param name="app">The route builder used to map the endpoint.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/items/{productId}", async (
            Guid productId,
            ISender sender,
            ClaimsPrincipal user
        ) =>
        {
            var userName = user.Identity!.Name;

            // Create and send the remove item command
            var command = new RemoveItemFromBasketCommand(userName!, productId);
            var result = await sender.Send(command);

            var response = result.Adapt<RemoveItemFromBasketResponse>();

            return Results.Ok(response);
        })
        .WithName(RouteMetaField.RemoveItem.Name)
        .WithSummary(RouteMetaField.RemoveItem.Summary)
        .WithDescription(RouteMetaField.RemoveItem.Description)
        .Produces<RemoveItemFromBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization();
    }
}
