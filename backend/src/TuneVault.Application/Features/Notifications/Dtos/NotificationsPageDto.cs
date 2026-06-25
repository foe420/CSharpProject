using System.Collections.Generic;
using TuneVault.Application.Common.Models;

namespace TuneVault.Application.Features.Notifications.Dtos;

public class NotificationsPageDto
{
    public List<NotificationDto> Items { get; set; } = new();
    public int UnreadCount { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
