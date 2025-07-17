namespace TektonChallengeProducts.Application.UseCases.CreateProduct;

using MediatR;
using Domain.Abstractions.Persistence;
using Domain.Entities;

public class CreateProductCommandHandler(IProductRepository productRepository) : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository productRepository = productRepository;

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
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
