using System.Security.Claims;
using Carter;
using EShop.Basket.Domain.Basket.Dtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EShop.Basket.Domain.Basket.UseCases.CreateBasket;

/// <summary>
/// Represents the request body for creating a new basket.
/// </summary>
/// <param name="ShoppingCart">The shopping cart data to create, excluding the username which is taken from the authenticated user.</param>
public record CreateBasketRequest(ShoppingCartDto ShoppingCart);

/// <summary>
/// Represents the response returned after successfully creating a new basket.
/// </summary>
/// <param name="Id">The identifier of the newly created basket.</param>
public record CreateBasketResponse(Guid Id);

/// <summary>
/// Carter endpoint module responsible for handling basket creation.
/// </summary>
/// <remarks>
/// This endpoint requires an authenticated user. It uses the username from the 
/// user's identity claims and ensures the basket is associated with that user.
/// </remarks>
public class CreateBasketEndpoint : ICarterModule
{
    /// <summary>
    /// Configures the HTTP POST endpoint for basket creation.
    /// </summary>
    /// <param name="app">The endpoint route builder used to register the route.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (
                CreateBasketRequest request,
                ISender sender,
                ClaimsPrincipal user
            ) =>
            {
                // Extract authenticated username
                var userName = user.Identity!.Name;

                // Overwrite username in the incoming ShoppingCartDto to enforce ownership
                var shoppingCart = request.ShoppingCart with { UserName = userName! };

                // Send the command to create the basket
                var command = new CreateBasketCommand(shoppingCart);
                var result = await sender.Send(command);

                // Adapt result to response type
                var response = result.Adapt<CreateBasketResponse>();
                
                return Results.Created($"/basket/{response.Id}", response);
            })
            .WithName(RouteMetaField.Create.Name)
            .WithSummary(RouteMetaField.Create.Summary)
            .WithDescription(RouteMetaField.Create.Description)
            .ProducesValidationProblem()
            .Produces<CreateBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();
    }
}
