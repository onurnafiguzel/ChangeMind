namespace ChangeMind.Application.Validators.Exercises;

using ChangeMind.Application.UseCases.Exercises.Commands;
using ChangeMind.Domain.Enums;
using FluentValidation;

public sealed class UpdateExerciseCommandValidator : AbstractValidator<UpdateExerciseCommand>
{
    public UpdateExerciseCommandValidator()
    {
        RuleFor(x => x.ExerciseId)
            .NotEmpty()
                .WithMessage("Egzersiz kimliği geçersiz.");

        RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage("Egzersiz adı boş olamaz.")
            .MaximumLength(255)
                .WithMessage("Egzersiz adı en fazla 255 karakter olabilir.");

        RuleFor(x => x.MuscleGroup)
            .NotEmpty()
                .WithMessage("Kas grubu boş olamaz.")
            .Must(v => Enum.TryParse<MuscleGroup>(v, ignoreCase: true, out _))
                .WithMessage($"Geçersiz kas grubu. Geçerli değerler: {string.Join(", ", Enum.GetNames<MuscleGroup>())}");

        RuleFor(x => x.DifficultyLevel)
            .NotEmpty()
                .WithMessage("Zorluk seviyesi boş olamaz.")
            .Must(v => Enum.TryParse<DifficultyLevel>(v, ignoreCase: true, out _))
                .WithMessage("Geçersiz zorluk seviyesi. Geçerli değerler: Beginner, Intermediate, Advanced.");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
                .WithMessage("Açıklama en fazla 1000 karakter olabilir.")
            .When(x => x.Description is not null);

        RuleFor(x => x.VideoUrl)
            .MaximumLength(500)
                .WithMessage("Video URL en fazla 500 karakter olabilir.")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("Geçerli bir video URL giriniz.")
            .When(x => !string.IsNullOrEmpty(x.VideoUrl));
    }
}
