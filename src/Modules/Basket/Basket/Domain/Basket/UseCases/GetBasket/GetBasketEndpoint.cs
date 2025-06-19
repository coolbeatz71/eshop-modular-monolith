using System.Security.Claims;
using Carter;
using EShop.Basket.Domain.Basket.Dtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EShop.Basket.Domain.Basket.UseCases.GetBasket;

public record GetBasketResponse(ShoppingCartDto ShoppingCart);

public class GetBasketEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket", async (ISender sender, ClaimsPrincipal user) =>
        {
            var userName = user.Identity!.Name;
            var result = await sender.Send(new GetBasketQuery(userName!));
            
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