using TuneVault.Domain.Entities;

namespace TuneVault.Application.Interfaces.Persistence;

public interface IPlayHistoryRepository
{
    Task AddAsync(PlayHistory playHistory, CancellationToken cancellationToken);
    Task<List<PlayHistory>> GetRecentByUserIdAsync(Guid userId, int limit, CancellationToken cancellationToken);
}
