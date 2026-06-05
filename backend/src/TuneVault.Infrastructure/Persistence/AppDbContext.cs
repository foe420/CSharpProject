using Microsoft.EntityFrameworkCore;
using TuneVault.Domain.Entities;

namespace TuneVault.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<MediaItem> MediaItems => Set<MediaItem>();
    public DbSet<Playlist> Playlists => Set<Playlist>();
    public DbSet<PlaylistTrack> PlaylistTracks => Set<PlaylistTrack>();
    public DbSet<MediaShare> MediaShares => Set<MediaShare>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<PlayHistory> PlayHistories => Set<PlayHistory>();
    public DbSet<Follow> Follows => Set<Follow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.UserName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(256).IsRequired();
            entity.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(x => x.UserId);
            entity.Property(x => x.Bio).HasMaxLength(500).IsRequired();

            entity.HasOne(x => x.User)
                .WithOne(x => x.Profile)
                .HasForeignKey<UserProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MediaItem>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Artist).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Genre).HasMaxLength(100);
            entity.Property(x => x.FilePath).IsRequired();
            entity.Property(x => x.FileType).IsRequired();
            entity.Property(x => x.Duration).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(1000);
            entity.Property(x => x.CreatedAt).IsRequired();

            entity.HasOne(x => x.Owner)
                .WithMany(x => x.OwnedMediaItems)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.IsPublic).HasDefaultValue(false);
            entity.Property(x => x.CreatedAt).IsRequired();

            entity.HasOne(x => x.Owner)
                .WithMany(x => x.Playlists)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PlaylistTrack>(entity =>
        {
            entity.HasKey(x => new { x.PlaylistId, x.MediaItemId });
            entity.Property(x => x.Position).IsRequired();

            entity.HasOne(x => x.Playlist)
                .WithMany(x => x.Tracks)
                .HasForeignKey(x => x.PlaylistId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.MediaItem)
                .WithMany(x => x.PlaylistTracks)
                .HasForeignKey(x => x.MediaItemId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<MediaShare>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.SharedAt).IsRequired();

            entity.HasOne(x => x.Sender)
                .WithMany(x => x.SentShares)
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Receiver)
                .WithMany(x => x.ReceivedShares)
                .HasForeignKey(x => x.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.MediaItem)
                .WithMany(x => x.MediaShares)
                .HasForeignKey(x => x.MediaItemId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Playlist)
                .WithMany(x => x.MediaShares)
                .HasForeignKey(x => x.PlaylistId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Type).HasMaxLength(50).IsRequired();
            entity.Property(x => x.PayloadJson).IsRequired();
            entity.Property(x => x.IsRead).HasDefaultValue(false);
            entity.Property(x => x.CreatedAt).IsRequired();

            entity.HasOne(x => x.User)
                .WithMany(x => x.Notifications)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(x => new { x.UserId, x.MediaItemId });
            entity.Property(x => x.CreatedAt).IsRequired();

            entity.HasOne(x => x.User)
                .WithMany(x => x.Favorites)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.MediaItem)
                .WithMany(x => x.Favorites)
                .HasForeignKey(x => x.MediaItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PlayHistory>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.PlayedAt).IsRequired();

            entity.HasOne(x => x.User)
                .WithMany(x => x.PlayHistoryEntries)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.MediaItem)
                .WithMany(x => x.PlayHistoryEntries)
                .HasForeignKey(x => x.MediaItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Follow>(entity =>
        {
            entity.HasKey(x => new { x.FollowerId, x.FolloweeId });
            entity.Property(x => x.FollowedAt).IsRequired();

            entity.HasOne(x => x.Follower)
                .WithMany(x => x.Following)
                .HasForeignKey(x => x.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Followee)
                .WithMany(x => x.Followers)
                .HasForeignKey(x => x.FolloweeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
