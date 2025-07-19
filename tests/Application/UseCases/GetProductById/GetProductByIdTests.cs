namespace TektonChallengeProducts.Application.Tests.UseCases.GetProductById;

using Moq;
using NUnit.Framework;
using Application.Resources;
using Application.UseCases.GetProductById;
using Domain.Abstractions.Persistence;
using Domain.Abstractions.Services;
using Domain.Entities;
using Domain.Enums;

[TestFixture]
public class GetProductByIdTests
{
    private readonly Mock<IProductRepository> mockProductRepository;
    private readonly Mock<ICacheService> mockCacheService;

    public GetProductByIdTests()
    {
        mockProductRepository = new Mock<IProductRepository>();
        mockCacheService = new Mock<ICacheService>();
    }

    [Test]
    public async Task Handle_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();
        mockProductRepository.Setup(repo => repo.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Product?)null);

        var handler = new GetProductByIdQueryHandler(mockProductRepository.Object, mockCacheService.Object);
        var query = new GetProductByIdQuery(productId);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void Handle_ShouldThrowException_WhenDictionaryIsNotCached()
    {
        // Arrange
        string name = "Test Product";
        Status status = Status.Active;
        int stock = 5;
        string description = "Test Product";
        decimal price = 100m;
        byte discount = 10;

        var product = new Product(name, status, stock, description, price);
        product.SetDiscountPercentage(discount);

        mockProductRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(product);

        mockCacheService.Setup(cache => cache.GetAsync<Dictionary<Status, string>>(It.IsAny<string>()))
                        .ReturnsAsync((Dictionary<Status, string>?)null);

        var handler = new GetProductByIdQueryHandler(mockProductRepository.Object, mockCacheService.Object);
        var query = new GetProductByIdQuery(Guid.NewGuid());

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(query, default));
        Assert.That(ex!.Message, Is.EqualTo(ValidationMessagesResources.StatusDictionaryNotCached));
    }

    [Test]
    public void Handle_ShouldThrowException_WhenCacheThrows()
    {
        // Arrange
        string name = "Test Product";
        Status status = Status.Active;
        int stock = 5;
        string description = "Test Product";
        decimal price = 100m;
        byte discount = 10;

        var product = new Product(name, status, stock, description, price);
        product.SetDiscountPercentage(discount);

        mockProductRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(product);

        var statusDict = new Dictionary<Status, string>
        {
           { Status.Inactive, "Inactive" }
        };

        mockCacheService.Setup(cache => cache.GetAsync<Dictionary<Status, string>>(It.IsAny<string>()))
                        .ReturnsAsync(statusDict);

        var handler = new GetProductByIdQueryHandler(mockProductRepository.Object, mockCacheService.Object);
        var query = new GetProductByIdQuery(Guid.NewGuid());

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(query, default));
        Assert.That(ex!.Message, Is.EqualTo(string.Format(ValidationMessagesResources.StatusNotFoundInCache, product.Status)));
    }


    [Test]
    public async Task Handle_ShouldReturnProductResponse_WhenProductExists()
    {
        // Arrange
        string name = "Test Product";
        Status status = Status.Active;
        int stock = 5;
        string description = "Test Product";
        decimal price = 100m;
        byte discount = 10;

        var product = new Product(name, status, stock, description, price);
        product.SetDiscountPercentage(discount);

        mockProductRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(product);

        var statusDict = new Dictionary<Status, string>
        {
            { Status.Active, "Active" },
            { Status.Inactive, "Inactive" }
        };
        mockCacheService.Setup(cache => cache.GetAsync<Dictionary<Status, string>>(It.IsAny<string>()))
                        .ReturnsAsync(statusDict);

        var handler = new GetProductByIdQueryHandler(mockProductRepository.Object, mockCacheService.Object);
        var query = new GetProductByIdQuery(It.IsAny<Guid>());

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(product.Name));
        Assert.That(result.StatusName, Is.EqualTo(statusDict[product.Status]));
        Assert.That(result.Stock, Is.EqualTo(product.Stock));
        Assert.That(result.Description, Is.EqualTo(product.Description));
        Assert.That(result.Price, Is.EqualTo(product.Price));
        Assert.That(result.DiscountPercentage, Is.EqualTo(product.DiscountPercentage));
        Assert.That(result.FinalPrice, Is.EqualTo(price * (100 - discount) / 100));
    }
}
