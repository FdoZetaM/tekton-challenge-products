namespace TektonChallengeProducts.Application.UseCases.Validators;

using FluentValidation;
using Application.Resources;
using Application.UseCases.Common;

public class ProductCommandValidator : AbstractValidator<ProductCommandBase>
{
    public ProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationMessagesResources.NameMandatory)
            .MaximumLength(100).WithMessage(ValidationMessagesResources.NameMaxLength);

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ValidationMessagesResources.StockCantBeNegative);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(ValidationMessagesResources.DescriptionMandatory)
            .MaximumLength(500).WithMessage(ValidationMessagesResources.DescriptionMaxLength);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage(ValidationMessagesResources.PriceGreaterThanZero);
    }
}
