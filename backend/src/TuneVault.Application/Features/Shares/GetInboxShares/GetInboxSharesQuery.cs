using MediatR;

namespace TuneVault.Application.Features.Shares.GetInboxShares;

public class GetInboxSharesQuery : IRequest<IReadOnlyList<ShareDto>>
{
    public Guid UserId { get; set; }
}