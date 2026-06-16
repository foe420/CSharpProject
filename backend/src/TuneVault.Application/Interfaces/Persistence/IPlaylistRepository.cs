using TuneVault.Domain.Entities;

namespace TuneVault.Application.Interfaces.Persistence;

public interface IPlaylistRepository
{
    Task<Playlist?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Playlist> AddAsync(Playlist playlist, CancellationToken cancellationToken);
    Task UpdateAsync(Playlist playlist, CancellationToken cancellationToken);
    Task DeleteAsync(Playlist playlist, CancellationToken cancellationToken);
}
