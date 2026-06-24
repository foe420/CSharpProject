using TuneVault.Domain.Entities;

namespace TuneVault.Application.Interfaces.Persistence;

public interface IFavoriteRepository
{
    Task<Favorite?> GetAsync(Guid userId, Guid mediaItemId, CancellationToken cancellationToken);
    Task<Favorite> AddAsync(Favorite favorite, CancellationToken cancellationToken);
    Task DeleteAsync(Favorite favorite, CancellationToken cancellationToken);
    Task<(List<Favorite> Items, int TotalCount)> GetByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken);
}
