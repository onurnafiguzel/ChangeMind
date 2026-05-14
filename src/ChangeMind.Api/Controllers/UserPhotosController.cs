namespace ChangeMind.Api.Controllers;

using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.UserPhotos.Commands;
using ChangeMind.Application.UseCases.UserPhotos.Queries;
using ChangeMind.Domain.Exceptions;

[ApiController]
[Route("api/users/{userId:guid}/photos")]
[Authorize]
public class UserPhotosController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Upload (or replace) the 4 body view photos for the user.
    /// Must be sent as multipart/form-data with fields: front, back, left, right (PNG or JPEG, ≤10 MB each).
    /// Only the user themselves may upload.
    /// </summary>
    [HttpPost]
    [RequestSizeLimit(50 * 1024 * 1024)]
    public async Task<ActionResult<List<UserPhotoDto>>> Upload(
        Guid userId,
        [FromForm(Name = "front")] IFormFile front,
        [FromForm(Name = "back")]  IFormFile back,
        [FromForm(Name = "left")]  IFormFile left,
        [FromForm(Name = "right")] IFormFile right,
        CancellationToken cancellationToken = default)
    {
        AuthorizeUploadFor(userId);
        if (front is null || back is null || left is null || right is null)
            throw new ValidationException("front, back, left, right alanlarının dördü de zorunludur.");

        await using var fs = front.OpenReadStream();
        await using var bs = back.OpenReadStream();
        await using var ls = left.OpenReadStream();
        await using var rs = right.OpenReadStream();

        var command = new UploadUserPhotosCommand(
            userId,
            new UserPhotoUpload(fs, front.ContentType, front.Length, front.FileName),
            new UserPhotoUpload(bs, back.ContentType,  back.Length,  back.FileName),
            new UserPhotoUpload(ls, left.ContentType,  left.Length,  left.FileName),
            new UserPhotoUpload(rs, right.ContentType, right.Length, right.FileName));

        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>List photos for a user — owner, coach, or admin.</summary>
    [HttpGet]
    public async Task<ActionResult<List<UserPhotoDto>>> List(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        AuthorizeViewFor(userId);
        var result = await mediator.Send(new GetUserPhotosQuery(userId), cancellationToken);
        return Ok(result);
    }

    private void AuthorizeUploadFor(Guid userId)
    {
        var sub = User.FindFirstValue("sub");
        if (!Guid.TryParse(sub, out var tokenGuid) || tokenGuid != userId)
            throw new ForbiddenException("Yalnızca kendi fotoğraflarınızı yükleyebilirsiniz.");
    }

    private void AuthorizeViewFor(Guid userId)
    {
        var role = User.FindFirstValue("role");
        if (role == "Admin" || role == "Coach") return;

        var sub = User.FindFirstValue("sub");
        if (!Guid.TryParse(sub, out var tokenGuid) || tokenGuid != userId)
            throw new ForbiddenException("Yalnızca kendi fotoğraflarınıza erişebilirsiniz.");
    }
}
