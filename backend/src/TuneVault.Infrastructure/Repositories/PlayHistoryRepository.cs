using Microsoft.EntityFrameworkCore;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;
using TuneVault.Infrastructure.Persistence;

namespace TuneVault.Infrastructure.Repositories;

public class PlayHistoryRepository : IPlayHistoryRepository
{
    private readonly AppDbContext _context;

    public PlayHistoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(PlayHistory playHistory, CancellationToken cancellationToken)
    {
        _context.PlayHistories.Add(playHistory);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<PlayHistory>> GetRecentByUserIdAsync(Guid userId, int limit, CancellationToken cancellationToken)
    {
        // Last 10 distinct MediaItems by PlayedAt desc
        return await _context.PlayHistories
            .Where(h => h.UserId == userId)
            .Include(h => h.MediaItem)
            .OrderByDescending(h => h.PlayedAt)
            .GroupBy(h => h.MediaItemId)
            .Select(g => g.First())
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}
