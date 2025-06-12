using Carter;
using EShop.Catalog.Domain.Products.Dtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EShop.Catalog.Domain.Products.UseCases.GetProductById;

/// <summary>
/// Represents the response containing the product data retrieved by ID.
/// </summary>
/// <param name="Product">The product data transfer object.</param>
public record GetProductByIdResponse(ProductDto Product);

/// <summary>
/// Defines the endpoint module for retrieving a product by its unique identifier.
/// Maps an HTTP GET route to handle requests for fetching product details.
/// </summary>
public class GetProductByIdEndpoint : ICarterModule
{
    /// <summary>
    /// Adds the product retrieval endpoint to the application's route builder.
    /// Handles GET requests to "/products/{id}", sends a query via MediatR,
    /// and returns the product data if found.
    /// </summary>
    /// <param name="app">The route builder used to map the endpoint.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (string id, ISender sender) =>
            {
                var query = new GetProductByIdQuery(ProductId: id);

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