using TuneVault.Domain.Entities;
using TuneVault.Domain.Enums;

namespace TuneVault.Application.Interfaces.Persistence;

public interface IMediaRepository
{
    Task<IReadOnlyList<MediaItem>> GetLibraryAsync(CancellationToken cancellationToken);
    Task<MediaItem> AddAsync(MediaItem mediaItem, CancellationToken cancellationToken);
    Task<MediaItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<(IReadOnlyList<MediaItem> Items, int TotalCount)> SearchAsync(string? term, MediaFileType? fileType, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<IReadOnlyList<MediaItem>> GetTrendingAsync(int count, CancellationToken cancellationToken);
}
