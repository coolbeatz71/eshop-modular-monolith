using Carter;
using EShop.Catalog.Products.Dtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EShop.Catalog.Products.UseCases.UpdateProduct;

public abstract record UpdateProductRequest(ProductDto Product);
public abstract record UpdateProductResponse(bool IsSuccess);

public class UpdateProductEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateProductCommand>();
            
            var result = await sender.Send(command);
            
            var response = result.Adapt<UpdateProductResponse>();
            
            return Results.Ok(response);
        })
            .WithName(ProductRoutes.Update.Name)
            .WithSummary(ProductRoutes.Update.Summary)
            .WithDescription(ProductRoutes.Update.Description)
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}