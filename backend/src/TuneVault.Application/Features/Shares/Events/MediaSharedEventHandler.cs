using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TuneVault.Application.Common.Models;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Application.Interfaces.Services;
using TuneVault.Domain.Entities;
using TuneVault.Domain.Events;

namespace TuneVault.Application.Features.Shares.Events;

public class MediaSharedEventHandler : INotificationHandler<MediaSharedEvent>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationPushService _notificationPushService;
    private readonly IMediaRepository _mediaRepository;
    private readonly IPlaylistRepository _playlistRepository;

    public MediaSharedEventHandler(
        INotificationRepository notificationRepository,
        INotificationPushService notificationPushService,
        IMediaRepository mediaRepository,
        IPlaylistRepository playlistRepository)
    {
        _notificationRepository = notificationRepository;
        _notificationPushService = notificationPushService;
        _mediaRepository = mediaRepository;
        _playlistRepository = playlistRepository;
    }

    public async Task Handle(MediaSharedEvent notification, CancellationToken cancellationToken)
    {
        string title = "Item";
        if (notification.MediaItemId.HasValue)
        {
            var mediaItem = await _mediaRepository.GetByIdAsync(notification.MediaItemId.Value, cancellationToken);
            title = mediaItem?.Title ?? "Unknown Track";
        }
        else if (notification.PlaylistId.HasValue)
        {
            var playlist = await _playlistRepository.GetByIdAsync(notification.PlaylistId.Value, cancellationToken);
            title = playlist?.Title ?? "Unknown Playlist";
        }

        string itemType = notification.MediaItemId.HasValue ? "track" : "playlist";
        string message = $"{notification.SenderName} shared a {itemType}: \"{title}\"";

        var payloadObj = new
        {
            senderId = notification.SenderId,
            senderName = notification.SenderName,
            mediaId = notification.MediaItemId,
            playlistId = notification.PlaylistId,
            title = title,
            message = message
        };

        string payloadJson = JsonSerializer.Serialize(payloadObj);

        var dbNotification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = notification.ReceiverId,
            Type = "MediaShared",
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

        await _notificationPushService.PushToUser(notification.ReceiverId, notificationDto);
    }
}
