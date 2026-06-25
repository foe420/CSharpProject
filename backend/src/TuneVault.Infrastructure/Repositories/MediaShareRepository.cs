using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;
using TuneVault.Infrastructure.Persistence;

namespace TuneVault.Infrastructure.Repositories;

public class MediaShareRepository : IMediaShareRepository
{
    private readonly AppDbContext _dbContext;

    public MediaShareRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MediaShare?> GetExistingShareAsync(
        Guid senderId,
        Guid receiverId,
        Guid? mediaItemId,
        Guid? playlistId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.MediaShares
            .FirstOrDefaultAsync(s => 
                s.SenderId == senderId &&
                s.ReceiverId == receiverId &&
                s.MediaItemId == mediaItemId &&
                s.PlaylistId == playlistId, 
                cancellationToken);
    }

    public async Task<MediaShare> AddAsync(MediaShare mediaShare, CancellationToken cancellationToken)
    {
        // Ensure both Sender and Receiver exist in the domain Users table (sync from ApplicationUsers if missing)
        await EnsureUserExistsAsync(mediaShare.SenderId, cancellationToken);
        await EnsureUserExistsAsync(mediaShare.ReceiverId, cancellationToken);

        _dbContext.MediaShares.Add(mediaShare);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return mediaShare;
    }

    public async Task<(IReadOnlyList<MediaShare> Items, int TotalCount)> GetReceivedSharesPaginatedAsync(
        Guid receiverId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.MediaShares
            .AsNoTracking()
            .Where(s => s.ReceiverId == receiverId)
            .Include(s => s.Sender)
                .ThenInclude(u => u!.Profile)
            .Include(s => s.Receiver)
                .ThenInclude(u => u!.Profile)
            .Include(s => s.MediaItem)
            .Include(s => s.Playlist);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(s => s.SharedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<MediaShare> Items, int TotalCount)> GetSentSharesPaginatedAsync(
        Guid senderId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.MediaShares
            .AsNoTracking()
            .Where(s => s.SenderId == senderId)
            .Include(s => s.Sender)
                .ThenInclude(u => u!.Profile)
            .Include(s => s.Receiver)
                .ThenInclude(u => u!.Profile)
            .Include(s => s.MediaItem)
            .Include(s => s.Playlist);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(s => s.SharedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    private async Task EnsureUserExistsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.Users.AnyAsync(u => u.Id == userId, cancellationToken);
        if (!exists)
        {
            var appUser = await _dbContext.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (appUser != null)
            {
                var user = new User
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
    }
}
