namespace TektonChallengeProducts.Domain.Abstractions.Persistence;

using System.Threading.Tasks;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken);
}
