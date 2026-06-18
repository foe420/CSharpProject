using FluentValidation;

namespace TuneVault.Application.Features.Shares.CreateShare;

public class CreateShareCommandValidator : AbstractValidator<CreateShareCommand>
{
    public CreateShareCommandValidator()
    {
        RuleFor(x => x.ReceiverEmail)
            .NotEmpty().WithMessage("Receiver email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x)
            .Must(x => x.MediaItemId.HasValue || x.PlaylistId.HasValue)
            .WithMessage("Either MediaItemId or PlaylistId must be provided");

        RuleFor(x => x)
            .Must(x => !(x.MediaItemId.HasValue && x.PlaylistId.HasValue))
            .WithMessage("Cannot share both MediaItem and Playlist at the same time");
    }
}