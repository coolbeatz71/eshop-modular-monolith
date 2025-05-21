using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using EShop.Catalog.Products.Dtos;

namespace EShop.Catalog.Products.UseCases.CreateProduct;

public abstract record CreateProductRequest(ProductDto Product);
public abstract record CreateProductResponse(Guid Id);

public class CreateProductEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateProductResponse>();

                return Results.Created($"/products/{response.Id}", response);
            })
                .WithName(ProductRoutes.Create.Name)
                .WithSummary(ProductRoutes.Create.Summary)
                .WithDescription(ProductRoutes.Create.Description)
                .Produces<CreateProductResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}