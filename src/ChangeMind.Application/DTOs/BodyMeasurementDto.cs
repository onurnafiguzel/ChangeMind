namespace ChangeMind.Application.DTOs;

public class BodyMeasurementDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime RecordedAt { get; set; }
    public decimal? WeightKg { get; set; }
    public decimal? WaistCm { get; set; }
    public decimal? ArmCm { get; set; }
    public decimal? LegCm { get; set; }
    public decimal? NeckCm { get; set; }
    public decimal? HipCm { get; set; }
    public decimal? BodyFatPercent { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class MeasurementDeltaDto
{
    public decimal? WeightKg { get; set; }
    public decimal? WaistCm { get; set; }
    public decimal? ArmCm { get; set; }
    public decimal? LegCm { get; set; }
    public decimal? NeckCm { get; set; }
    public decimal? HipCm { get; set; }
    public decimal? BodyFatPercent { get; set; }
}

public class BodyMeasurementComparisonDto
{
    public BodyMeasurementDto? Previous { get; set; }
    public BodyMeasurementDto? Current { get; set; }
    public MeasurementDeltaDto? Delta { get; set; }
}
