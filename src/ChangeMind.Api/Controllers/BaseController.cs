namespace ChangeMind.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public abstract class BaseController : ControllerBase
{
    protected bool IsAuthorizedForUser(Guid userId)
    {
        var tokenUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRoleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

        if (Guid.TryParse(tokenUserIdClaim, out var tokenUserId))
            return tokenUserId == userId || userRoleClaim == "Admin";

        return false;
    }

    protected bool IsAuthorizedForCoach(Guid coachId)
    {
        var tokenUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRoleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

        if (Guid.TryParse(tokenUserIdClaim, out var tokenCoachId))
            return tokenCoachId == coachId || userRoleClaim == "Admin";

        return false;
    }
}
