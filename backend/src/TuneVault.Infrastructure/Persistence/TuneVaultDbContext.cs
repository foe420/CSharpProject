using Microsoft.EntityFrameworkCore;
using TuneVault.Domain.Entities;

namespace TuneVault.Infrastructure.Persistence;

public class TuneVaultDbContext : DbContext
{
    public TuneVaultDbContext(DbContextOptions<TuneVaultDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<MediaItem> MediaItems => Set<MediaItem>();
    public DbSet<Playlist> Playlists => Set<Playlist>();
    public DbSet<PlaylistMedia> PlaylistMediaItems => Set<PlaylistMedia>();

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

        modelBuilder.Entity<MediaItem>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.MediaUrl).HasMaxLength(1000).IsRequired();
            entity.Property(x => x.ThumbnailUrl).HasMaxLength(1000).IsRequired();

            entity.HasOne(x => x.UploadedBy)
                .WithMany(x => x.UploadedMediaItems)
                .HasForeignKey(x => x.UploadedById)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();

            entity.HasOne(x => x.Owner)
                .WithMany(x => x.Playlists)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PlaylistMedia>(entity =>
        {
            entity.HasKey(x => new { x.PlaylistId, x.MediaItemId });

            entity.HasOne(x => x.Playlist)
                .WithMany(x => x.PlaylistMediaItems)
                .HasForeignKey(x => x.PlaylistId);

            entity.HasOne(x => x.MediaItem)
                .WithMany(x => x.PlaylistMediaItems)
                .HasForeignKey(x => x.MediaItemId);
        });
    }
}
