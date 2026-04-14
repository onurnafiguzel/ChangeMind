namespace ChangeMind.Application.Validators.Auth;

using ChangeMind.Application.UseCases.Auth.Commands;
using ChangeMind.Application.Validators.Rules;
using FluentValidation;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .ValidEmail();

        RuleFor(x => x.Password)
            .NotEmpty()
                .WithMessage("Şifre boş olamaz.");
    }
}
