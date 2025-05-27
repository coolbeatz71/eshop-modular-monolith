using FluentValidation;

namespace EShop.Shared.Extensions;

public static class ValidatorExtension
{
    public static void IsValidGuid<T>(this IRuleBuilder<T, string> ruleBuilder,
        string fieldDisplayName = "Id")
    {
        ruleBuilder
            .NotEmpty().WithMessage($"{fieldDisplayName} is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage($"{fieldDisplayName} is invalid.");
    }
}