using MediatR;
using TuneVault.Application.Features.Shares.GetInboxShares;  

namespace TuneVault.Application.Features.Shares.GetSentShares;

public class GetSentSharesQuery : IRequest<IReadOnlyList<ShareDto>>  
{
    public Guid UserId { get; set; }
}