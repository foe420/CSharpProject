using MediatR;
using TuneVault.Application.Features.Playlists.Dtos;

namespace TuneVault.Application.Features.Playlists.Queries.GetPlaylistById;

public record GetPlaylistByIdQuery(Guid Id, Guid? CurrentUserId) : IRequest<PlaylistDetailDto>;
