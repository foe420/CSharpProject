namespace TuneVault.Application.Features.Profiles.Dtos;

public class UserProfileDto
{
    public Guid UserId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string? AvatarPath { get; set; }
}
