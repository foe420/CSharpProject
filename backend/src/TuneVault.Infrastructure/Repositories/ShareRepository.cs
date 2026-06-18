using Microsoft.EntityFrameworkCore;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;
using TuneVault.Infrastructure.Persistence;

namespace TuneVault.Infrastructure.Repositories;

public class ShareRepository : IShareRepository
{
    private readonly AppDbContext _dbContext;

    public ShareRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MediaShare> AddAsync(MediaShare share, CancellationToken cancellationToken)
    {
        _dbContext.MediaShares.Add(share);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return share;
    }

    public async Task<IReadOnlyList<MediaShare>> GetInboxByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.MediaShares
            .Where(x => x.ReceiverId == userId)
            .Include(x => x.Sender)
            .Include(x => x.Receiver)
            .Include(x => x.MediaItem)
            .Include(x => x.Playlist)
            .OrderByDescending(x => x.SharedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<MediaShare>> GetSentByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.MediaShares
            .Where(x => x.SenderId == userId)
            .Include(x => x.Sender)
            .Include(x => x.Receiver)
            .Include(x => x.MediaItem)
            .Include(x => x.Playlist)
            .OrderByDescending(x => x.SharedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<MediaShare?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.MediaShares
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}