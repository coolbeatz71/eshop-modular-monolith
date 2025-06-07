using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using EShop.Catalog.Products.Dtos;

namespace EShop.Catalog.Products.UseCases.CreateProduct;

/// <summary>
/// Represents the request to create a product, containing the product data.
/// </summary>
/// <param name="Product">The product data transfer object.</param>
public record CreateProductRequest(ProductDto Product);

/// <summary>
/// Represents the response returned after successfully creating a product.
/// </summary>
/// <param name="Id">The unique identifier of the created product.</param>
public record CreateProductResponse(Guid Id);

/// <summary>
/// Defines the endpoint module for creating a new product.
/// Maps an HTTP POST route to handle product creation requests.
/// </summary>
public class CreateProductEndpoint : ICarterModule
{
    /// <summary>
    /// Adds the product creation endpoint to the application's route builder.
    /// Handles POST requests to "/products", sends a command via MediatR,
    /// and returns a 201 Created response with the created product's ID.
    /// </summary>
    /// <param name="app">The route builder used to map the endpoint.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateProductResponse>();

                return Results.Created($"/products/{response.Id}", response);
            })
            .WithName(RouteMetaField.Create.Name)
            .WithSummary(RouteMetaField.Create.Summary)
            .WithDescription(RouteMetaField.Create.Description)
            .ProducesValidationProblem()
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
