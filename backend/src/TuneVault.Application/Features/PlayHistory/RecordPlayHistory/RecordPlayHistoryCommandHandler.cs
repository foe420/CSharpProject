using MediatR;
using Microsoft.Extensions.Logging;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;

namespace TuneVault.Application.Features.PlayHistory.RecordPlayHistory;

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
            var history = new TuneVault.Domain.Entities.PlayHistory  
            {
                UserId = request.UserId,
                MediaItemId = request.MediaItemId,
                PlayedAt = DateTime.UtcNow
            };

            await _playHistoryRepository.AddAsync(history, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to record play history for MediaItem {MediaItemId}, User {UserId}",
                request.MediaItemId, request.UserId);
        }

        return Unit.Value;
    }
}