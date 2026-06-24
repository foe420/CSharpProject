using Microsoft.EntityFrameworkCore;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;
using TuneVault.Infrastructure.Persistence;

namespace TuneVault.Infrastructure.Repositories;

public class FavoriteRepository : IFavoriteRepository
{
    private readonly AppDbContext _context;

    public FavoriteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Favorite?> GetAsync(Guid userId, Guid mediaItemId, CancellationToken cancellationToken)
    {
        return await _context.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.MediaItemId == mediaItemId, cancellationToken);
    }

    public async Task<Favorite> AddAsync(Favorite favorite, CancellationToken cancellationToken)
    {
        _context.Favorites.Add(favorite);
        await _context.SaveChangesAsync(cancellationToken);
        return favorite;
    }

    public async Task DeleteAsync(Favorite favorite, CancellationToken cancellationToken)
    {
        _context.Favorites.Remove(favorite);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<(List<Favorite> Items, int TotalCount)> GetByUserIdAsync(
        Guid userId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Favorites
            .Where(f => f.UserId == userId)
            .Include(f => f.MediaItem)
            .OrderByDescending(f => f.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
