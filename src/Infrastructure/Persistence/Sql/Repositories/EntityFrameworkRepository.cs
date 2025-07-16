namespace TektonChallengeProducts.Infrastructure.Persistence.Sql.Repositories;

using Microsoft.EntityFrameworkCore;
using Domain.Abstractions.Persistence;
using Domain.Entities.Base;

public class EntityFrameworkRepository<TEntity, TId>(EntityFrameworkUnitOfWork unitOfWork) : IRepository<TEntity, TId> where TEntity : class, IEntity
{
    private readonly EntityFrameworkUnitOfWork unitOfWork = unitOfWork;

    protected readonly DbSet<TEntity> dbSet = unitOfWork.Set<TEntity>();

    public IUnitOfWork UnitOfWork => unitOfWork;

    public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken)
    {
        return await GetByIdAsync(id, cancellationToken) is not null;
    }

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        return await dbSet.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
    }

    public void Update(TEntity entity)
    {
        dbSet.Update(entity);
    }
}
