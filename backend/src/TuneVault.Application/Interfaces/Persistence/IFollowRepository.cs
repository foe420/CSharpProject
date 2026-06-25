using System;
using System.Threading;
using System.Threading.Tasks;
using TuneVault.Domain.Entities;

namespace TuneVault.Application.Interfaces.Persistence;

public interface IFollowRepository
{
    Task<Follow?> GetAsync(Guid followerId, Guid followeeId, CancellationToken cancellationToken);
    Task<Follow> AddAsync(Follow follow, CancellationToken cancellationToken);
    Task DeleteAsync(Follow follow, CancellationToken cancellationToken);
}
