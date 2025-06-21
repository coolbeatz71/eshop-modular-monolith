using System.Security.Claims;
using Carter;
using EShop.Basket.Domain.Basket.Dtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EShop.Basket.Domain.Basket.UseCases.GetBasket;

/// <summary>
/// Represents the response returned when retrieving the user's basket.
/// </summary>
/// <param name="ShoppingCart">The shopping cart DTO associated with the user.</param>
public record GetBasketResponse(ShoppingCartDto ShoppingCart);

/// <summary>
/// Carter endpoint module for handling GET requests to retrieve the basket
/// of the currently authenticated user.
/// </summary>
/// <remarks>
/// This endpoint requires the user to be authenticated via ASP.NET Core Identity or JWT.
/// It retrieves the username from the current user's claims and sends a <see cref="GetBasketQuery"/>
/// through MediatR to obtain the corresponding basket.
/// </remarks>
public class GetBasketEndpoint : ICarterModule
{
    /// <summary>
    /// Configures the routes handled by this module.
    /// </summary>
    /// <param name="app">The endpoint route builder provided by ASP.NET Core.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket", async (ISender sender, ClaimsPrincipal user) =>
        {
            // Get username from the authenticated user
            var userName = user.Identity!.Name;

            // Send the query to retrieve the basket
            var result = await sender.Send(new GetBasketQuery(userName!));

            // Adapt the result to the HTTP response format
            var response = result.Adapt<GetBasketResponse>();

            return Results.Ok(response);
        })
        .WithName(RouteMetaField.Get.Name)
        .WithSummary(RouteMetaField.Get.Summary)
        .WithDescription(RouteMetaField.Get.Description)
        .ProducesValidationProblem()
        .Produces<GetBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireAuthorization();
    }
}
