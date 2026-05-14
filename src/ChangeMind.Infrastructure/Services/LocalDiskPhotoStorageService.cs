namespace ChangeMind.Infrastructure.Services;

using ChangeMind.Application.Services;
using ChangeMind.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class LocalDiskPhotoStorageService(
    IConfiguration configuration,
    ILogger<LocalDiskPhotoStorageService> logger) : IPhotoStorageService
{
    private string Root =>
        configuration["Storage:PhotosRoot"]
            ?? Path.Combine(AppContext.BaseDirectory, "wwwroot", "uploads", "photos");

    /// <summary>URL prefix exposed via UseStaticFiles. Must match what's wired in Program.cs.</summary>
    private const string PublicUrlPrefix = "/uploads/photos";

    public async Task<string> SaveAsync(
        Guid userId,
        PhotoViewType viewType,
        Stream content,
        string contentType,
        CancellationToken cancellationToken)
    {
        var ext = ResolveExtension(contentType);
        var viewSegment = viewType.ToString().ToLowerInvariant();
        var fileName = $"{Guid.NewGuid():N}{ext}";

        var relativeDir = Path.Combine(userId.ToString(), viewSegment);
        var absoluteDir = Path.Combine(Root, relativeDir);
        Directory.CreateDirectory(absoluteDir);

        var absolutePath = Path.Combine(absoluteDir, fileName);
        await using (var fs = File.Create(absolutePath))
        {
            await content.CopyToAsync(fs, cancellationToken);
        }

        var url = $"{PublicUrlPrefix}/{userId}/{viewSegment}/{fileName}";
        logger.LogInformation("Saved photo for user {UserId} ({ViewType}) at {Path}", userId, viewType, absolutePath);
        return url;
    }

    public Task DeleteAsync(string imageUrl, CancellationToken cancellationToken)
    {
        try
        {
            if (!imageUrl.StartsWith(PublicUrlPrefix, StringComparison.OrdinalIgnoreCase))
            {
                logger.LogWarning("Skipping delete for non-local URL {Url}", imageUrl);
                return Task.CompletedTask;
            }

            var relative = imageUrl[PublicUrlPrefix.Length..].TrimStart('/');
            var path = Path.Combine(Root, relative.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(path))
                File.Delete(path);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to delete photo file {Url}", imageUrl);
        }
        return Task.CompletedTask;
    }

    private static string ResolveExtension(string contentType) => contentType.ToLowerInvariant() switch
    {
        "image/png"  => ".png",
        "image/jpeg" => ".jpg",
        _ => throw new InvalidOperationException($"Unsupported content type '{contentType}'.")
    };
}
