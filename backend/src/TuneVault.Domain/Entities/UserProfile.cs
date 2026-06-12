namespace TuneVault.Domain.Entities;

public class UserProfile
{
    public Guid UserId { get; set; }
    public ApplicationUser? User { get; set; }

    public string DisplayName { get; set; } = string.Empty;

    public string Bio { get; set; } = string.Empty;
    public string? AvatarPath { get; set; }
}
