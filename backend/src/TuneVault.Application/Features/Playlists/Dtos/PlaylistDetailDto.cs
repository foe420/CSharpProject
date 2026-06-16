namespace TuneVault.Application.Features.Playlists.Dtos;

public class PlaylistDetailDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public List<MediaItemSummaryDto> Tracks { get; set; } = new();
}
