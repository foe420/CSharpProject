using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TuneVault.Application.Common.Models;
using TuneVault.Application.Features.Notifications.Dtos;
using TuneVault.Application.Interfaces.Persistence;

namespace TuneVault.Application.Features.Notifications.Queries.GetNotifications;

public record GetNotificationsQuery(
    Guid UserId,
    int PageNumber = 1,
    int PageSize = 20
) : IRequest<NotificationsPageDto>;

public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, NotificationsPageDto>
{
    private readonly INotificationRepository _notificationRepository;

    public GetNotificationsQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<NotificationsPageDto> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        var page = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var size = request.PageSize <= 0 ? 20 : (request.PageSize > 50 ? 50 : request.PageSize);

        var (notifications, totalCount) = await _notificationRepository.GetUserNotificationsPaginatedAsync(
            request.UserId,
            page,
            size,
            cancellationToken);

        var unreadCount = await _notificationRepository.GetUnreadCountAsync(request.UserId, cancellationToken);

        var items = notifications.Select(n => new NotificationDto
        {
            Id = n.Id,
            Type = n.Type,
            PayloadJson = n.PayloadJson,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt
        }).ToList();

        return new NotificationsPageDto
        {
            Items = items,
            UnreadCount = unreadCount,
            TotalCount = totalCount,
            PageNumber = page,
            PageSize = size
        };
    }
}
