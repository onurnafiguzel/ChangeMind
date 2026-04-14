namespace ChangeMind.Application.Validators.TrainingPrograms;

using ChangeMind.Application.UseCases.TrainingPrograms.Commands;
using FluentValidation;

public sealed class CreateTrainingProgramCommandValidator : AbstractValidator<CreateTrainingProgramCommand>
{
    public CreateTrainingProgramCommandValidator()
    {
        RuleFor(x => x.CoachId)
            .NotEmpty()
                .WithMessage("Koç kimliği geçersiz.");

        RuleFor(x => x.UserId)
            .NotEmpty()
                .WithMessage("Kullanıcı kimliği geçersiz.");

        RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage("Program adı boş olamaz.")
            .MaximumLength(200)
                .WithMessage("Program adı en fazla 200 karakter olabilir.");

        RuleFor(x => x.DurationWeeks)
            .InclusiveBetween(1, 52)
                .WithMessage("Program süresi 1 ile 52 hafta arasında olmalıdır.");

        RuleFor(x => x.Description)
            .MaximumLength(2000)
                .WithMessage("Açıklama en fazla 2000 karakter olabilir.")
            .When(x => x.Description is not null);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
                .WithMessage("Bitiş tarihi başlangıç tarihinden sonra olmalıdır.")
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue);
    }
}
