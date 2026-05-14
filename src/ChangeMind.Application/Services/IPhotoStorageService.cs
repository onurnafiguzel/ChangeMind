namespace ChangeMind.Application.Services;

using ChangeMind.Domain.Enums;

public interface IPhotoStorageService
{
    /// <summary>
    /// Persists a photo binary and returns the URL/path to store in <c>UserPhoto.ImageUrl</c>.
    /// </summary>
    Task<string> SaveAsync(
        Guid userId,
        PhotoViewType viewType,
        Stream content,
        string contentType,
        CancellationToken cancellationToken);

    /// <summary>Deletes a previously saved photo by its stored URL.</summary>
    Task DeleteAsync(string imageUrl, CancellationToken cancellationToken);
}
