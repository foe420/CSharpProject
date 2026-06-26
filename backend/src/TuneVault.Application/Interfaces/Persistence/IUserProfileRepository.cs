using TuneVault.Domain.Entities;

namespace TuneVault.Application.Interfaces.Persistence;

public interface IUserProfileRepository
{
    Task<UserProfile> AddAsync(UserProfile userProfile, CancellationToken cancellationToken = default);
    Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserProfile userProfile, CancellationToken cancellationToken = default);
}