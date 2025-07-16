namespace TektonChallengeProducts.Infrastructure.Persistence.Sql;

using Microsoft.EntityFrameworkCore;
using Domain.Abstractions.Persistence;
using EntityConfigurations;

public class EntityFrameworkUnitOfWork(DbContextOptions<EntityFrameworkUnitOfWork> options)
    : DbContext(options), IUnitOfWork
{
    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return await SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProductsEntityConfiguration());
    }
}
