using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TuneVault.Application.Common.Exceptions;
using TuneVault.Application.Interfaces.Persistence;

namespace TuneVault.Application.Features.Notifications.Commands.MarkRead;

public record MarkNotificationReadCommand(Guid UserId, Guid NotificationId) : IRequest<Unit>;

public class MarkNotificationReadCommandHandler : IRequestHandler<MarkNotificationReadCommand, Unit>
{
    private readonly INotificationRepository _notificationRepository;

    public MarkNotificationReadCommandHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Unit> Handle(MarkNotificationReadCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationRepository.GetByIdAsync(request.NotificationId, cancellationToken);
        if (notification == null)
        {
            throw new NotFoundException($"Notification with ID {request.NotificationId} was not found.");
        }

        if (notification.UserId != request.UserId)
        {
            throw new ForbiddenException("You do not have permission to access this notification.");
        }

        notification.IsRead = true;
        await _notificationRepository.UpdateAsync(notification, cancellationToken);

        return Unit.Value;
    }
}
