using MediatR;
using TuneVault.Application.Interfaces.Persistence;

namespace TuneVault.Application.Features.MediaItems.Queries.GetMediaItems;

public class GetMediaItemsQueryHandler : IRequestHandler<GetMediaItemsQuery, IReadOnlyList<MediaItemDto>>
{
    private readonly IMediaRepository _mediaRepository;

    public GetMediaItemsQueryHandler(IMediaRepository mediaRepository)
    {
        _mediaRepository = mediaRepository;
    }

    public async Task<IReadOnlyList<MediaItemDto>> Handle(GetMediaItemsQuery request, CancellationToken cancellationToken)
    {
        var mediaItems = await _mediaRepository.GetLibraryAsync(cancellationToken);

        return mediaItems.Select(m => new MediaItemDto
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            MediaUrl = m.MediaUrl,
            ThumbnailUrl = m.ThumbnailUrl,
            MediaType = m.MediaType,
            DurationSeconds = m.DurationSeconds
        }).ToList();
    }
}
