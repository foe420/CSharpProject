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
                    Id = appUser.Id,  // Dùng cùng Id
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
}
