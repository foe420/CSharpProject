using MediatR;
using System;

namespace TuneVault.Application.Features.MediaItems.Queries.GetMediaStream;

public record GetMediaStreamQuery(Guid Id) : IRequest<MediaStreamDto>;

public class MediaStreamDto
{
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}
