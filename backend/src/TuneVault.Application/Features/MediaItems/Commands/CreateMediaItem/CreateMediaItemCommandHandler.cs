using MediatR;
using TuneVault.Application.Features.MediaItems.Queries.GetMediaItems;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;

namespace TuneVault.Application.Features.MediaItems.Commands.CreateMediaItem;

public class CreateMediaItemCommandHandler : IRequestHandler<CreateMediaItemCommand, MediaItemDto>
{
    private readonly IMediaRepository _mediaRepository;

    public CreateMediaItemCommandHandler(IMediaRepository mediaRepository)
    {
        _mediaRepository = mediaRepository;
    }

    public async Task<MediaItemDto> Handle(CreateMediaItemCommand request, CancellationToken cancellationToken)
    {
        var mediaItem = new MediaItem
        {
            Title = request.Title,
            Description = request.Description,
            MediaUrl = request.MediaUrl,
            ThumbnailUrl = request.ThumbnailUrl,
            DurationSeconds = request.DurationSeconds,
            MediaType = request.MediaType
        };

        var created = await _mediaRepository.AddAsync(mediaItem, cancellationToken);

        return new MediaItemDto
        {
            Id = created.Id,
            Title = created.Title,
            Description = created.Description,
            MediaUrl = created.MediaUrl,
            ThumbnailUrl = created.ThumbnailUrl,
            DurationSeconds = created.DurationSeconds,
            MediaType = created.MediaType
        };
    }
}
