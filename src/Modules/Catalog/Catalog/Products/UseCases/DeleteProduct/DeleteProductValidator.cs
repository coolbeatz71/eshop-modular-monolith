using FluentValidation;

namespace EShop.Catalog.Products.UseCases.DeleteProduct;

public class DeleteProductValidator: AbstractValidator<DeleteProductCommand>
{
    public DeleteProductValidator()
    {
        RuleFor(x => x.ProductId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Product Id is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Product Id is invalid.");
    }
}