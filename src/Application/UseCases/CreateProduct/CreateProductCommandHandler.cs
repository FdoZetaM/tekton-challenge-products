namespace TektonChallengeProducts.Application.UseCases.CreateProduct;

using FluentValidation;
using MediatR;
using Application.Resources;
using Application.Services;
using Domain.Abstractions.Persistence;
using Domain.Entities;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository productRepository;
    private readonly IDiscountService discountService;
    private readonly IValidator<CreateProductCommand> validator;

    public CreateProductCommandHandler(IProductRepository productRepository,
                                       IDiscountService discountService,
                                       IValidator<CreateProductCommand> validator)
    {
        this.productRepository = productRepository;
        this.discountService = discountService;
        this.validator = validator;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException($"{ValidationMessagesResources.ValidationErrors}: {errors}");
        }

        byte discountPercentage = await discountService.GetDiscountToApplyAsync();

        var product = new Product(
            request.Name,
            request.Status,
            request.Stock,
            request.Description,
            request.Price
        );

        product.SetDiscountPercentage(discountPercentage);

        await productRepository.CreateAsync(product, cancellationToken);
        await productRepository.UnitOfWork.CommitAsync(cancellationToken);

        return product.Id;
    }
}
