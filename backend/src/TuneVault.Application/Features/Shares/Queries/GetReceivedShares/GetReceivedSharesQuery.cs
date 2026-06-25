using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TuneVault.Application.Common.Models;
using TuneVault.Application.Features.Playlists.Dtos;
using TuneVault.Application.Features.Shares.Dtos;
using TuneVault.Application.Interfaces.Persistence;

namespace TuneVault.Application.Features.Shares.Queries.GetReceivedShares;

public record GetReceivedSharesQuery(
    Guid ReceiverId,
    int PageNumber,
    int PageSize
) : IRequest<PagedResult<ShareDto>>;

public class GetReceivedSharesQueryHandler : IRequestHandler<GetReceivedSharesQuery, PagedResult<ShareDto>>
{
    private readonly IMediaShareRepository _mediaShareRepository;

    public GetReceivedSharesQueryHandler(IMediaShareRepository mediaShareRepository)
    {
        _mediaShareRepository = mediaShareRepository;
    }

    public async Task<PagedResult<ShareDto>> Handle(GetReceivedSharesQuery request, CancellationToken cancellationToken)
    {
        var page = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var size = request.PageSize <= 0 ? 20 : (request.PageSize > 50 ? 50 : request.PageSize);

        var (shares, totalCount) = await _mediaShareRepository.GetReceivedSharesPaginatedAsync(
            request.ReceiverId,
            page,
            size,
            cancellationToken);

        var mappedShares = shares.Select(s => new ShareDto
        {
            Id = s.Id,
            SenderName = s.Sender != null 
                ? (s.Sender.Profile?.DisplayName ?? s.Sender.UserName) 
                : "Someone",
            ReceiverName = s.Receiver != null 
                ? (s.Receiver.Profile?.DisplayName ?? s.Receiver.UserName) 
                : "Me",
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
