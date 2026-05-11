namespace ChangeMind.Application.Validators.WorkoutSessions;

using System.Text.RegularExpressions;
using FluentValidation;
using ChangeMind.Application.UseCases.WorkoutSessions.Commands;

public sealed class CreateWorkoutSessionCommandValidator : AbstractValidator<CreateWorkoutSessionCommand>
{
    private static readonly Regex DayKeyPattern = new(@"^Day-\d+$", RegexOptions.Compiled);

    public CreateWorkoutSessionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Kullanıcı kimliği geçersiz.");

        RuleFor(x => x.DayKey)
            .NotEmpty()
            .WithMessage("Gün anahtarı boş olamaz.")
            .Must(d => DayKeyPattern.IsMatch(d))
            .WithMessage("Gün anahtarı 'Day-N' formatında olmalıdır (örn: Day-1, Day-3).");

        RuleFor(x => x.RecordDate)
            .Must(d => d <= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Kayıt tarihi gelecekte olamaz.");

        RuleFor(x => x.Exercises)
            .NotNull().WithMessage("Egzersiz listesi gönderilmelidir.")
            .Must(e => e.Count > 0).WithMessage("En az bir egzersiz gönderilmelidir.");

        RuleForEach(x => x.Exercises).ChildRules(exercise =>
        {
            exercise.RuleFor(e => e.ExerciseId).NotEmpty()
                .WithMessage("Egzersiz kimliği geçersiz.");

            exercise.RuleFor(e => e.Sets)
                .NotNull().WithMessage("Set listesi gönderilmelidir.")
                .Must(s => s.Count > 0).WithMessage("Her egzersizde en az bir set olmalıdır.");

            exercise.RuleForEach(e => e.Sets).ChildRules(set =>
            {
                set.RuleFor(s => s.SetNumber).GreaterThan(0)
                    .WithMessage("Set numarası 0'dan büyük olmalıdır.");
                set.RuleFor(s => s.Weight).GreaterThanOrEqualTo(0)
                    .WithMessage("Ağırlık negatif olamaz.");
                set.RuleFor(s => s.Reps).GreaterThan(0)
                    .WithMessage("Tekrar sayısı en az 1 olmalıdır.");
            });
        });
    }
}
