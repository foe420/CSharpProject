using TuneVault.Domain.Entities;

namespace TuneVault.Application.Interfaces.Persistence;

public interface IFavoriteRepository
{
    Task<Favorite?> GetByUserAndMediaAsync(Guid userId, Guid mediaItemId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Favorite>> GetFavoritesByUserAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken);
    Task<int> GetTotalFavoritesByUserAsync(Guid userId, CancellationToken cancellationToken);
    Task AddAsync(Favorite favorite, CancellationToken cancellationToken);
    Task DeleteAsync(Favorite favorite, CancellationToken cancellationToken);
}