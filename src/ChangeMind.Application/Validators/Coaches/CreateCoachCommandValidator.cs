namespace ChangeMind.Application.Validators.Coaches;

using ChangeMind.Application.UseCases.Coaches.Commands;
using ChangeMind.Application.Validators.Rules;
using FluentValidation;

public sealed class CreateCoachCommandValidator : AbstractValidator<CreateCoachCommand>
{
    public CreateCoachCommandValidator()
    {
        RuleFor(x => x.Email)
            .ValidEmail();
    }
}
