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
    public DateTime CreatedAt { get; private set; }

    public User User { get; private set; } = null!;

    public static PersonalRecord Create(
        Guid userId,
        PersonalRecordLift lift,
        decimal weightKg,
        DateTime recordedAt)
    {
        if (userId == Guid.Empty)
            throw new ValidationException("Kullanıcı kimliği geçersiz.");

        if (weightKg < 0 || weightKg > 1000)
            throw new ValidationException("Ağırlık 0-1000 kg aralığında olmalıdır.");

        if (recordedAt > DateTime.UtcNow.AddDays(1))
            throw new ValidationException("Kayıt tarihi gelecekte olamaz.");

        return new PersonalRecord
        {
            Id          = Guid.NewGuid(),
            UserId      = userId,
            Lift        = lift,
            WeightKg    = weightKg,
            RecordedAt  = recordedAt,
            CreatedAt   = DateTime.UtcNow
        };
    }
}
