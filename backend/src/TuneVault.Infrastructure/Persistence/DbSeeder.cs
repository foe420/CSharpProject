using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TuneVault.Domain.Entities;
using TuneVault.Domain.Enums;

namespace TuneVault.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(
        AppDbContext dbContext,
        IPasswordHasher<ApplicationUser>? passwordHasher = null,
        CancellationToken cancellationToken = default)
    {
        Console.WriteLine("=== [DbSeeder] Starting Database Seeding ===");
        await dbContext.Database.MigrateAsync(cancellationToken);

        // 1. Starter User (Domain Model only)
        var starterUser = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == "starteruser", cancellationToken);
        if (starterUser == null)
        {
            Console.WriteLine("-> Seeding starteruser...");
            starterUser = new User
            {
                UserName = "starteruser",
                Email = "starter@tunevault.local",
                PasswordHash = "starter-password-hash"
            };
            dbContext.Users.Add(starterUser);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            Console.WriteLine("-> starteruser already exists.");
        }

        // 2. Test User with proper Login credentials (ApplicationUser + User + UserProfile)
        var testUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var testAppUser = await dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == "testuser@tunevault.local", cancellationToken);
        if (testAppUser == null)
        {
            Console.WriteLine("-> Seeding testuser@tunevault.local...");
            testAppUser = new ApplicationUser
            {
                Id = testUserId,
                UserName = "testuser@tunevault.local",
                Email = "testuser@tunevault.local",
                NormalizedUserName = "TESTUSER@TUNEVAULT.LOCAL",
                NormalizedEmail = "TESTUSER@TUNEVAULT.LOCAL",
                Role = "User",
                DisplayName = "Test User",
                CreatedAtUtc = DateTime.UtcNow
            };

            if (passwordHasher != null)
            {
                testAppUser.PasswordHash = passwordHasher.HashPassword(testAppUser, "Password123!");
            }
            else
            {
                testAppUser.PasswordHash = "Password123!Hash";
            }

            dbContext.ApplicationUsers.Add(testAppUser);

            var testDomainUser = new User
            {
                Id = testUserId,
                UserName = "testuser@tunevault.local",
                Email = "testuser@tunevault.local",
                PasswordHash = testAppUser.PasswordHash,
                CreatedAtUtc = testAppUser.CreatedAtUtc
            };
            dbContext.Users.Add(testDomainUser);

            var testProfile = new UserProfile
            {
                UserId = testUserId,
                DisplayName = "Test User",
                Bio = "Seeded account for testing playlists."
            };
            dbContext.UserProfiles.Add(testProfile);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            Console.WriteLine("-> testuser@tunevault.local already exists.");
        }

        // 3. Seed MediaItems
        var starterTrack = await dbContext.MediaItems.FirstOrDefaultAsync(m => m.Title == "Starter Track", cancellationToken);
        if (starterTrack == null)
        {
            Console.WriteLine("-> Seeding 'Starter Track'...");
            starterTrack = new MediaItem
            {
                Title = "Starter Track",
                Artist = "TuneVault",
                Genre = "Demo",
                Description = "Seeded audio content.",
                FilePath = "https://cdn.example.com/audio/starter-track.mp3",
                Duration = 182,
                FileType = MediaFileType.Audio,
                OwnerId = starterUser.Id
            };
            dbContext.MediaItems.Add(starterTrack);
        }
        else
        {
            Console.WriteLine("-> 'Starter Track' already exists.");
        }

        var lofiTrack = await dbContext.MediaItems.FirstOrDefaultAsync(m => m.Title == "Lo-Fi Chill", cancellationToken);
        if (lofiTrack == null)
        {
            Console.WriteLine("-> Seeding 'Lo-Fi Chill'...");
            lofiTrack = new MediaItem
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Title = "Lo-Fi Chill",
                Artist = "Chillhop Artist",
                Genre = "Lo-Fi",
                Description = "Relaxing lo-fi beats.",
                FilePath = "https://cdn.example.com/audio/lofi-chill.mp3",
                Duration = 150,
                FileType = MediaFileType.Audio,
                OwnerId = testUserId
            };
            dbContext.MediaItems.Add(lofiTrack);
        }
        else
        {
            Console.WriteLine("-> 'Lo-Fi Chill' already exists.");
        }

        var synthTrack = await dbContext.MediaItems.FirstOrDefaultAsync(m => m.Title == "Synthwave Ride", cancellationToken);
        if (synthTrack == null)
        {
            Console.WriteLine("-> Seeding 'Synthwave Ride'...");
            synthTrack = new MediaItem
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Title = "Synthwave Ride",
                Artist = "Retro Synth",
                Genre = "Synthwave",
                Description = "Driving synthwave rhythms.",
                FilePath = "https://cdn.example.com/audio/synthwave-ride.mp3",
                Duration = 210,
                FileType = MediaFileType.Audio,
                OwnerId = testUserId
            };
            dbContext.MediaItems.Add(synthTrack);
        }
        else
        {
            Console.WriteLine("-> 'Synthwave Ride' already exists.");
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        // 4. Seed Playlists
        var publicPlaylist = await dbContext.Playlists.FirstOrDefaultAsync(p => p.Title == "Test Public Playlist", cancellationToken);
        if (publicPlaylist == null)
        {
            Console.WriteLine("-> Seeding 'Test Public Playlist'...");
            publicPlaylist = new Playlist
            {
                Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                Title = "Test Public Playlist",
                OwnerId = testUserId,
                IsPublic = true,
                Tracks = new List<PlaylistTrack>
                {
                    new PlaylistTrack
                    {
                        MediaItemId = lofiTrack.Id,
                        Position = 1
                    }
                }
            };
            dbContext.Playlists.Add(publicPlaylist);
        }
        else
        {
            Console.WriteLine("-> 'Test Public Playlist' already exists.");
        }

        var privatePlaylist = await dbContext.Playlists.FirstOrDefaultAsync(p => p.Title == "Test Private Playlist", cancellationToken);
        if (privatePlaylist == null)
        {
            Console.WriteLine("-> Seeding 'Test Private Playlist'...");
            privatePlaylist = new Playlist
            {
                Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                Title = "Test Private Playlist",
                OwnerId = testUserId,
                IsPublic = false,
                Tracks = new List<PlaylistTrack>
                {
                    new PlaylistTrack
                    {
                        MediaItemId = synthTrack.Id,
                        Position = 1
                    }
                }
            };
            dbContext.Playlists.Add(privatePlaylist);
        }
        else
        {
            Console.WriteLine("-> 'Test Private Playlist' already exists.");
        }

        var starterPlaylist = await dbContext.Playlists.FirstOrDefaultAsync(p => p.Title == "Getting Started", cancellationToken);
        if (starterPlaylist == null)
        {
            Console.WriteLine("-> Seeding 'Getting Started' playlist...");
            starterPlaylist = new Playlist
            {
                Title = "Getting Started",
                OwnerId = starterUser.Id,
                IsPublic = true,
                Tracks = new List<PlaylistTrack>
                {
                    new PlaylistTrack
                    {
                        MediaItemId = starterTrack.Id,
                        Position = 1
                    }
                }
            };
            dbContext.Playlists.Add(starterPlaylist);
        }
        else
        {
            Console.WriteLine("-> 'Getting Started' playlist already exists.");
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        Console.WriteLine("=== [DbSeeder] Database Seeding Completed ===");
    }
}
