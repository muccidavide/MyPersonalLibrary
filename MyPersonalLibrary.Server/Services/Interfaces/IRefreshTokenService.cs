using MyPersonalLibrary.Server.Models;

namespace MyPersonalLibrary.Server.Services.Interfaces
{
    public interface IRefreshTokenService
    {
     
        Task<(string rawToken, RefreshToken entity)> CreateRefreshTokenForUserAsync(
            int userId,
            CancellationToken cancellationToken = default);

        Task<RefreshToken?> ValidateRefreshTokenAsync(
            string rawToken,
            CancellationToken cancellationToken = default);

        Task RevokeRefreshTokenAsync(
            string rawToken,
            string? replacedByRawToken = null,
            CancellationToken cancellationToken = default);

        Task RevokeAllUserTokensAsync(
            int userId,
            CancellationToken cancellationToken = default);
    }

}
