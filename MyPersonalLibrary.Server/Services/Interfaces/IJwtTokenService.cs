using System.Security.Claims;

public interface IJwtTokenService
{
    string GenerateAccessToken(int userId, string username, string role);
    string GenerateRefreshToken();
    ClaimsPrincipal? ValidateAndExtractClaims(string token);
    string ComputeTokenHash(string token);
    DateTime GetAccessTokenExpirationTime();
}