using EShop.Shared.Extensions;
using FluentValidation;

namespace EShop.Catalog.Products.UseCases.GetProductById;

public class GetProductByIdValidator: AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdValidator()
    {
        RuleFor(x => x.ProductId)
            .Cascade(CascadeMode.Stop)
            .IsValidGuid("Product Id");
    }
}