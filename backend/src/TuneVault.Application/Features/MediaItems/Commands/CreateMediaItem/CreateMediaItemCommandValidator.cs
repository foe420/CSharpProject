using FluentValidation;

namespace TuneVault.Application.Features.MediaItems.Commands.CreateMediaItem;

public class CreateMediaItemCommandValidator : AbstractValidator<CreateMediaItemCommand>
{
    public CreateMediaItemCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.MediaUrl).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.ThumbnailUrl).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.DurationSeconds).GreaterThan(0);
    }
}
