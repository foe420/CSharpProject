using MediatR;
using TuneVault.Application.Features.Playlists.Dtos;

namespace TuneVault.Application.Features.Playlists.Queries.GetUserPlaylists;

public class GetUserPlaylistsQuery : IRequest<IReadOnlyList<PlaylistDto>>
{
    public Guid UserId { get; set; }
}