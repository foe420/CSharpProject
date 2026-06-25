using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TuneVault.Domain.Entities;

namespace TuneVault.Application.Interfaces.Persistence;

public interface IMediaShareRepository
{
    Task<MediaShare?> GetExistingShareAsync(
        Guid senderId,
        Guid receiverId,
        Guid? mediaItemId,
        Guid? playlistId,
        CancellationToken cancellationToken);

    Task<MediaShare> AddAsync(
        MediaShare mediaShare,
        CancellationToken cancellationToken);

    Task<(IReadOnlyList<MediaShare> Items, int TotalCount)> GetReceivedSharesPaginatedAsync(
        Guid receiverId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

    Task<(IReadOnlyList<MediaShare> Items, int TotalCount)> GetSentSharesPaginatedAsync(
        Guid senderId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
}
