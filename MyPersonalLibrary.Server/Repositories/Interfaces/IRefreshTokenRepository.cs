using MyPersonalLibrary.Server.Models;

namespace MyPersonalLibrary.Server.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetRefreshTokenByHashAsync(string tokenHash, CancellationToken cancellationToken = default);
        Task<RefreshToken> CreateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
        Task RevokeRefreshTokenAsync(string tokenHash, string? replacedByTokenHash = null, CancellationToken cancellationToken = default);
        Task RevokeAllUserRefreshTokensAsync(int userId, CancellationToken cancellationToken = default);
        Task DeleteExpiredRefreshTokensAsync(CancellationToken cancellationToken = default);
    }
}