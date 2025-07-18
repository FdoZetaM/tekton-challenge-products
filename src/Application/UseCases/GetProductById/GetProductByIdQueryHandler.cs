namespace TektonChallengeProducts.Application.UseCases.GetProductById;

using MediatR;
using Application.Resources;
using Domain.Abstractions.Persistence;
using Domain.Abstractions.Services;
using Domain.Enums;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse?>
{
    private readonly IProductRepository productRepository;
    private readonly ICacheService cacheService;

    public GetProductByIdQueryHandler(IProductRepository productRepository, ICacheService cacheService)
    {
        this.productRepository = productRepository;
        this.cacheService = cacheService;
    }

    public async Task<ProductResponse?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null) return null;

        var statusDict = await cacheService.GetAsync<Dictionary<Status, string>>(CacheKeys.ProductStatus)
                      ?? throw new InvalidOperationException(ValidationMessagesResources.StatusDictionaryNotCached);

        string status = statusDict.GetValueOrDefault(product.Status)
                     ?? throw new InvalidOperationException(string.Format(ValidationMessagesResources.StatusNotFoundInCache, product.Status));

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Status = status,
            Stock = product.Stock,
            Description = product.Description,
            Price = product.Price,
            DiscountPercentage = product.DiscountPercentage,
        };
    }
}
