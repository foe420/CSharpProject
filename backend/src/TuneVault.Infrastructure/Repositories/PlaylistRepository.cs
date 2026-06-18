using Microsoft.EntityFrameworkCore;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;
using TuneVault.Infrastructure.Persistence;

namespace TuneVault.Infrastructure.Repositories;

public class PlaylistRepository : IPlaylistRepository
{
    private readonly AppDbContext _dbContext;

    public PlaylistRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Playlist> AddAsync(Playlist playlist, CancellationToken cancellationToken)
    {
        _dbContext.Playlists.Add(playlist);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return playlist;
    }

    public async Task<Playlist?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Playlists
            .Include(x => x.Owner)
            .Include(x => x.Tracks)
                .ThenInclude(t => t.MediaItem)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    // ✅ THÊM METHOD NÀY
    public async Task<IReadOnlyList<Playlist>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Playlists
            .Where(x => x.OwnerId == userId)
            .Include(x => x.Owner)
            .Include(x => x.Tracks)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Playlist playlist, CancellationToken cancellationToken)
    {
        _dbContext.Playlists.Update(playlist);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var playlist = await _dbContext.Playlists.FindAsync(id, cancellationToken);
        if (playlist != null)
        {
            _dbContext.Playlists.Remove(playlist);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}