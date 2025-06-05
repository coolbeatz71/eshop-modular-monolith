using EShop.Shared.Extensions;
using FluentValidation;

namespace EShop.Catalog.Products.UseCases.DeleteProduct;

/// <summary>
/// Validator for <see cref="DeleteProductCommand"/> that ensures the product data is valid.
/// </summary>
public class DeleteProductValidator: AbstractValidator<DeleteProductCommand>
{
    /// <summary>
    /// Configure validation rules for the product properties.
    /// </summary>
    public DeleteProductValidator()
    {
        RuleFor(x => x.ProductId)
            .Cascade(CascadeMode.Stop)
            .IsValidGuid("Product Id");
    }
}