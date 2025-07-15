namespace TektonChallengeProducts.Domain.Abstractions.Persistence;

using Entities.Base;

public interface IRepository<TEntity, TId> where TEntity : IEntity
{
    IUnitOfWork UnitOfWork { get; }

    Task CreateAsync(TEntity entity, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken);

    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken);

    void Update(TEntity entity);
}
