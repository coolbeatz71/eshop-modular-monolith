using System.Security.Claims;
using Carter;
using EShop.Basket.Domain.Basket.Dtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EShop.Basket.Domain.Basket.UseCases.AddItemToBasket;

/// <summary>
/// Represents the request body for adding an item to a user's shopping basket.
/// </summary>
/// <param name="ShoppingCartItem">
/// The shopping cart item to add to the basket, including product ID, quantity, and price details.
/// </param>
public record AddItemToBasketRequest(ShoppingCartItemDto ShoppingCartItem);

/// <summary>
/// Represents the response returned after successfully adding an item to the basket.
/// </summary>
/// <param name="Id">The identifier of the updated or newly created basket.</param>
public record AddItemToBasketResponse(Guid Id);

/// <summary>
/// Carter endpoint module responsible for handling POST requests to add an item
/// to the authenticated user's shopping basket.
/// </summary>
/// <remarks>
/// This endpoint requires the user to be authenticated. It binds the item to the user based on their identity,
/// constructs a command using the item's details, and returns a 201 Created response with the basket ID.
/// </remarks>
public class AddItemToBasketEndpoint : ICarterModule
{
    /// <summary>
    /// Configures the POST route for adding an item to the user's basket.
    /// </summary>
    /// <param name="app">The route builder used to register this Carter endpoint.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/items", async (
            AddItemToBasketRequest request,
            ISender sender,
            ClaimsPrincipal user
        ) =>
        {
            // Extract authenticated username
            var userName = user.Identity!.Name;

            // Create a command to add the item to the user's basket
            var command = new AddItemToBasketCommand(userName!, request.ShoppingCartItem);
            var result = await sender.Send(command);

            // Adapt the result to an HTTP response DTO
            var response = result.Adapt<AddItemToBasketResponse>();

            return Results.Created($"/basket/{response.Id}", response);
        })
        .WithName(RouteMetaField.AddItem.Name)
        .WithSummary(RouteMetaField.AddItem.Summary)
        .WithDescription(RouteMetaField.AddItem.Description)
        .ProducesValidationProblem()
        .Produces<AddItemToBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization();
    }
}
