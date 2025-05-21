using Carter;
using MediatR;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

using EShop.Catalog.Products.Dtos;

namespace EShop.Catalog.Products.UseCases.GetProductsByCategory;

public abstract record GetProductsByCategoryResponse(IEnumerable<ProductDto> Products);

public class GetProductsByCategoryEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("products/category/{category}", async (string category, ISender sender) =>
            {
                var query = new GetProductsByCategoryQuery(category);
                
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