namespace TuneVault.Application.Features.Playlists.Dtos;

public class PlaylistDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public int TrackCount { get; set; }
}
