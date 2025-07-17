namespace TektonChallengeProducts.Application.Tests.UseCases.CreateProduct;

using Moq;
using NUnit.Framework;
using Application.UseCases.CreateProduct;
using Application.UseCases.Validators;
using Domain.Abstractions.Persistence;
using Domain.Enums;
using Domain.Entities;

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

        var validator = new CreateOrUpdateProductCommandValidator();
        var command = new CreateProductCommand(Status.Active, 10, "desc", 100);

        var handler = new CreateProductCommandHandler(mockProductRepository.Object, validator);

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
