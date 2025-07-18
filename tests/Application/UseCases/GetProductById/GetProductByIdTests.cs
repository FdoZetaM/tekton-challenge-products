namespace TektonChallengeProducts.Application.Tests.UseCases.GetProductById;

using Moq;
using NUnit.Framework;
using Application.UseCases.GetProductById;
using Domain.Abstractions.Persistence;
using Domain.Entities;
using Domain.Enums;

[TestFixture]
public class GetProductByIdTests
{
    private readonly Mock<IProductRepository> mockProductRepository;

    public GetProductByIdTests()
    {
        mockProductRepository = new Mock<IProductRepository>();
    }

    [Test]
    public async Task Handle_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();
        mockProductRepository.Setup(repo => repo.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Product?)null);

        var handler = new GetProductByIdQueryHandler(mockProductRepository.Object);
        var query = new GetProductByIdQuery(productId);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.That(result, Is.Null);
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

        var handler = new GetProductByIdQueryHandler(mockProductRepository.Object);
        var query = new GetProductByIdQuery(It.IsAny<Guid>());

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(product.Name));
        Assert.That(result.Status, Is.EqualTo(product.Status.ToString()));
        Assert.That(result.Stock, Is.EqualTo(product.Stock));
        Assert.That(result.Description, Is.EqualTo(product.Description));
        Assert.That(result.Price, Is.EqualTo(product.Price));
        Assert.That(result.DiscountPercentage, Is.EqualTo(product.DiscountPercentage));
        Assert.That(result.FinalPrice, Is.EqualTo(price * (100 - discount) / 100));
    }
}
