using MediatR;
using Microsoft.Extensions.Logging;
using TuneVault.Application.Interfaces.Persistence;
using TuneVault.Domain.Entities;

namespace TuneVault.Application.Features.Shares.CreateShare;

public class CreateShareCommandHandler : IRequestHandler<CreateShareCommand, ShareResponseDto>
{
    private readonly IShareRepository _shareRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<CreateShareCommandHandler> _logger;

    public CreateShareCommandHandler(
        IShareRepository shareRepository,
        IUserRepository userRepository,
        ILogger<CreateShareCommandHandler> logger)
    {
        _shareRepository = shareRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<ShareResponseDto> Handle(CreateShareCommand request, CancellationToken cancellationToken)
    {
        // Kiểm tra người nhận tồn tại
        var receiver = await _userRepository.GetByEmailAsync(request.ReceiverEmail, cancellationToken);
        if (receiver == null)
        {
            throw new InvalidOperationException($"User with email '{request.ReceiverEmail}' not found");
        }

        // Kiểm tra không share với chính mình
        if (receiver.Id == request.SenderId)
        {
            throw new InvalidOperationException("Cannot share with yourself");
        }

        // Kiểm tra phải có MediaItemId hoặc PlaylistId
        if (!request.MediaItemId.HasValue && !request.PlaylistId.HasValue)
        {
            throw new InvalidOperationException("Either MediaItemId or PlaylistId must be provided");
        }

        var share = new MediaShare
        {
            Id = Guid.NewGuid(),
            SenderId = request.SenderId,
            ReceiverId = receiver.Id,
            MediaItemId = request.MediaItemId,
            PlaylistId = request.PlaylistId,
            SharedAt = DateTime.UtcNow
        };

        var created = await _shareRepository.AddAsync(share, cancellationToken);

        // Lấy thông tin sender
        var sender = await _userRepository.GetByIdAsync(request.SenderId, cancellationToken);

        return new ShareResponseDto
        {
            Id = created.Id,
            SenderName = sender?.UserName ?? "Unknown",
            ReceiverName = receiver.UserName,
            SharedAt = created.SharedAt,
            MediaItemId = created.MediaItemId,
            PlaylistId = created.PlaylistId
        };
    }
}