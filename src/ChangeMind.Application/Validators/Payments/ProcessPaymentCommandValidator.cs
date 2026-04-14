namespace ChangeMind.Application.Validators.Payments;

using ChangeMind.Application.UseCases.Payments.Commands;
using FluentValidation;

public sealed class ProcessPaymentCommandValidator : AbstractValidator<ProcessPaymentCommand>
{
    public ProcessPaymentCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
                .WithMessage("Kullanıcı kimliği geçersiz.");

        RuleFor(x => x.PackageId)
            .NotEmpty()
                .WithMessage("Paket kimliği geçersiz.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
                .WithMessage("Ödeme tutarı sıfırdan büyük olmalıdır.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
                .WithMessage("Açıklama en fazla 500 karakter olabilir.")
            .When(x => x.Description is not null);
    }
}
