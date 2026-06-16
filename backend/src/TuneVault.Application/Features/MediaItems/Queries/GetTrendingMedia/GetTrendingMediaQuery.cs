using MediatR;
using TuneVault.Application.Features.Playlists.Dtos;

namespace TuneVault.Application.Features.MediaItems.Queries.GetTrendingMedia;

public record GetTrendingMediaQuery : IRequest<List<MediaItemSummaryDto>>;
