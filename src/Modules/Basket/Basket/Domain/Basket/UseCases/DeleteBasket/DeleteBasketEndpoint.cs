using System.Security.Claims;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EShop.Basket.Domain.Basket.UseCases.DeleteBasket;

/// <summary>
/// Represents the response returned after successfully deleting a basket.
/// </summary>
/// <param name="IsSuccess">Indicates whether the basket deletion was successful.</param>
public record DeleteBasketResponse(bool IsSuccess);

/// <summary>
/// Defines the endpoint module for deleting an existing basket.
/// Maps an HTTP DELETE route to handle basket deletion requests.
/// </summary>
public class DeleteBasketEndpoint : ICarterModule
{
    /// <summary>
    /// Adds the basket deletion endpoint to the application's route builder.
    /// Handles DELETE requests to "/basket", sends a command via MediatR,
    /// and returns a 200 OK response with the result of the deletion operation.
    /// </summary>
    /// <param name="app">The route builder used to map the endpoint.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket", async (ISender sender, ClaimsPrincipal user) =>
            {
                var userName = user.Identity!.Name;

                var result = await sender.Send(new DeleteBasketCommand(userName!));

                var response = result.Adapt<DeleteBasketResponse>();

                return Results.Ok(response);
            })
            .WithName(RouteMetaField.Delete.Name)
            .WithSummary(RouteMetaField.Delete.Summary)
            .WithDescription(RouteMetaField.Delete.Description)
            .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();
    }
}