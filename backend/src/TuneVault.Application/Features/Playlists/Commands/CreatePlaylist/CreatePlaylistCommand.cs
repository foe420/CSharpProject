using MediatR;
using TuneVault.Application.Features.Playlists.Dtos;

namespace TuneVault.Application.Features.Playlists.Commands.CreatePlaylist;

public class CreatePlaylistCommand : IRequest<PlaylistDto>
{
    public string Title { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public Guid OwnerId { get; set; }
}
