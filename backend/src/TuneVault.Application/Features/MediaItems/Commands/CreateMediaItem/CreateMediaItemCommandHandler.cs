using MediatR;
using TuneVault.Application.Features.MediaItems.Queries.GetMediaItems;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;

namespace TuneVault.Application.Features.MediaItems.Commands.CreateMediaItem;

public class CreateMediaItemCommandHandler : IRequestHandler<CreateMediaItemCommand, MediaItemDto>
{
    private readonly IMediaRepository _mediaRepository;

    public CreateMediaItemCommandHandler(IMediaRepository mediaRepository)
    {
        _mediaRepository = mediaRepository;
    }

    public async Task<MediaItemDto> Handle(CreateMediaItemCommand request, CancellationToken cancellationToken)
    {
        var mediaItem = new MediaItem
        {
            Title = request.Title,
            Artist = request.Artist,
            Genre = request.Genre,
            FilePath = request.FilePath,
            FileType = request.FileType,
            Duration = request.Duration,
            Description = request.Description
        };

        var created = await _mediaRepository.AddAsync(mediaItem, cancellationToken);

        return new MediaItemDto
        {
            Id = created.Id,
            Title = created.Title,
            Artist = created.Artist,
            Genre = created.Genre,
            FilePath = created.FilePath,
            FileType = created.FileType,
            Duration = created.Duration,
            Description = created.Description,
            CreatedAt = created.CreatedAt
        };
    }
}
