using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;
using TuneVault.Infrastructure.Persistence;

namespace TuneVault.Infrastructure.Repositories;

public class FollowRepository : IFollowRepository
{
    private readonly AppDbContext _dbContext;

    public FollowRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Follow?> GetAsync(Guid followerId, Guid followeeId, CancellationToken cancellationToken)
    {
        return await _dbContext.Follows
            .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId, cancellationToken);
    }

    public async Task<Follow> AddAsync(Follow follow, CancellationToken cancellationToken)
    {
        await EnsureUserExistsAsync(follow.FollowerId, cancellationToken);
        await EnsureUserExistsAsync(follow.FolloweeId, cancellationToken);

        _dbContext.Follows.Add(follow);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return follow;
    }

    public async Task DeleteAsync(Follow follow, CancellationToken cancellationToken)
    {
        _dbContext.Follows.Remove(follow);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureUserExistsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.Users.AnyAsync(u => u.Id == userId, cancellationToken);
        if (!exists)
        {
            var appUser = await _dbContext.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (appUser != null)
            {
                var user = new User
                {
                    Id = appUser.Id,
                    UserName = appUser.UserName ?? appUser.Email,
                    Email = appUser.Email,
                    PasswordHash = appUser.PasswordHash,
                    CreatedAtUtc = appUser.CreatedAtUtc
                };
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
