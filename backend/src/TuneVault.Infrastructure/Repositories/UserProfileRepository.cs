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
}