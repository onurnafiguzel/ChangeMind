namespace ChangeMind.Application.Validators.Rules;

using FluentValidation;

/// <summary>
/// Reusable FluentValidation extensions for password strength rules.
/// Applied to any property that represents a plain-text password.
/// </summary>
public static class PasswordRules
{
    /// <summary>
    /// Applies the standard ChangeMind password policy to the given rule builder.
    /// Rules: min 8 chars, at least one uppercase, lowercase, digit, special char.
    /// </summary>
    public static IRuleBuilderOptions<T, string> StrongPassword<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithMessage("Şifre boş olamaz.")
            .MinimumLength(8)
                .WithMessage("Şifre en az 8 karakter olmalıdır.")
            .Matches("[A-Z]")
                .WithMessage("Şifre en az bir büyük harf içermelidir.")
            .Matches("[a-z]")
                .WithMessage("Şifre en az bir küçük harf içermelidir.")
            .Matches("[0-9]")
                .WithMessage("Şifre en az bir rakam içermelidir.")
            .Matches("[^a-zA-Z0-9]")
                .WithMessage("Şifre en az bir özel karakter içermelidir (örn. !, @, #, $).");
    }

    /// <summary>
    /// Applies a valid email format rule.
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidEmail<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithMessage("E-posta adresi boş olamaz.")
            .EmailAddress()
                .WithMessage("Geçerli bir e-posta adresi giriniz.")
            .MaximumLength(254)
                .WithMessage("E-posta adresi en fazla 254 karakter olabilir.");
    }
}
