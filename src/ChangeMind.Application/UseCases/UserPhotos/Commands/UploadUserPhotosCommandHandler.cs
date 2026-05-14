namespace ChangeMind.Application.UseCases.UserPhotos.Commands;

using MediatR;
using ChangeMind.Application.Configuration;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;

public class UploadUserPhotosCommandHandler(
    IUserRepository userRepository,
    IUserPhotoRepository photoRepository,
    IPhotoStorageService photoStorage,
    IUnitOfWork unitOfWork,
    ICacheService cache) : IRequestHandler<UploadUserPhotosCommand, List<UserPhotoDto>>
{
    private const long MaxBytes = 10L * 1024 * 1024; // 10 MB

    public async Task<List<UserPhotoDto>> Handle(UploadUserPhotosCommand request, CancellationToken cancellationToken)
    {
        _ = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User '{request.UserId}' not found.");

        var uploads = new (PhotoViewType View, UserPhotoUpload File)[]
        {
            (PhotoViewType.Front, request.Front),
            (PhotoViewType.Back,  request.Back),
            (PhotoViewType.Left,  request.Left),
            (PhotoViewType.Right, request.Right)
        };

        foreach (var (view, file) in uploads)
            ValidateUpload(view, file);

        // History mode: previous uploads stay in DB & on disk; this call just appends a new set of 4.
        var newPhotos = new List<UserPhoto>();
        foreach (var (view, file) in uploads)
        {
            var url = await photoStorage.SaveAsync(request.UserId, view, file.Content, file.ContentType, cancellationToken);
            newPhotos.Add(UserPhoto.Create(request.UserId, url, view));
        }

        await photoRepository.AddRangeAsync(newPhotos);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(CacheKeys.User(request.UserId), cancellationToken);

        return newPhotos.Select(p => new UserPhotoDto
        {
            Id         = p.Id,
            UserId     = p.UserId,
            ImageUrl   = p.ImageUrl,
            ViewType   = p.ViewType,
            UploadedAt = p.UploadedAt
        }).ToList();
    }

    private static void ValidateUpload(PhotoViewType view, UserPhotoUpload upload)
    {
        if (upload is null)
            throw new ValidationException($"'{view}' fotoğrafı zorunludur.");

        if (upload.Length <= 0 || upload.Length > MaxBytes)
            throw new ValidationException($"'{view}' dosyası 0..10 MB arası olmalıdır.");

        var ct = upload.ContentType?.ToLowerInvariant();
        if (ct is not ("image/png" or "image/jpeg"))
            throw new ValidationException($"'{view}' yalnızca PNG veya JPEG kabul edilir.");
    }
}
