using MediatR;
using TuneVault.Application.Common.Models;
using TuneVault.Application.Features.Playlists.Dtos;
using TuneVault.Application.Interfaces.Persistence;

namespace TuneVault.Application.Features.MediaItems.Queries.SearchMedia;

public class SearchMediaQueryHandler : IRequestHandler<SearchMediaQuery, PagedResult<MediaItemSummaryDto>>
{
    private readonly IMediaRepository _mediaRepository;

    public SearchMediaQueryHandler(IMediaRepository mediaRepository)
    {
        _mediaRepository = mediaRepository;
    }

    public async Task<PagedResult<MediaItemSummaryDto>> Handle(SearchMediaQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var pageSize = request.PageSize <= 0 ? 20 : (request.PageSize > 50 ? 50 : request.PageSize);

        var (items, totalCount) = await _mediaRepository.SearchAsync(
            request.Term,
            request.FileType,
            pageNumber,
            pageSize,
            cancellationToken);

        var mappedItems = items.Select(m => new MediaItemSummaryDto
        {
            Id = m.Id,
            Title = m.Title,
            Artist = m.Artist,
            Duration = m.Duration,
            FileType = m.FileType.ToString(),
            StreamUrl = $"/api/media/{m.Id}/stream"
        }).ToList();

        return new PagedResult<MediaItemSummaryDto>(mappedItems, totalCount, pageNumber, pageSize);
    }
}
