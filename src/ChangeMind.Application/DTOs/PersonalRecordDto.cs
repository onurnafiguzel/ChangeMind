namespace ChangeMind.Application.DTOs;

using ChangeMind.Domain.Enums;

public class PersonalRecordDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public PersonalRecordLift Lift { get; set; }
    public decimal WeightKg { get; set; }
    public DateTime RecordedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PersonalRecordCurrentDto
{
    public PersonalRecordLift Lift { get; set; }
    public decimal? WeightKg { get; set; }
    public DateTime? RecordedAt { get; set; }
}
