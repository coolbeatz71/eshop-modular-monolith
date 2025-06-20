using EShop.Basket.Domain.Basket.Validators;
using FluentValidation;

namespace EShop.Basket.Domain.Basket.UseCases.AddItemToBasket;

public class AddItemToBasketValidator: AbstractValidator<AddItemToBasketCommand>
{
    /// <summary>
    /// Configure validation rules for adding an item to the basket.
    /// </summary>
    /// <remarks>
    /// Ensures that the shopping cart item being added is valid
    /// by applying the rules defined in <see cref="ShoppingCartItemDtoValidator"/>.
    /// </remarks>
    public AddItemToBasketValidator()
    {
        RuleFor(x => x.ShoppingCartItem)
            .SetValidator(new ShoppingCartItemDtoValidator());
    }
}