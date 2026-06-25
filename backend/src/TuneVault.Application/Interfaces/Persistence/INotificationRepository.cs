using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TuneVault.Domain.Entities;

namespace TuneVault.Application.Interfaces.Persistence;

public interface INotificationRepository
{
    Task<Notification> AddAsync(Notification notification, CancellationToken cancellationToken);
    Task<(IReadOnlyList<Notification> Items, int TotalCount)> GetUserNotificationsPaginatedAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<int> GetUnreadCountAsync(Guid userId, CancellationToken cancellationToken);
    Task<Notification?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(Notification notification, CancellationToken cancellationToken);
    Task MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken);
}
