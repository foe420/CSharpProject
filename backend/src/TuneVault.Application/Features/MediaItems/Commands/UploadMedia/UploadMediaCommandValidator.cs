using FluentValidation;

namespace TuneVault.Application
.Features.MediaItems.Commands.UploadMedia;

public class UploadMediaCommandValidator
    : AbstractValidator<UploadMediaCommand>
{
    public UploadMediaCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull();

        RuleFor(x => x.Title)
            .NotEmpty();

        RuleFor(x => x.Artist)
            .NotEmpty();
    }
}