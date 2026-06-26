using MediatR;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TuneVault.Application.Interfaces.Persistence;

namespace TuneVault.Application.Features.MediaItems.Queries.GetMediaStream;

public class GetMediaStreamQueryHandler : IRequestHandler<GetMediaStreamQuery, MediaStreamDto>
{
    private readonly IMediaRepository _mediaRepository;

    public GetMediaStreamQueryHandler(IMediaRepository mediaRepository)
    {
        _mediaRepository = mediaRepository;
    }

    public async Task<MediaStreamDto> Handle(GetMediaStreamQuery request, CancellationToken cancellationToken)
    {
        var media = await _mediaRepository.GetByIdAsync(request.Id, cancellationToken);
        if (media == null)
        {
            throw new KeyNotFoundException("Media item not found.");
        }

        var extension = Path.GetExtension(media.FilePath).ToLowerInvariant();
        string contentType = extension switch
        {
            ".mp3" => "audio/mpeg",
            ".wav" => "audio/wav",
            ".mp4" => "video/mp4",
            ".webm" => "video/webm",
            _ => "application/octet-stream"
        };

        return new MediaStreamDto
        {
            FilePath = media.FilePath,
            ContentType = contentType
        };
    }
}
