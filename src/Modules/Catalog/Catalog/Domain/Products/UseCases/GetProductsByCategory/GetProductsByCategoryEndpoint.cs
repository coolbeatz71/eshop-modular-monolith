using Carter;
using EShop.Catalog.Domain.Products.Dtos;
using EShop.Shared.Pagination;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EShop.Catalog.Domain.Products.UseCases.GetProductsByCategory;

/// <summary>
/// Represents the response containing a paginated list of products
/// filtered by a specified category.
/// </summary>
/// <param name="Products">A paginated result containing product DTOs that belong to the specified category.</param>
public record GetProductsByCategoryResponse(PaginatedResult<ProductDto> Products);

/// <summary>
/// Defines the endpoint module for retrieving products by category.
/// Maps an HTTP GET route that supports pagination and category filtering.
/// </summary>
public class GetProductsByCategoryEndpoint : ICarterModule
{
    /// <summary>
    /// Adds the product-by-category endpoint to the application's route builder.
    /// Handles GET requests to "/products/category/{category}", constructs a query with pagination and category filter,
    /// sends it via MediatR, and returns the paginated result.
    /// </summary>
    /// <param name="app">The route builder used to map the endpoint.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async (
                string category,
                [AsParameters] PaginatedRequest request,
                ISender sender
            ) =>
            {
                var query = new GetProductsByCategoryQuery(category, request);
                
                var result = await sender.Send(query);

                var response = result.Adapt<GetProductsByCategoryResponse>();
                
                return Results.Ok(response);
            })
            .WithName(RouteMetaField.GetByCategory.Name)
            .WithSummary(RouteMetaField.GetByCategory.Summary)
            .WithDescription(RouteMetaField.GetByCategory.Description)
            .Produces<GetProductsByCategoryResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
