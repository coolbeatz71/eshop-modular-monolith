using EShop.Basket.Domain.Basket.Dtos;
using FluentValidation;

namespace EShop.Basket.Domain.Basket.Validators;

public class ShoppingCartItemDtoValidator: AbstractValidator<ShoppingCartItemDto>
{
    /// <summary>
    /// Configure validation rules for the basket/shopping cart item properties.
    /// </summary>
    public ShoppingCartItemDtoValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required.");
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.Color)
            .MaximumLength(30)
            .WithMessage("Color cannot exceed 30 characters.");
    }
}