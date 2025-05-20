using MediatR;

namespace EShop.Catalog.Products.UseCases.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    string ImageFile,
    decimal Price,
    List<string> Category
): IRequest<CreateProductResponse>;

public record CreateProductResponse(Guid Id);

public class CreateProductCommandHandler: IRequestHandler<CreateProductCommand, CreateProductResponse>
{
    public Task<CreateProductResponse> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        
    }
}