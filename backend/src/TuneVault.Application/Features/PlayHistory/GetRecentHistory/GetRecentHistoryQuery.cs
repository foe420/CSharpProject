using MediatR;

namespace TuneVault.Application.Features.PlayHistory.GetRecentHistory;

public class GetRecentHistoryQuery : IRequest<IReadOnlyList<HistoryItemDto>>
{
    public Guid UserId { get; set; }
}