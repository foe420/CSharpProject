using MediatR;

namespace TuneVault.Application.Features.PlayHistory.RecordPlayHistory;

public class RecordPlayHistoryCommand : IRequest
{
    public Guid MediaItemId { get; init; }
    public Guid UserId { get; set; }
}