namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;

public class UserPhoto
{
    // Identifier
    public Guid Id { get; set; }

    // Primitive Properties
    public string ImageUrl { get; set; } // S3/MinIO/Azure URL
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
    public User User { get; set; }
}
