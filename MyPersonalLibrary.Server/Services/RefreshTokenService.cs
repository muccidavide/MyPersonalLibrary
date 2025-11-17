using Microsoft.Extensions.Options;
using MyPersonalLibrary.Server.Models;
using MyPersonalLibrary.Server.Models.Configurations;
using MyPersonalLibrary.Server.Repositories.Interfaces;
using MyPersonalLibrary.Server.Services.Interfaces;

namespace MyPersonalLibrary.Server.Services
{
    public sealed class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenService(
            IRefreshTokenRepository refreshTokenRepository,
            IJwtTokenService jwtTokenService,
            IOptions<JwtSettings> jwtSettings)
        {
            _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<(string rawToken, RefreshToken entity)> CreateRefreshTokenForUserAsync(
            int userId,
            CancellationToken cancellationToken = default)
        {
            var rawToken = _jwtTokenService.GenerateRefreshToken();
            var tokenHash = _jwtTokenService.ComputeTokenHash(rawToken);

            var refreshTokenEntity = new RefreshToken
            {
                UserId = userId,
                TokenHash = tokenHash,
                ExpiresAtUtc = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                CreatedAtUtc = DateTime.UtcNow
            };

            var createdEntity = await _refreshTokenRepository.CreateRefreshTokenAsync(
                refreshTokenEntity,
                cancellationToken);

            return (rawToken, createdEntity);
        }

        public async Task<RefreshToken?> ValidateRefreshTokenAsync(
            string rawToken,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(rawToken))
            {
                return null;
            }

            var tokenHash = _jwtTokenService.ComputeTokenHash(rawToken);
            var refreshToken = await _refreshTokenRepository.GetRefreshTokenByHashAsync(
                tokenHash,
                cancellationToken);

            if (refreshToken is null || !refreshToken.IsValidForUse())
            {
                return null;
            }

            return refreshToken;
        }

        public async Task RevokeRefreshTokenAsync(
            string rawToken,
            string? replacedByRawToken = null,
            CancellationToken cancellationToken = default)
        {
            var tokenHash = _jwtTokenService.ComputeTokenHash(rawToken);

            string? replacedByTokenHash = null;
            if (!string.IsNullOrWhiteSpace(replacedByRawToken))
            {
                replacedByTokenHash = _jwtTokenService.ComputeTokenHash(replacedByRawToken);
            }

            await _refreshTokenRepository.RevokeRefreshTokenAsync(
                tokenHash,
                replacedByTokenHash,
                cancellationToken);
        }

        public async Task RevokeAllUserTokensAsync(
            int userId,
            CancellationToken cancellationToken = default)
        {
            await _refreshTokenRepository.RevokeAllUserRefreshTokensAsync(userId, cancellationToken);
        }
    }
}
