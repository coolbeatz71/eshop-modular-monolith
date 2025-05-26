using FluentValidation;

namespace EShop.Catalog.Products.UseCases.CreateProduct;

public class CreateProductValidator: AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
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
            .Must(c => c.Count != 0).WithMessage("The category list cannot be empty");
    }
}