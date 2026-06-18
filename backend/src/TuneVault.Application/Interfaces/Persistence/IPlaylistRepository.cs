using TuneVault.Domain.Entities;

namespace TuneVault.Application.Interfaces.Persistence;

public interface IPlaylistRepository
{
    Task<Playlist> AddAsync(Playlist playlist, CancellationToken cancellationToken);
    Task<Playlist?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Playlist>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);  // ← THÊM DÒNG NÀY
    Task UpdateAsync(Playlist playlist, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}