using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TuneVault.Application.Interfaces.Persistence;

namespace TuneVault.Application.Features.Notifications.Commands.MarkAllRead;

public record MarkAllReadCommand(Guid UserId) : IRequest<Unit>;

public class MarkAllReadCommandHandler : IRequestHandler<MarkAllReadCommand, Unit>
{
    private readonly INotificationRepository _notificationRepository;

    public MarkAllReadCommandHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Unit> Handle(MarkAllReadCommand request, CancellationToken cancellationToken)
    {
        await _notificationRepository.MarkAllAsReadAsync(request.UserId, cancellationToken);
        return Unit.Value;
    }
}
