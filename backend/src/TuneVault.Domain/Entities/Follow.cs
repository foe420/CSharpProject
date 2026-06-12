namespace TuneVault.Domain.Entities;

public class Follow
{
    public Guid FollowerId { get; set; }
    public User? Follower { get; set; }

    public Guid FolloweeId { get; set; }
    public User? Followee { get; set; }

    public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
}
