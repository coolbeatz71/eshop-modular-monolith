using FluentValidation;

namespace EShop.Catalog.Products.UseCases.DeleteProduct;

public class DeleteProductValidator: AbstractValidator<DeleteProductCommand>
{
    public DeleteProductValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product Id is required.");
    }
}