namespace TektonChallengeProducts.Application.UseCases.GetProductById;

using MediatR;
using Domain.Abstractions.Persistence;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse?>
{
    private readonly IProductRepository productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<ProductResponse?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null) return null;

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Status = product.Status.ToString(),
            Stock = product.Stock,
            Description = product.Description,
            Price = product.Price,
            DiscountPercentage = product.DiscountPercentage,
        };
    }
}
