using Microsoft.EntityFrameworkCore;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;
using TuneVault.Infrastructure.Persistence;

namespace TuneVault.Infrastructure.Repositories;

public class FavoriteRepository : IFavoriteRepository
{
    private readonly AppDbContext _dbContext;

    public FavoriteRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Favorite?> GetByUserAndMediaAsync(Guid userId, Guid mediaItemId, CancellationToken cancellationToken) => await _dbContext.Favorites
            .FirstOrDefaultAsync(x => x.UserId == userId && x.MediaItemId == mediaItemId, cancellationToken);

    public async Task<IReadOnlyList<Favorite>> GetFavoritesByUserAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken) => await _dbContext.Favorites
            .Where(x => x.UserId == userId)
            .Include(x => x.MediaItem)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public async Task<int> GetTotalFavoritesByUserAsync(Guid userId, CancellationToken cancellationToken) => await _dbContext.Favorites
            .CountAsync(x => x.UserId == userId, cancellationToken);

    public async Task AddAsync(Favorite favorite, CancellationToken cancellationToken)
    {
        _dbContext.Favorites.Add(favorite);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Favorite favorite, CancellationToken cancellationToken)
    {
        _dbContext.Favorites.Remove(favorite);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}