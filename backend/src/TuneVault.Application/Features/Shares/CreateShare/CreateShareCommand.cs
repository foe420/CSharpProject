using MediatR;

namespace TuneVault.Application.Features.Shares.CreateShare;

public class CreateShareCommand : IRequest<ShareResponseDto>
{
    public string ReceiverEmail { get; set; } = string.Empty;
    public Guid? MediaItemId { get; set; }
    public Guid? PlaylistId { get; set; }
    public Guid SenderId { get; set; }
}

public class ShareResponseDto
{
    public Guid Id { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;
    public DateTime SharedAt { get; set; }
    public Guid? MediaItemId { get; set; }
    public Guid? PlaylistId { get; set; }
}