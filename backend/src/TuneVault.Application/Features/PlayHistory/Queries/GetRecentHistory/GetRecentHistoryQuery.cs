using MediatR;
using TuneVault.Application.Features.PlayHistory.Dtos;

namespace TuneVault.Application.Features.PlayHistory.Queries.GetRecentHistory;

public record GetRecentHistoryQuery(Guid UserId) : IRequest<List<PlayHistoryDto>>;
