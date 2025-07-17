namespace TektonChallengeProducts.Application.Tests.UseCases.CreateProduct;

using Moq;
using NUnit.Framework;
using Application.UseCases.CreateProduct;
using Domain.Entities;
using Domain.Abstractions.Persistence;
using TektonChallengeProducts.Domain.Enums;

[TestFixture]
public class CreateProductTests
{
    private readonly Mock<IProductRepository> mockProductRepository;
    private readonly Mock<IUnitOfWork> mockUnitOfWork;

    public CreateProductTests()
    {
        mockProductRepository = new Mock<IProductRepository>();
        mockUnitOfWork = new Mock<IUnitOfWork>();
    }

    [Test]
    public async Task Handle_ShouldCreateProductAndReturnId()
    {
        // Arrange
        mockProductRepository.SetupGet(repo => repo.UnitOfWork)
                             .Returns(mockUnitOfWork.Object);
        mockProductRepository.Setup(repo => repo.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                             .Returns(Task.CompletedTask);
        mockUnitOfWork.Setup(unit => unit.CommitAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(1);

        var handler = new CreateProductCommandHandler(mockProductRepository.Object);
        var command = new CreateProductCommand(Status.Active, 10, "desc", 100);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.That(result, Is.Not.EqualTo(Guid.Empty));
        mockProductRepository.Verify(repo => repo.CreateAsync(It.IsAny<Product>(),
                                                              It.IsAny<CancellationToken>()),
                                                              Times.Once);
        mockUnitOfWork.Verify(unit => unit.CommitAsync(It.IsAny<CancellationToken>()),
                                                       Times.Once);
    }
}
