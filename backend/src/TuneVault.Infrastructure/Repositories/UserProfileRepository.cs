using Microsoft.EntityFrameworkCore;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;
using TuneVault.Infrastructure.Persistence;

namespace TuneVault.Infrastructure.Repositories;

public sealed class UserProfileRepository : IUserProfileRepository
{
    private readonly AppDbContext _dbContext;

    public UserProfileRepository(AppDbContext dbContext) => _dbContext = dbContext;

    public async Task<UserProfile> AddAsync(UserProfile userProfile, CancellationToken cancellationToken = default)
    {
        _dbContext.UserProfiles.Add(userProfile);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return userProfile;
    }

    public async Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserProfiles
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
    }

    public async Task UpdateAsync(UserProfile userProfile, CancellationToken cancellationToken = default)
    {
        _dbContext.UserProfiles.Update(userProfile);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}