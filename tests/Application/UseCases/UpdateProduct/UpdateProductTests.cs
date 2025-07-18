namespace TektonChallengeProducts.Application.Tests.UseCases.UpdateProduct;

using FluentValidation;
using Moq;
using NUnit.Framework;
using Application.Resources;
using Application.UseCases.UpdateProduct;
using Application.UseCases.Validators;
using Domain.Abstractions.Persistence;
using Domain.Enums;
using Domain.Entities;

[TestFixture]
public class UpdateProductTests
{
    private readonly Mock<IProductRepository> mockProductRepository;
    private readonly Mock<IUnitOfWork> mockUnitOfWork;

    public UpdateProductTests()
    {
        mockProductRepository = new Mock<IProductRepository>();
        mockUnitOfWork = new Mock<IUnitOfWork>();
    }

    [Test]
    public void Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
    {
        // Arrange
        var id = Guid.NewGuid();
        string name = string.Empty;
        int stock = 10;
        string description = "desc";
        decimal price = 100m;

        var command = new UpdateProductCommand(
            id,
            name,
            Status.Active,
            stock,
            description,
            price
        );

        var validator = new ProductCommandValidator();
        var handler = new UpdateProductCommandHandler(mockProductRepository.Object, validator);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ValidationException>(async () => await handler.Handle(command, default));
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex!.Message, Does.Contain(ValidationMessagesResources.NameMandatory));
    }

    [Test]
    public void Handle_ShouldThrowKeyNotFoundException_WhenProductDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        string name = "Test Product";
        Status status = Status.Active;
        int stock = 10;
        string description = "desc";
        decimal price = 100m;

        var command = new UpdateProductCommand(
            id,
            name,
            status,
            stock,
            description,
            price
        );

        var validator = new ProductCommandValidator();
        mockProductRepository.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Product?)null);

        var handler = new UpdateProductCommandHandler(mockProductRepository.Object, validator);

        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await handler.Handle(command, default));
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex!.Message, Does.Contain(ValidationMessagesResources.ProductNotFound));
    }

    [Test]
    public async Task Handle_ShouldUpdateProductAndReturnId()
    {
        // Arrange
        string newName = "New Name";
        Status newStatus = Status.Active;
        int newStock = 10;
        string newDescription = "New Desc";
        decimal newPrice = 100m;

        var product = new Product("Old Name", Status.Inactive, 5, "Old Desc", 50m);
        mockProductRepository.Setup(repo => repo.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(product);
        mockProductRepository.SetupGet(repo => repo.UnitOfWork)
                             .Returns(mockUnitOfWork.Object);
        mockUnitOfWork.Setup(unit => unit.CommitAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(1);

        var command = new UpdateProductCommand(
            product.Id,
            newName,
            newStatus,
            newStock,
            newDescription,
            newPrice
        );

        var validator = new ProductCommandValidator();
        var handler = new UpdateProductCommandHandler(mockProductRepository.Object, validator);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.That(result, Is.EqualTo(product.Id));
        mockProductRepository.Verify(repo => repo.Update(product), Times.Once);
        mockUnitOfWork.Verify(unit => unit.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.That(product.Name, Is.EqualTo(newName));
        Assert.That(product.Status, Is.EqualTo(Status.Active));
        Assert.That(product.Stock, Is.EqualTo(newStock));
        Assert.That(product.Description, Is.EqualTo(newDescription));
        Assert.That(product.Price, Is.EqualTo(newPrice));
    }
}
