using System;
using System.Threading.Tasks;
using TuneVault.Application.Common.Models;

namespace TuneVault.Application.Interfaces.Services;

public interface INotificationPushService
{
    Task PushToUser(Guid receiverId, NotificationDto notificationDto);
}
