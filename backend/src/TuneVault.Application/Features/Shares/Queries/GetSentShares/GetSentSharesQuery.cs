using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TuneVault.Application.Common.Models;
using TuneVault.Application.Features.Playlists.Dtos;
using TuneVault.Application.Features.Shares.Dtos;
using TuneVault.Application.Interfaces.Persistence;

namespace TuneVault.Application.Features.Shares.Queries.GetSentShares;

public record GetSentSharesQuery(
    Guid SenderId,
    int PageNumber,
    int PageSize
) : IRequest<PagedResult<ShareDto>>;

public class GetSentSharesQueryHandler : IRequestHandler<GetSentSharesQuery, PagedResult<ShareDto>>
{
    private readonly IMediaShareRepository _mediaShareRepository;

    public GetSentSharesQueryHandler(IMediaShareRepository mediaShareRepository)
    {
        _mediaShareRepository = mediaShareRepository;
    }

    public async Task<PagedResult<ShareDto>> Handle(GetSentSharesQuery request, CancellationToken cancellationToken)
    {
        var page = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var size = request.PageSize <= 0 ? 20 : (request.PageSize > 50 ? 50 : request.PageSize);

        var (shares, totalCount) = await _mediaShareRepository.GetSentSharesPaginatedAsync(
            request.SenderId,
            page,
            size,
            cancellationToken);

        var mappedShares = shares.Select(s => new ShareDto
        {
            Id = s.Id,
            SenderName = s.Sender != null 
                ? (s.Sender.Profile?.DisplayName ?? s.Sender.UserName) 
                : "Me",
            ReceiverName = s.Receiver != null 
                ? (s.Receiver.Profile?.DisplayName ?? s.Receiver.UserName) 
                : "Someone",
            SharedAt = s.SharedAt,
            PlaylistId = s.PlaylistId,
            PlaylistTitle = s.Playlist?.Title,
            MediaItem = s.MediaItem != null ? new MediaItemSummaryDto
            {
                Id = s.MediaItem.Id,
                Title = s.MediaItem.Title,
                Artist = s.MediaItem.Artist,
                Duration = s.MediaItem.Duration,
                FileType = s.MediaItem.FileType.ToString(),
                StreamUrl = $"/api/media/{s.MediaItem.Id}/stream"
            } : null
        }).ToList();

        return new PagedResult<ShareDto>(mappedShares, totalCount, page, size);
    }
}
