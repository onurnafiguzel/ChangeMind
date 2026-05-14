namespace ChangeMind.Application.UseCases.UserPhotos.Commands;

using MediatR;
using ChangeMind.Application.DTOs;

public record UploadUserPhotosCommand(
    Guid UserId,
    UserPhotoUpload Front,
    UserPhotoUpload Back,
    UserPhotoUpload Left,
    UserPhotoUpload Right) : IRequest<List<UserPhotoDto>>;

public record UserPhotoUpload(
    Stream Content,
    string ContentType,
    long Length,
    string? FileName);
