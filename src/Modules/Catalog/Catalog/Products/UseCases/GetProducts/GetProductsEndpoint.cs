using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using EShop.Catalog.Products.Dtos;
using EShop.Shared.Pagination;

namespace EShop.Catalog.Products.UseCases.GetProducts;

public record GetProductsResponse(PaginatedResult<ProductDto> Products);

public class GetProductsEndpoint: ICarterModule
{
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