namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Exceptions;

public sealed class BodyMeasurement : AggregateRoot
{
    private BodyMeasurement() { }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime RecordedAt { get; private set; }
    public decimal? WeightKg { get; private set; }
    public decimal? WaistCm { get; private set; }
    public decimal? ArmCm { get; private set; }
    public decimal? LegCm { get; private set; }
    public decimal? NeckCm { get; private set; }
    public decimal? HipCm { get; private set; }
    public decimal? BodyFatPercent { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User User { get; private set; } = null!;

    public static BodyMeasurement Create(
        Guid userId,
        DateTime recordedAt,
        decimal? weightKg,
        decimal? waistCm,
        decimal? armCm,
        decimal? legCm,
        decimal? neckCm,
        decimal? hipCm,
        decimal? bodyFatPercent,
        string? notes)
    {
        if (userId == Guid.Empty)
            throw new ValidationException("Kullanıcı kimliği geçersiz.");

        if (recordedAt > DateTime.UtcNow.AddDays(1))
            throw new ValidationException("Kayıt tarihi gelecekte olamaz.");

        ValidatePositiveRange(waistCm, nameof(waistCm), 30, 250);
        ValidatePositiveRange(armCm,   nameof(armCm),    10, 100);
        ValidatePositiveRange(legCm,   nameof(legCm),    20, 150);
        ValidatePositiveRange(neckCm,  nameof(neckCm),   20, 100);
        ValidatePositiveRange(hipCm,   nameof(hipCm),    30, 250);
        ValidatePositiveRange(weightKg, nameof(weightKg), 20, 400);
        ValidatePositiveRange(bodyFatPercent, nameof(bodyFatPercent), 2, 60);

        if (notes is { Length: > 500 })
            throw new ValidationException("Notes en fazla 500 karakter olabilir.");

        return new BodyMeasurement
        {
            Id              = Guid.NewGuid(),
            UserId          = userId,
            RecordedAt      = recordedAt,
            WeightKg        = weightKg,
            WaistCm         = waistCm,
            ArmCm           = armCm,
            LegCm           = legCm,
            NeckCm          = neckCm,
            HipCm           = hipCm,
            BodyFatPercent  = bodyFatPercent,
            Notes           = notes,
            CreatedAt       = DateTime.UtcNow
        };
    }

    private static void ValidatePositiveRange(decimal? value, string name, decimal min, decimal max)
    {
        if (value is null) return;
        if (value <= 0 || value < min || value > max)
            throw new ValidationException($"{name} değeri {min}-{max} aralığında olmalıdır.");
    }
}
