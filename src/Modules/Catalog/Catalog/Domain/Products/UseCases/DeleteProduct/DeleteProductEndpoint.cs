using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EShop.Catalog.Domain.Products.UseCases.DeleteProduct;

/// <summary>
/// Represents the response returned after attempting to delete a product.
/// </summary>
/// <param name="IsSuccess">Indicates whether the deletion was successful.</param>
public record DeleteProductResponse(bool IsSuccess);

/// <summary>
/// Defines the endpoint module for deleting a product.
/// Maps an HTTP DELETE route to handle product deletion requests by product ID.
/// </summary>
public class DeleteProductEndpoint: ICarterModule
{
    /// <summary>
    /// Adds the product deletion endpoint to the application's route builder.
    /// Handles DELETE requests to "/products/{id}", sends a command via MediatR,
    /// and returns a response indicating success or failure.
    /// </summary>
    /// <param name="app">The route builder used to map the endpoint.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}", async (string id, ISender sender) =>
            {
                var command = new DeleteProductCommand(ProductId: id);

                var result = await sender.Send(command);

                var response = result.Adapt<DeleteProductResponse>();

                return Results.Ok(response);
            })
            .WithName(RouteMetaField.Delete.Name)
            .WithSummary(RouteMetaField.Delete.Summary)
            .WithDescription(RouteMetaField.Delete.Description)
            .Produces<DeleteProductResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}