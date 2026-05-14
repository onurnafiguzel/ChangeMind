namespace ChangeMind.Application.UseCases.UserPhotos.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public record GetUserPhotosQuery(Guid UserId) : IRequest<List<UserPhotoDto>>;
