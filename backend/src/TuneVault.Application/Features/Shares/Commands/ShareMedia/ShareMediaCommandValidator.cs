using FluentValidation;

namespace TuneVault.Application.Features.Shares.Commands.ShareMedia;

public class ShareMediaCommandValidator : AbstractValidator<ShareMediaCommand>
{
    public ShareMediaCommandValidator()
    {
        RuleFor(x => x.ReceiverId)
            .NotEmpty().WithMessage("ReceiverId is required.");

        RuleFor(x => x)
            .Must(x => (x.MediaItemId.HasValue && !x.PlaylistId.HasValue) || (!x.MediaItemId.HasValue && x.PlaylistId.HasValue))
            .WithMessage("Exactly one of MediaItemId or PlaylistId must be set.");

        RuleFor(x => x)
            .Must(x => x.SenderId != x.ReceiverId)
            .WithMessage("Cannot share with yourself");
    }
}
