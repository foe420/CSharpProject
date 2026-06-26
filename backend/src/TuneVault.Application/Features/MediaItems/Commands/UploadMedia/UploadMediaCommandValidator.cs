using FluentValidation;

namespace TuneVault.Application
.Features.MediaItems.Commands.UploadMedia;

public class UploadMediaCommandValidator
    : AbstractValidator<UploadMediaCommand>
{
    public UploadMediaCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull()
            .Must(file => file == null || file.Length <= 50 * 1024 * 1024)
            .WithMessage("File size must not exceed 50 MB.");

        RuleFor(x => x.Title)
            .NotEmpty();

        RuleFor(x => x.Artist)
            .NotEmpty();
    }
}