using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TuneVault.Application.Common.Models;
using TuneVault.Application.Interfaces.Services;
using TuneVault.Infrastructure.Hubs;

namespace TuneVault.Infrastructure.Services;

public class SignalRNotificationPushService : INotificationPushService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRNotificationPushService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task PushToUser(Guid receiverId, NotificationDto notificationDto)
    {
        await _hubContext.Clients.User(receiverId.ToString())
            .SendAsync("ReceiveNotification", notificationDto);
    }
}
