using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using EShop.Catalog.Products.Dtos;
using EShop.Shared.Pagination;

namespace EShop.Catalog.Products.UseCases.GetProducts;

/// <summary>
/// Represents the response returned when retrieving a paginated list of products.
/// </summary>
/// <param name="Products">A paginated result containing a collection of product DTOs.</param>
public record GetProductsResponse(PaginatedResult<ProductDto> Products);

/// <summary>
/// Defines the endpoint module for retrieving a paginated list of products.
/// Maps an HTTP GET route that supports pagination via query parameters.
/// </summary>
public class GetProductsEndpoint : ICarterModule
{
    /// <summary>
    /// Adds the paginated product listing endpoint to the application's route builder.
    /// Handles GET requests to "/products", sends a query with pagination data via MediatR,
    /// and returns a paginated list of products.
    /// </summary>
    /// <param name="app">The route builder used to map the endpoint.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] PaginatedRequest request, ISender sender) =>
            {
                var query = new GetProductsQuery(request);
                
                var result = await sender.Send(query);
                
                var response = result.Adapt<GetProductsResponse>();
                
                return Results.Ok(response);
            })
            .WithName(RouteMetaField.GetAll.Name)
            .WithSummary(RouteMetaField.GetAll.Summary)
            .WithDescription(RouteMetaField.GetAll.Description)
            .Produces<GetProductsResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}