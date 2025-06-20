using Carter;
using EShop.Catalog.Domain.Products.Dtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EShop.Catalog.Domain.Products.UseCases.UpdateProduct;

/// <summary>
/// Represents the request to update a product with new data.
/// </summary>
/// <param name="Product">The updated product data transfer object.</param>
public record UpdateProductRequest(ProductDto Product);

/// <summary>
/// Represents the response indicating whether the product update was successful.
/// </summary>
/// <param name="IsSuccess">True if the update was successful; otherwise, false.</param>
public record UpdateProductResponse(bool IsSuccess);

/// <summary>
/// Defines the endpoint module responsible for updating an existing product.
/// Maps an HTTP PUT route to handle update operations using MediatR.
/// </summary>
public class UpdateProductEndpoint : ICarterModule
{
    /// <summary>
    /// Adds the product update endpoint to the application's route builder.
    /// Handles PUT requests to "/products/{id}", adapts the incoming request into an update command,
    /// and returns the result of the update operation.
    /// </summary>
    /// <param name="app">The route builder used to register the endpoint.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products/{id}", async (string id, UpdateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>() with { ProductId = id };
    
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
