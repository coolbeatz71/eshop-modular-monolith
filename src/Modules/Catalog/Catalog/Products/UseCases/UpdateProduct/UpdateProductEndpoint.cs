using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using EShop.Catalog.Products.Dtos;

namespace EShop.Catalog.Products.UseCases.UpdateProduct;

public record UpdateProductRequest(ProductDto Product);
public record UpdateProductResponse(bool IsSuccess);

public class UpdateProductEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products/{id:guid}", async (Guid id, UpdateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>() with { Id = id };
    
                var result = await sender.Send(command);

                var response = result.Adapt<UpdateProductResponse>();

                return Results.Ok(response);
            })
                .WithName(RouteMetaField.Update.Name)
                .WithSummary(RouteMetaField.Update.Summary)
                .WithDescription(RouteMetaField.Update.Description)
                .Produces<UpdateProductResponse>()
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);
    }
}