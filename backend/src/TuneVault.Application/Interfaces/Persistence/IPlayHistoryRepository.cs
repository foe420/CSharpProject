using TuneVault.Domain.Entities;

namespace TuneVault.Application.Interfaces.Persistence;

public interface IPlayHistoryRepository
{
    Task AddAsync(PlayHistory history, CancellationToken cancellationToken);
    Task<IReadOnlyList<PlayHistory>> GetRecentHistoryByUserAsync(Guid userId, int limit, CancellationToken cancellationToken);
}