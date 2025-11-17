using MyPersonalLibrary.Server.Repositories.Interfaces;
using MyPersonalLibrary.Server.Models;
using MyPersonalLibrary.Server.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace MyPersonalLibrary.Server.Repositories
{
    public sealed class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly MyPersonalLibraryContext _dbContext;

        public RefreshTokenRepository(MyPersonalLibraryContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<RefreshToken?> GetRefreshTokenByHashAsync(
            string tokenHash,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash, cancellationToken);
        }

        public async Task<RefreshToken> CreateRefreshTokenAsync(
            RefreshToken refreshToken,
            CancellationToken cancellationToken = default)
        {
            refreshToken.CreatedAtUtc = DateTime.UtcNow;

            await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return refreshToken;
        }

        public async Task RevokeRefreshTokenAsync(
            string tokenHash,
            string? replacedByTokenHash = null,
            CancellationToken cancellationToken = default)
        {
            var token = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash, cancellationToken);

            if (token is not null)
            {
                token.IsRevoked = true;
                token.RevokedAtUtc = DateTime.UtcNow;
                token.ReplacedByTokenHash = replacedByTokenHash;

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task RevokeAllUserRefreshTokensAsync(
            int userId,
            CancellationToken cancellationToken = default)
        {
            var tokens = await _dbContext.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked)
                .ToListAsync(cancellationToken);

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.RevokedAtUtc = DateTime.UtcNow;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteExpiredRefreshTokensAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var thirtyDaysAgo = now.AddDays(-30);

            var expiredTokens = await _dbContext.RefreshTokens
                .Where(rt => rt.ExpiresAtUtc < now || (rt.IsRevoked && rt.RevokedAtUtc < thirtyDaysAgo))
                .ToListAsync(cancellationToken);

            _dbContext.RefreshTokens.RemoveRange(expiredTokens);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}