using MediatR;
using TuneVault.Application.Common.Models;
using TuneVault.Application.Features.Playlists.Dtos;
using TuneVault.Domain.Enums;

namespace TuneVault.Application.Features.MediaItems.Queries.SearchMedia;

public record SearchMediaQuery(
    string? Term,
    MediaFileType? FileType,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<PagedResult<MediaItemSummaryDto>>;
