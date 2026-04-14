namespace ChangeMind.Application.Validators.Users;

using ChangeMind.Application.UseCases.Users.Commands;
using FluentValidation;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
                .WithMessage("Kullanıcı kimliği geçersiz.");

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

        RuleFor(x => x.Age)
            .InclusiveBetween(1, 120)
                .WithMessage("Yaş 1 ile 120 arasında olmalıdır.")
            .When(x => x.Age.HasValue);

        RuleFor(x => x.Height)
            .InclusiveBetween(50m, 300m)
                .WithMessage("Boy 50 cm ile 300 cm arasında olmalıdır.")
            .When(x => x.Height.HasValue);

        RuleFor(x => x.Weight)
            .InclusiveBetween(1m, 500m)
                .WithMessage("Kilo 1 kg ile 500 kg arasında olmalıdır.")
            .When(x => x.Weight.HasValue);
    }
}
