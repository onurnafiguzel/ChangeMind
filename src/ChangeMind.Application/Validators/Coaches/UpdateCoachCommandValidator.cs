namespace ChangeMind.Application.Validators.Coaches;

using ChangeMind.Application.UseCases.Coaches.Commands;
using FluentValidation;

public sealed class UpdateCoachCommandValidator : AbstractValidator<UpdateCoachCommand>
{
    public UpdateCoachCommandValidator()
    {
        RuleFor(x => x.CoachId)
            .NotEmpty()
                .WithMessage("Koç kimliği geçersiz.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
                .WithMessage("Ad boş olamaz.")
            .MaximumLength(100)
                .WithMessage("Ad en fazla 100 karakter olabilir.");

        RuleFor(x => x.LastName)
            .NotEmpty()
                .WithMessage("Soyad boş olamaz.")
            .MaximumLength(100)
                .WithMessage("Soyad en fazla 100 karakter olabilir.");
    }
}
