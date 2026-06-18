using Microsoft.EntityFrameworkCore;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;
using TuneVault.Infrastructure.Persistence;

namespace TuneVault.Infrastructure.Repositories;

public class PlayHistoryRepository : IPlayHistoryRepository
{
    private readonly AppDbContext _dbContext;

    public PlayHistoryRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(PlayHistory history, CancellationToken cancellationToken)
    {
        _dbContext.PlayHistories.Add(history);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PlayHistory>> GetRecentHistoryByUserAsync(Guid userId, int limit, CancellationToken cancellationToken) => await _dbContext.PlayHistories
            .Where(x => x.UserId == userId)
            .Include(x => x.MediaItem)
            .OrderByDescending(x => x.PlayedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
}