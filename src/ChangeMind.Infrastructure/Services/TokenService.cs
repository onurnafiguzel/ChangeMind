namespace ChangeMind.Infrastructure.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ChangeMind.Application.Configuration;
using ChangeMind.Application.Services;
using ChangeMind.Domain.Enums;

public class TokenService(JwtSettings jwtSettings) : ITokenService
{
    public (string AccessToken, string RefreshToken) GenerateTokens(Guid userId, string email, UserRole role)
    {
        var accessToken = GenerateAccessToken(userId, email, role);
        var refreshToken = GenerateRefreshToken();
        return (accessToken, refreshToken);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateAccessToken(Guid userId, string email, UserRole role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, userId.ToString()),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, email),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
