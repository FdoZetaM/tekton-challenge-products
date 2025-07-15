namespace TektonChallengeProducts.Domain.Abstractions.Persistence;

using Domain.Entities;

public interface IProductRepository : IRepository<Product, Guid>
{
}
