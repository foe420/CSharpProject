using MediatR;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TuneVault.Application.Common.Models;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Application.Interfaces.Services;
using TuneVault.Domain.Entities;
using TuneVault.Domain.Events;

namespace TuneVault.Application.Features.Follows.Events;

public class UserFollowedEventHandler : INotificationHandler<UserFollowedEvent>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationPushService _notificationPushService;

    public UserFollowedEventHandler(
        INotificationRepository notificationRepository,
        INotificationPushService notificationPushService)
    {
        _notificationRepository = notificationRepository;
        _notificationPushService = notificationPushService;
    }

    public async Task Handle(UserFollowedEvent notification, CancellationToken cancellationToken)
    {
        string message = $"{notification.FollowerName} started following you.";

        var payloadObj = new
        {
            followerId = notification.FollowerId,
            followerName = notification.FollowerName,
            message = message
        };

        string payloadJson = JsonSerializer.Serialize(payloadObj);

        var dbNotification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = notification.FolloweeId,
            Type = "Follow",
            PayloadJson = payloadJson,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(dbNotification, cancellationToken);

        var notificationDto = new NotificationDto
        {
            Id = dbNotification.Id,
            Type = dbNotification.Type,
            PayloadJson = dbNotification.PayloadJson,
            IsRead = dbNotification.IsRead,
            CreatedAt = dbNotification.CreatedAt
        };

        await _notificationPushService.PushToUser(notification.FolloweeId, notificationDto);
    }
}
