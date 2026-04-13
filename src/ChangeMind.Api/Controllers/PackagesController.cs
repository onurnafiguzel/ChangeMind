namespace ChangeMind.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.Packages.Commands;
using ChangeMind.Application.UseCases.Packages.Queries;

[ApiController]
[Route("api/packages")]
public class PackagesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get all packages with optional pagination and active filter
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<PackageDto>>> GetPackages(
        [FromQuery] bool? isActiveOnly = true,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPackagesQuery
        {
            IsActiveOnly = isActiveOnly,
            Page = page,
            PageSize = pageSize
        };

        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get package by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PackageDto>> GetPackageById(Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetPackageByIdQuery { PackageId = id };
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create a new package (admin only)
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreatePackage(
        [FromBody] CreatePackageCommand command,
        CancellationToken cancellationToken = default)
    {
        var packageId = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetPackageById), new { id = packageId }, packageId);
    }

    /// <summary>
    /// Update package (admin only)
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePackage(
        Guid id,
        [FromBody] UpdatePackageCommand baseRequest,
        CancellationToken cancellationToken = default)
    {
        var command = baseRequest with { PackageId = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Delete package (soft delete, sets IsActive = false) - admin only
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePackage(Guid id, CancellationToken cancellationToken = default)
    {
        var command = new DeletePackageCommand { PackageId = id };
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
