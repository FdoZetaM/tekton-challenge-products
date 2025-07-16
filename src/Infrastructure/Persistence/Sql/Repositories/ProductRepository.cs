namespace TektonChallengeProducts.Infrastructure.Persistence.Sql.Repositories;

using Domain.Abstractions.Persistence;
using Domain.Entities;

public class ProductRepository(EntityFrameworkUnitOfWork unitOfWork)
    : EntityFrameworkRepository<Product, Guid>(unitOfWork), IProductRepository
{
}
