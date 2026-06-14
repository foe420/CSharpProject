using Microsoft.AspNetCore.Http;
using TuneVault.Application.Interfaces.Services;

namespace TuneVault.Infrastructure.Services;

public class LocalFileStorageService
    : IFileStorageService
{
    public async Task<string> SaveAsync(
        IFormFile file,
        CancellationToken cancellationToken)
    {
        var extension =
            Path.GetExtension(file.FileName)
                .ToLowerInvariant();

        var allowed =
            new[]
            {
                ".mp3",
                ".wav",
                ".mp4",
                ".webm"
            };

        if (!allowed.Contains(extension))
        {
            throw new Exception(
                "Unsupported file type");
        }

        string folder =
            extension == ".mp3" ||
            extension == ".wav"
                ? "Audio"
                : "Video";

        var fileName =
            $"{Guid.NewGuid()}{extension}";

        var uploadPath =
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "Uploads",
                folder);

        Directory.CreateDirectory(uploadPath);

        var fullPath =
            Path.Combine(
                uploadPath,
                fileName);

        using var stream =
            new FileStream(
                fullPath,
                FileMode.Create);

        await file.CopyToAsync(
            stream,
            cancellationToken);

        return fullPath;
    }
}