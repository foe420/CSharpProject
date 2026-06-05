using Microsoft.EntityFrameworkCore;
using TuneVault.Domain.Entities;
using TuneVault.Domain.Enums;

namespace TuneVault.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
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
            Artist = "TuneVault",
            Genre = "Demo",
            Description = "Seeded audio content.",
            FilePath = "https://cdn.example.com/audio/starter-track.mp3",
            Duration = 182,
            FileType = MediaFileType.Audio,
            Owner = user
        };

        var playlist = new Playlist
        {
            Title = "Getting Started",
            Owner = user,
            IsPublic = true,
            Tracks =
            [
                new PlaylistTrack
                {
                    MediaItem = mediaItem,
                    Position = 1
                }
            ]
        };

        dbContext.Users.Add(user);
        dbContext.MediaItems.Add(mediaItem);
        dbContext.Playlists.Add(playlist);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
