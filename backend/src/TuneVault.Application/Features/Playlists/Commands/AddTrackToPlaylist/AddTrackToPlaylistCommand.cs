using MediatR;
using TuneVault.Application.Features.Playlists.Dtos;

namespace TuneVault.Application.Features.Playlists.Commands.AddTrackToPlaylist;

public class AddTrackToPlaylistCommand : IRequest<PlaylistDto>
{
    public Guid PlaylistId { get; set; }
    public Guid MediaItemId { get; set; }
    public Guid OwnerId { get; set; }
}
