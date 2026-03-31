namespace ChangeMind.Application.Services;

using ChangeMind.Domain.Enums;

public interface ITokenService
{
    (string AccessToken, string RefreshToken) GenerateTokens(Guid userId, string email, UserRole role);
    string GenerateRefreshToken();
}
