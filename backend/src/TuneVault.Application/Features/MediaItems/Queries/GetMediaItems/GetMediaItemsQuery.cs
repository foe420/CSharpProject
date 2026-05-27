using MediatR;

namespace TuneVault.Application.Features.MediaItems.Queries.GetMediaItems;

public class GetMediaItemsQuery : IRequest<IReadOnlyList<MediaItemDto>>
{
}
