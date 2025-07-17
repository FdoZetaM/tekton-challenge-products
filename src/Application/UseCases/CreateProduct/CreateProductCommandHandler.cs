namespace TektonChallengeProducts.Application.UseCases.CreateProduct;

using FluentValidation;
using MediatR;
using Domain.Abstractions.Persistence;
using Domain.Entities;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository productRepository;
    private readonly IValidator<CreateProductCommand> validator;

    public CreateProductCommandHandler(IProductRepository productRepository,
                                       IValidator<CreateProductCommand> validator)
    {
        this.productRepository = productRepository;
        this.validator = validator;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException($"Errores de validación: {errors}");
        }

        decimal discountPercentage = 10;

        var product = new Product(
            request.Status,
            request.Stock,
            request.Description,
            request.Price,
            discountPercentage
        );

        await productRepository.CreateAsync(product, cancellationToken);
        await productRepository.UnitOfWork.CommitAsync(cancellationToken);

        return product.Id;
    }
}
