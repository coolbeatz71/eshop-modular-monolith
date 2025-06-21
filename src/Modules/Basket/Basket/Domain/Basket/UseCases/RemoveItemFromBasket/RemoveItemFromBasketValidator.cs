using EShop.Basket.Domain.Basket.Validators;
using FluentValidation;

namespace EShop.Basket.Domain.Basket.UseCases.RemoveItemFromBasket;

public class RemoveItemFromBasketValidator: AbstractValidator<RemoveItemFromBasketCommand>
{
    /// <summary>
    /// Configure validation rules for removing an item from the basket.
    /// </summary>
    /// <remarks>
    /// Ensures that the shopping cart item being removed is valid
    /// by applying the rules defined in <see cref="ShoppingCartItemDtoValidator"/>.
    /// </remarks>
    public RemoveItemFromBasketValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required.");
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required.");
    }
}