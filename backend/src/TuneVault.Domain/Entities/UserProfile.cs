namespace TuneVault.Domain.Entities;

public class UserProfile
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public string Bio { get; set; } = string.Empty;
    public string? AvatarPath { get; set; }
}
