using MediatR;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Application.Interfaces.Services;
using TuneVault.Domain.Entities;
using TuneVault.Domain.Enums;

namespace TuneVault.Application
.Features.MediaItems.Commands.UploadMedia;

public class UploadMediaCommandHandler
    : IRequestHandler<
        UploadMediaCommand,
        Guid>
{
    private readonly IMediaRepository
        _mediaRepository;

    private readonly IFileStorageService
        _fileStorage;

    public UploadMediaCommandHandler(
        IMediaRepository mediaRepository,
        IFileStorageService fileStorage)
    {
        _mediaRepository = mediaRepository;
        _fileStorage = fileStorage;
    }

    public async Task<Guid> Handle(
        UploadMediaCommand request,
        CancellationToken cancellationToken)
    {
        var savedPath =
            await _fileStorage.SaveAsync(
                request.File,
                cancellationToken);

        var extension =
            Path.GetExtension(
                request.File.FileName)
            .ToLowerInvariant();

        var mediaItem = new MediaItem
        {
            OwnerId = request.OwnerId,

            Title = request.Title,

            Artist = request.Artist,

            Genre = request.Genre,

            Description = request.Description,

            FilePath = savedPath,

            FileType =
                extension == ".mp4" ||
                extension == ".webm"
                    ? MediaFileType.Video
                    : MediaFileType.Audio
        };

        var created =
            await _mediaRepository
                .AddAsync(
                    mediaItem,
                    cancellationToken);

        return created.Id;
    }
}