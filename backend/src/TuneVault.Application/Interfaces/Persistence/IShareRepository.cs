using TuneVault.Domain.Entities;

namespace TuneVault.Application.Interfaces.Persistence;

public interface IShareRepository
{
    Task<MediaShare> AddAsync(MediaShare share, CancellationToken cancellationToken);
    Task<IReadOnlyList<MediaShare>> GetInboxByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<MediaShare>> GetSentByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<MediaShare?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}