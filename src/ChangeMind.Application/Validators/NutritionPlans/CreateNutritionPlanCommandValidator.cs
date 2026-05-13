namespace ChangeMind.Application.Validators.NutritionPlans;

using ChangeMind.Application.UseCases.NutritionPlans.Commands;
using ChangeMind.Domain.Enums;
using FluentValidation;

public sealed class CreateNutritionPlanCommandValidator : AbstractValidator<CreateNutritionPlanCommand>
{
    public CreateNutritionPlanCommandValidator()
    {
        RuleFor(x => x.CoachId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Plan başlığı boş olamaz.")
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => x.Description is not null);

        RuleFor(x => x.Days)
            .NotNull().WithMessage("Plan günleri boş olamaz.")
            .Must(d => d != null && d.Count >= 1 && d.Count <= 2)
                .WithMessage("Plan 1 veya 2 gün tipi içermelidir (WorkoutDay/OffDay).")
            .Must(d => d != null && d.Keys.All(k => k == NutritionDayType.WorkoutDay || k == NutritionDayType.OffDay))
                .WithMessage("Sadece WorkoutDay ve OffDay gün tipleri desteklenir.");

        RuleForEach(x => x.Days).ChildRules(day =>
        {
            day.RuleFor(d => d.Value)
                .NotEmpty().WithMessage("Her gün en az bir öğün içermelidir.");

            day.RuleForEach(d => d.Value).ChildRules(meal =>
            {
                meal.RuleFor(m => m.Name)
                    .NotEmpty().WithMessage("Öğün adı boş olamaz.")
                    .MaximumLength(100);
                meal.RuleFor(m => m.Items)
                    .NotEmpty().WithMessage("Her öğün en az bir besin içermelidir.");
                meal.RuleForEach(m => m.Items).ChildRules(item =>
                {
                    item.RuleFor(i => i.FoodId).NotEmpty();
                    item.RuleFor(i => i)
                        .Must(i => (i.Grams.HasValue && i.Grams > 0m) ^ (i.Pieces.HasValue && i.Pieces > 0m))
                        .WithMessage("Her besin için ya 'grams' ya da 'pieces' alanından tam olarak biri pozitif olmalı.");
                });
            });
        });
    }
}
