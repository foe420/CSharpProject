using MediatR;
using Microsoft.Extensions.Logging;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;

namespace TuneVault.Application.Features.PlayHistory.Commands.RecordPlayHistory;

public class RecordPlayHistoryCommandHandler : IRequestHandler<RecordPlayHistoryCommand, Unit>
{
    private readonly IPlayHistoryRepository _playHistoryRepository;
    private readonly ILogger<RecordPlayHistoryCommandHandler> _logger;

    public RecordPlayHistoryCommandHandler(
        IPlayHistoryRepository playHistoryRepository,
        ILogger<RecordPlayHistoryCommandHandler> logger)
    {
        _playHistoryRepository = playHistoryRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(RecordPlayHistoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var playHistory = new Domain.Entities.PlayHistory
            {
                UserId = request.UserId,
                MediaItemId = request.MediaItemId,
                PlayedAt = DateTime.UtcNow
            };

            await _playHistoryRepository.AddAsync(playHistory, cancellationToken);
        }
        catch (Exception ex)
        {
            // Fire-and-forget: never throws, log on error only
            _logger.LogError(ex, "Failed to record play history for UserId={UserId}, MediaItemId={MediaItemId}",
                request.UserId, request.MediaItemId);
        }
        return Unit.Value;
    }
}
