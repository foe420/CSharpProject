using FluentValidation;

namespace TuneVault.Application.Features.Profiles.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileCommandValidator()
    {
        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Display name is required.")
            .MaximumLength(100).WithMessage("Display name must not exceed 100 characters.");

        RuleFor(x => x.Bio)
            .NotNull().WithMessage("Bio must not be null.")
            .MaximumLength(500).WithMessage("Bio must not exceed 500 characters.");
    }
}
