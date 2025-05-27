using EShop.Shared.Extensions;
using FluentValidation;

namespace EShop.Catalog.Products.UseCases.UpdateProduct;

public class UpdateProductValidator: AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.ProductId)
            .Cascade(CascadeMode.Stop)
            .IsValidGuid("Product Id");
        RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Product.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(x => x.Product.ImageFile).NotEmpty().WithMessage("Image is required.");
        RuleFor(x => x.Product.Price)
            .Cascade(CascadeMode.Stop)
            .NotEqual(0).WithMessage("Price is required.")
            .GreaterThan(0).WithMessage("Price must be greater than zero.");
        RuleFor(x => x.Product.Category)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Category is required.")
            .Must(c => c.Count != 0).WithMessage("The category list cannot be empty.");
    }
}