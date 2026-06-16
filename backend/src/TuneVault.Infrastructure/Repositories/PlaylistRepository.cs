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

    public async Task<Playlist?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Playlists
            .Include(p => p.Owner)
                .ThenInclude(o => o!.Profile)
            .Include(p => p.Tracks)
                .ThenInclude(t => t.MediaItem)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Playlist> AddAsync(Playlist playlist, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == playlist.OwnerId, cancellationToken);
        
        if (user == null)
        {
            var appUser = await _dbContext.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == playlist.OwnerId, cancellationToken);
            
            if (appUser != null)
            {
                user = new User
                {
                    Id = appUser.Id,
                    UserName = appUser.UserName ?? appUser.Email,
                    Email = appUser.Email,
                    PasswordHash = appUser.PasswordHash,
                    CreatedAtUtc = appUser.CreatedAtUtc
                };
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        _dbContext.Playlists.Add(playlist);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return playlist;
    }

    public async Task UpdateAsync(Playlist playlist, CancellationToken cancellationToken)
    {
        _dbContext.Playlists.Update(playlist);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Playlist playlist, CancellationToken cancellationToken)
    {
        _dbContext.Playlists.Remove(playlist);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
