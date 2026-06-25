using MediatR;

namespace TuneVault.Domain.Events;

public record MediaSharedEvent(
    Guid SenderId,
    Guid ReceiverId,
    Guid? MediaItemId,
    Guid? PlaylistId,
    string SenderName
) : INotification;
