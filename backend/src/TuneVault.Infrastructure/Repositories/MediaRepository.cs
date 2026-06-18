using Microsoft.EntityFrameworkCore;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;
using TuneVault.Domain.Enums;
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
        // Kiểm tra xem OwnerId có tồn tại trong bảng Users không
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == mediaItem.OwnerId, cancellationToken);
        
        if (user == null)
        {
            // Nếu không có, tìm ApplicationUser tương ứng
            var appUser = await _dbContext.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == mediaItem.OwnerId, cancellationToken);
            
            if (appUser != null)
            {
                // Tạo user mới trong bảng Users với CÙNG Id
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
                Console.WriteLine($"=== Created missing User with Id: {user.Id} ===");
            }
            else
            {
                // Fallback: lấy user đầu tiên
                user = await _dbContext.Users.FirstOrDefaultAsync(cancellationToken);
                if (user != null)
                {
                    mediaItem.OwnerId = user.Id;
                }
            }
        }

        _dbContext.MediaItems.Add(mediaItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return mediaItem;
    }

    public async Task<MediaItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.MediaItems
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<MediaItem> Items, int TotalCount)> SearchAsync(
        string? term, 
        MediaFileType? fileType, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken)
    {
        var query = _dbContext.MediaItems.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(term))
        {
            var cleanedTerm = term.Trim().ToLower();
            query = query.Where(x => x.Title.ToLower().Contains(cleanedTerm) ||
                                     x.Artist.ToLower().Contains(cleanedTerm) ||
                                     (x.Genre != null && x.Genre.ToLower().Contains(cleanedTerm)));
        }

        if (fileType.HasValue)
        {
            query = query.Where(x => x.FileType == fileType.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<MediaItem>> GetTrendingAsync(
        int count, 
        CancellationToken cancellationToken)
    {
        var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);

        return await _dbContext.MediaItems
            .AsNoTracking()
            .Select(mi => new
            {
                MediaItem = mi,
                PlayCount = mi.PlayHistoryEntries.Count(ph => ph.PlayedAt >= sevenDaysAgo)
            })
            .OrderByDescending(x => x.PlayCount)
            .ThenByDescending(x => x.MediaItem.CreatedAt)
            .Take(count)
            .Select(x => x.MediaItem)
            .ToListAsync(cancellationToken);
    }
}