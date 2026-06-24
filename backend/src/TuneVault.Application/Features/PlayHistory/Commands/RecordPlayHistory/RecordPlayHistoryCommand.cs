using MediatR;

namespace TuneVault.Application.Features.PlayHistory.Commands.RecordPlayHistory;

public record RecordPlayHistoryCommand(Guid UserId, Guid MediaItemId) : IRequest<Unit>;
