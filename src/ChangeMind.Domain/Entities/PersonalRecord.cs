namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;

public sealed class PersonalRecord : AggregateRoot
{
    private PersonalRecord() { }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public PersonalRecordLift Lift { get; private set; }
    public decimal WeightKg { get; private set; }
    public DateTime RecordedAt { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User User { get; private set; } = null!;

    public static PersonalRecord Create(
        Guid userId,
        PersonalRecordLift lift,
        decimal weightKg,
        DateTime recordedAt,
        string? notes)
    {
        if (userId == Guid.Empty)
            throw new ValidationException("Kullanıcı kimliği geçersiz.");

        if (weightKg < 0 || weightKg > 1000)
            throw new ValidationException("Ağırlık 0-1000 kg aralığında olmalıdır.");

        if (recordedAt > DateTime.UtcNow.AddDays(1))
            throw new ValidationException("Kayıt tarihi gelecekte olamaz.");

        if (notes is { Length: > 200 })
            throw new ValidationException("Notes en fazla 200 karakter olabilir.");

        return new PersonalRecord
        {
            Id          = Guid.NewGuid(),
            UserId      = userId,
            Lift        = lift,
            WeightKg    = weightKg,
            RecordedAt  = recordedAt,
            Notes       = notes,
            CreatedAt   = DateTime.UtcNow
        };
    }
}
