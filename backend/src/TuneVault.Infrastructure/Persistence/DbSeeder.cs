using Microsoft.EntityFrameworkCore;
using TuneVault.Domain.Entities;
using TuneVault.Domain.Enums;

namespace TuneVault.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(TuneVaultDbContext dbContext, CancellationToken cancellationToken = default)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);

        if (await dbContext.Users.AnyAsync(cancellationToken))
        {
            return;
        }

        var user = new User
        {
            UserName = "starteruser",
            Email = "starter@tunevault.local",
            PasswordHash = "starter-password-hash"
        };

        var mediaItem = new MediaItem
        {
            Title = "Starter Track",
            Description = "Seeded audio content.",
            MediaUrl = "https://cdn.example.com/audio/starter-track.mp3",
            ThumbnailUrl = "https://cdn.example.com/images/starter-track.jpg",
            DurationSeconds = 182,
            MediaType = MediaType.Audio,
            UploadedBy = user
        };

        var playlist = new Playlist
        {
            Name = "Getting Started",
            Owner = user,
            IsPublic = true,
            PlaylistMediaItems =
            [
                new PlaylistMedia
                {
                    MediaItem = mediaItem,
                    Order = 1
                }
            ]
        };

        dbContext.Users.Add(user);
        dbContext.MediaItems.Add(mediaItem);
        dbContext.Playlists.Add(playlist);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
