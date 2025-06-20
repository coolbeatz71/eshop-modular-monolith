using System.Security.Claims;
using Carter;
using EShop.Basket.Domain.Basket.Dtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EShop.Basket.Domain.Basket.UseCases.CreateBasket;

public record CreateBasketRequest(ShoppingCartDto ShoppingCart);
public record CreateBasketResponse(Guid Id);

public class CreateBasketEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (
                CreateBasketRequest request,
                ISender sender,
                ClaimsPrincipal user
            ) =>
            {
                var userName = user.Identity!.Name;
                var shoppingCart = request.ShoppingCart with { UserName = userName! };

                var command = new CreateBasketCommand(shoppingCart);
                var result = await sender.Send(command);
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