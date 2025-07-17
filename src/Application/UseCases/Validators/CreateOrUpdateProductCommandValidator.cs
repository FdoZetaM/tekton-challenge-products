namespace TektonChallengeProducts.Application.UseCases.Validators;

using FluentValidation;
using Application.UseCases.CreateProduct;

public class CreateOrUpdateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateOrUpdateProductCommandValidator()
    {
        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El stock no puede ser negativo.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción es obligatoria.")
            .MaximumLength(500).WithMessage("La descripción no puede exceder los {MaxLength} caracteres.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a cero.");
    }
}
