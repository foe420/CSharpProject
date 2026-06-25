using System;
using MediatR;

namespace TuneVault.Application.Features.Shares.Commands.ShareMedia;

public class ShareMediaCommand : IRequest<ShareResponseDto>
{
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string? ReceiverEmail { get; set; }
    public Guid? MediaItemId { get; set; }
    public Guid? PlaylistId { get; set; }
}

public record ShareResponseDto(
    Guid ShareId,
    string ReceiverEmail,
    DateTime SharedAt
);
