using MediatR;
using Microsoft.AspNetCore.Http;

namespace TuneVault.Application
.Features.MediaItems.Commands.UploadMedia;

public class UploadMediaCommand : IRequest<Guid>
{
    public required IFormFile File { get; set; }

    public Guid OwnerId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Artist { get; set; } = string.Empty;

    public string? Genre { get; set; }

    public string? Description { get; set; }
}