namespace ChangeMind.Application.Validators.Users;

using ChangeMind.Application.UseCases.Users.Commands;
using ChangeMind.Application.Validators.Rules;
using FluentValidation;

public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
                .WithMessage("Kullanıcı kimliği geçersiz.");

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
