using MediatR;
using TuneVault.Application.Interfaces.Persistence;

namespace TuneVault.Application.Features.MediaItems.Queries.GetMediaItems;

public class GetMediaItemsQueryHandler : IRequestHandler<GetMediaItemsQuery, IReadOnlyList<MediaItemDto>>
{
    private readonly IMediaRepository _mediaRepository;

    public GetMediaItemsQueryHandler(IMediaRepository mediaRepository) => _mediaRepository = mediaRepository;

    public async Task<IReadOnlyList<MediaItemDto>> Handle(GetMediaItemsQuery request, CancellationToken cancellationToken)
    {
        var mediaItems = await _mediaRepository.GetLibraryAsync(cancellationToken);

        return mediaItems.Select(m => new MediaItemDto
        {
            Id = m.Id,
            Title = m.Title,
            Artist = m.Artist,
            Genre = m.Genre,
            FilePath = m.FilePath,
            FileType = m.FileType,
            Duration = m.Duration,
            Description = m.Description,
            CreatedAt = m.CreatedAt
        }).ToList();
    }
}
