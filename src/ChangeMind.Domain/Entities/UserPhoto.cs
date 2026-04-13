namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;

public class UserPhoto
{
    private UserPhoto() { }

    // Identifier
    public Guid Id { get; set; }

    // Primitive Properties
    public string ImageUrl { get; set; } = string.Empty; // S3/MinIO/Azure URL
    public PhotoViewType ViewType { get; set; }
    public string? Notes { get; set; }
    /// <summary>
    /// Vücut Ölçümleri (JSON: chest, waist, biceps, etc.)
    /// </summary>
    public string? MeasurementsJson { get; set; }

    // DateTime Properties
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public Guid UserId { get; set; }

    // Navigation Properties
    public User User { get; set; } = null!;

    public static UserPhoto Create(Guid userId, string imageUrl, PhotoViewType viewType, string? notes = null, string? measurementsJson = null)
    {
        return new UserPhoto
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ImageUrl = imageUrl,
            ViewType = viewType,
            Notes = notes,
            MeasurementsJson = measurementsJson,
            UploadedAt = DateTime.UtcNow
        };
    }
}
