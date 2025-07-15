namespace TektonChallengeProducts.Domain.Entities.Base;

public abstract class Entity<T>(T id) : IEntity
{
    public T Id { get; private set; } = id;

    object IEntity.Id
    {
        get { return Id!; }
    }
}
