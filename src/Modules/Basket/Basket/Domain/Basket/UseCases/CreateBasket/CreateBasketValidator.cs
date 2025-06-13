using EShop.Basket.Domain.Basket.Validators;
using FluentValidation;

namespace EShop.Basket.Domain.Basket.UseCases.CreateBasket;

public class CreateBasketValidator: AbstractValidator<CreateBasketCommand>
{
    /// <summary>
    /// Configure validation rules for the basket/shopping cart properties.
    /// </summary>
    /// <remarks>
    /// Shopping cart must contain at least one item,
    /// We do not allow creation of empty basket
    /// </remarks>
    public CreateBasketValidator()
    {
        RuleFor(x => x.ShoppingCart.UserName).NotEmpty().WithMessage("Username is required.");
        RuleFor(x => x.ShoppingCart.Items)
            .NotEmpty()
            .WithMessage("Shopping cart must contain at least one item.");
        RuleForEach(x => x.ShoppingCart.Items).SetValidator(new ShoppingCartItemDtoValidator());
    }
}