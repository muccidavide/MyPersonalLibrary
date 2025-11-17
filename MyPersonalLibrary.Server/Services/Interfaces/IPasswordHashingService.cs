namespace MyPersonalLibrary.Server.Services.Interfaces
{
    public interface IPasswordHashingService
    {
        string HashPassword(string plainTextPassword);
        bool VerifyPassword(string plainTextPassword, string passwordHash);
    }
}