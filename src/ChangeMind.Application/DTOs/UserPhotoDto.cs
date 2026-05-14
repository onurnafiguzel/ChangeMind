namespace ChangeMind.Application.DTOs;

using ChangeMind.Domain.Enums;

public class UserPhotoDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public PhotoViewType ViewType { get; set; }
    public DateTime UploadedAt { get; set; }
}
