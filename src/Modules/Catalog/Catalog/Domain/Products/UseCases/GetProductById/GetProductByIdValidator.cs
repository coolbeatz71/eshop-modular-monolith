using EShop.Shared.Extensions;
using FluentValidation;

namespace EShop.Catalog.Domain.Products.UseCases.GetProductById;

/// <summary>
/// Validator for <see cref="GetProductByIdQuery"/> that ensures the product data is valid.
/// </summary>
public class GetProductByIdValidator: AbstractValidator<GetProductByIdQuery>
{
    /// <summary>
    /// Configure validation rules for the product properties.
    /// </summary>
    public GetProductByIdValidator()
    {
        RuleFor(x => x.ProductId)
            .Cascade(CascadeMode.Stop)
            .IsValidGuid("Product Id");
    }
}