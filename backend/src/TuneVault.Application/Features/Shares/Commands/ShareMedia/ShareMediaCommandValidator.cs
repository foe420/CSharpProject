using FluentValidation;

namespace TuneVault.Application.Features.Shares.Commands.ShareMedia;

public class ShareMediaCommandValidator : AbstractValidator<ShareMediaCommand>
{
    public ShareMediaCommandValidator()
    {
        RuleFor(x => x)
            .Must(x => x.ReceiverId != Guid.Empty || !string.IsNullOrWhiteSpace(x.ReceiverEmail))
            .WithMessage("Either ReceiverId or ReceiverEmail is required.");

        RuleFor(x => x)
            .Must(x => (x.MediaItemId.HasValue && !x.PlaylistId.HasValue) || (!x.MediaItemId.HasValue && x.PlaylistId.HasValue))
            .WithMessage("Exactly one of MediaItemId or PlaylistId must be set.");
    }
}
