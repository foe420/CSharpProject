using Microsoft.EntityFrameworkCore;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;
using TuneVault.Infrastructure.Persistence;

namespace TuneVault.Infrastructure.Repositories;

public class MediaRepository : IMediaRepository
{
    private readonly AppDbContext _dbContext;

    public MediaRepository(AppDbContext dbContext) => _dbContext = dbContext;

    public async Task<IReadOnlyList<MediaItem>> GetLibraryAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.MediaItems
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<MediaItem> AddAsync(MediaItem mediaItem, CancellationToken cancellationToken)
    {
        var defaultUser = await _dbContext.Users.FirstOrDefaultAsync(cancellationToken);
        if (defaultUser is null)
        {
            defaultUser = new User
            {
                UserName = "demo",
                Email = "demo@tunevault.local",
                PasswordHash = "seed-password-hash"
            };
            _dbContext.Users.Add(defaultUser);
        }

        mediaItem.OwnerId = defaultUser.Id;

        _dbContext.MediaItems.Add(mediaItem);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return mediaItem;
    }
}
