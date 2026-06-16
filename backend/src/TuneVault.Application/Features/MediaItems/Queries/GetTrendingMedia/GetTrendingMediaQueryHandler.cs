using MediatR;
using TuneVault.Application.Features.Playlists.Dtos;
using TuneVault.Application.Interfaces.Persistence;

namespace TuneVault.Application.Features.MediaItems.Queries.GetTrendingMedia;

public class GetTrendingMediaQueryHandler : IRequestHandler<GetTrendingMediaQuery, List<MediaItemSummaryDto>>
{
    private readonly IMediaRepository _mediaRepository;

    public GetTrendingMediaQueryHandler(IMediaRepository mediaRepository)
    {
        _mediaRepository = mediaRepository;
    }

    public async Task<List<MediaItemSummaryDto>> Handle(GetTrendingMediaQuery request, CancellationToken cancellationToken)
    {
        var items = await _mediaRepository.GetTrendingAsync(10, cancellationToken);

        return items.Select(m => new MediaItemSummaryDto
        {
            Id = m.Id,
            Title = m.Title,
            Artist = m.Artist,
            Duration = m.Duration,
            FileType = m.FileType.ToString(),
            StreamUrl = $"/api/media/{m.Id}/stream"
        }).ToList();
    }
}
