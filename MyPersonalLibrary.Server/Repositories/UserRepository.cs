using MyPersonalLibrary.Server.Models;
using MyPersonalLibrary.Server.Models.Context;
using MyPersonalLibrary.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace MyPersonalLibrary.Server.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly MyPersonalLibraryContext _dbContext;

        public UserRepository(MyPersonalLibraryContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<User?> GetUserByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    user => user.Username == username && user.IsActive,
                    cancellationToken);
        }

        public async Task<User?> GetUserByEmailAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    user => user.Email == email && user.IsActive,
                    cancellationToken);
        }

        public async Task<User?> GetUserByIdAsync(
            int userId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    user => user.UserId == userId && user.IsActive,
                    cancellationToken);
        }

        public async Task<User> CreateUserAsync(
            User user,
            CancellationToken cancellationToken = default)
        {
            user.CreatedAtUtc = DateTime.UtcNow;

            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return user;
        }

        public async Task UpdateLastLoginAsync(
            int userId,
            CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

            if (user is not null)
            {
                user.LastLoginAtUtc = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> UsernameExistsAsync(
            string username,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .AnyAsync(user => user.Username == username, cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .AnyAsync(user => user.Email == email, cancellationToken);
        }
    }
}