using MyPersonalLibrary.Server.Services.Interfaces;

namespace MyPersonalLibrary.Server.Services
{
    public sealed class BCryptPasswordHashingService : IPasswordHashingService
    {
        private const int WorkFactor = 11;

        public string HashPassword(string plainTextPassword)
        {
            if (string.IsNullOrWhiteSpace(plainTextPassword))
            {
                throw new ArgumentException("Password non può essere vuota", nameof(plainTextPassword));
            }
            return BCrypt.Net.BCrypt.HashPassword(plainTextPassword, workFactor: WorkFactor);
        }

        public bool VerifyPassword(string plainTextPassword, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(plainTextPassword) || string.IsNullOrWhiteSpace(passwordHash))
            {
                return false;
            }

            try
            {
                return BCrypt.Net.BCrypt.Verify(plainTextPassword, passwordHash);
            }
            catch
            {
                return false;
            }
        }
    }
}