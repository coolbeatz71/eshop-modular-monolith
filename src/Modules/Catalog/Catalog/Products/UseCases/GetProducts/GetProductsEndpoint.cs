using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using EShop.Catalog.Products.Dtos;

namespace EShop.Catalog.Products.UseCases.GetProducts;

public abstract record GetProductsResponse(IEnumerable<ProductDto> Products);

//TODO: should add pagination implementation
public class GetProductsEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender sender) =>
            {
                var query = new GetProductsQuery();
                
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