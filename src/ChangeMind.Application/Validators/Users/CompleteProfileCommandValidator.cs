namespace ChangeMind.Application.Validators.Users;

using ChangeMind.Application.UseCases.Users.Commands;
using FluentValidation;

public sealed class CompleteProfileCommandValidator : AbstractValidator<CompleteProfileCommand>
{
    public CompleteProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Kullanıcı kimliği geçersiz.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad boş olamaz.")
            .MaximumLength(100).WithMessage("Ad en fazla 100 karakter olabilir.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad boş olamaz.")
            .MaximumLength(100).WithMessage("Soyad en fazla 100 karakter olabilir.");

        RuleFor(x => x.Age)
            .InclusiveBetween(1, 120).WithMessage("Yaş 1 ile 120 arasında olmalıdır.")
            .When(x => x.Age.HasValue);

        RuleFor(x => x.Height)
            .InclusiveBetween(50m, 300m).WithMessage("Boy 50 cm ile 300 cm arasında olmalıdır.")
            .When(x => x.Height.HasValue);

        RuleFor(x => x.Weight)
            .InclusiveBetween(1m, 500m).WithMessage("Kilo 1 kg ile 500 kg arasında olmalıdır.")
            .When(x => x.Weight.HasValue);

        // Health & lifestyle
        RuleFor(x => x.GymDaysPerWeek)
            .InclusiveBetween(0, 7).WithMessage("Gym günleri 0 ile 7 arasında olmalıdır.")
            .When(x => x.GymDaysPerWeek.HasValue);

        RuleFor(x => x.DailyWorkLifestyle)
            .MaximumLength(2000).WithMessage("Günlük yaşam tanımı en fazla 2000 karakter olabilir.")
            .When(x => x.DailyWorkLifestyle is not null);

        RuleFor(x => x.HealthConditions)
            .MaximumLength(2000).WithMessage("Sağlık koşulları en fazla 2000 karakter olabilir.")
            .When(x => x.HealthConditions is not null);

        RuleFor(x => x.FoodAllergies)
            .MaximumLength(2000).WithMessage("Besin alerjileri en fazla 2000 karakter olabilir.")
            .When(x => x.FoodAllergies is not null);

        RuleFor(x => x.SupplementInterest)
            .MaximumLength(2000).WithMessage("Supplement tercihi en fazla 2000 karakter olabilir.")
            .When(x => x.SupplementInterest is not null);
    }
}
