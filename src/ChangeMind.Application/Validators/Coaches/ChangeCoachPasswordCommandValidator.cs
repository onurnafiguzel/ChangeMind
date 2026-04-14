namespace ChangeMind.Application.Validators.Coaches;

using ChangeMind.Application.UseCases.Coaches.Commands;
using ChangeMind.Application.Validators.Rules;
using FluentValidation;

public sealed class ChangeCoachPasswordCommandValidator : AbstractValidator<ChangeCoachPasswordCommand>
{
    public ChangeCoachPasswordCommandValidator()
    {
        RuleFor(x => x.CoachId)
            .NotEmpty()
                .WithMessage("Koç kimliği geçersiz.");

        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
                .WithMessage("Mevcut şifre boş olamaz.");

        RuleFor(x => x.NewPassword)
            .StrongPassword();

        RuleFor(x => x)
            .Must(x => x.CurrentPassword != x.NewPassword)
                .WithMessage("Yeni şifre mevcut şifreyle aynı olamaz.")
            .When(x => !string.IsNullOrEmpty(x.CurrentPassword) && !string.IsNullOrEmpty(x.NewPassword));
    }
}
