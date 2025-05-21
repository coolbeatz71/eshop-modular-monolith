using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using EShop.Catalog.Products.Dtos;

namespace EShop.Catalog.Products.UseCases.GetProductById;

public abstract record GetProductByIdResponse(ProductDto Product);

public class GetProductByIdEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", async (Guid id, ISender sender) =>
            {
                var query = new GetProductByIdQuery(id);

                var result = await sender.Send(query);

                var response = result.Adapt<GetProductByIdResponse>();
                
                return Results.Ok(response);
            })
            .WithName(RouteMetaField.GetById.Name)
            .WithSummary(RouteMetaField.GetById.Summary)
            .WithDescription(RouteMetaField.GetById.Description)
            .Produces<GetProductByIdResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}