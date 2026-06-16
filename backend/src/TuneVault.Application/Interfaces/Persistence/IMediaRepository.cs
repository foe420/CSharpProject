using TuneVault.Domain.Entities;

namespace TuneVault.Application.Interfaces.Persistence;

public interface IMediaRepository
{
    Task<IReadOnlyList<MediaItem>> GetLibraryAsync(CancellationToken cancellationToken);
    Task<MediaItem> AddAsync(MediaItem mediaItem, CancellationToken cancellationToken);
    Task<MediaItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
