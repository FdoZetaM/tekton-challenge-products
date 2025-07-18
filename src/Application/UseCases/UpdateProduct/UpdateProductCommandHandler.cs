namespace TektonChallengeProducts.Application.UseCases.UpdateProduct;

using FluentValidation;
using MediatR;
using Application.Resources;
using Domain.Abstractions.Persistence;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Guid>
{
    private readonly IProductRepository productRepository;
    private readonly IValidator<UpdateProductCommand> validator;

    public UpdateProductCommandHandler(IProductRepository productRepository, IValidator<UpdateProductCommand> validator)
    {
        this.productRepository = productRepository;
        this.validator = validator;
    }

    public async Task<Guid> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException($"{ValidationMessagesResources.ValidationErrors}: {errors}");
        }

        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken)
                   ?? throw new KeyNotFoundException(ValidationMessagesResources.ProductNotFound);

        product.Update(
            request.Name,
            request.Status,
            request.Stock,
            request.Description,
            request.Price
        );

        productRepository.Update(product);
        await productRepository.UnitOfWork.CommitAsync(cancellationToken);

        return product.Id;
    }
}
