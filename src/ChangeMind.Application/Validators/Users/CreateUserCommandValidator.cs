namespace ChangeMind.Application.Validators.Users;

using ChangeMind.Application.UseCases.Users.Commands;
using ChangeMind.Application.Validators.Rules;
using FluentValidation;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .ValidEmail();

        RuleFor(x => x.Password)
            .StrongPassword();
    }
}
