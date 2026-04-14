namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;

public sealed class UserPhoto
{
    // EF Core constructor — object creation only through factory method
    private UserPhoto() { }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    /// <summary>S3 / MinIO / Azure Blob Storage URL</summary>
    public string ImageUrl { get; private set; } = string.Empty;
    public PhotoViewType ViewType { get; private set; }
    public string? Notes { get; private set; }
    /// <summary>Vücut ölçümleri (JSON: chest, waist, biceps, …)</summary>
    public string? MeasurementsJson { get; private set; }

    public DateTime UploadedAt { get; private set; }

    // Navigation Properties
    public User User { get; private set; } = null!;

    public static UserPhoto Create(
        Guid userId,
        string imageUrl,
        PhotoViewType viewType,
        string? notes = null,
        string? measurementsJson = null)
    {
        return new UserPhoto
        {
            Id               = Guid.NewGuid(),
            UserId           = userId,
            ImageUrl         = imageUrl,
            ViewType         = viewType,
            Notes            = notes,
            MeasurementsJson = measurementsJson,
            UploadedAt       = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Updates the body measurement JSON snapshot associated with this photo.
    /// </summary>
    public void UpdateMeasurements(string measurementsJson)
    {
        MeasurementsJson = measurementsJson;
    }

    /// <summary>
    /// Updates optional coach/user notes on the photo.
    /// </summary>
    public void UpdateNotes(string? notes)
    {
        Notes = notes;
    }
}
