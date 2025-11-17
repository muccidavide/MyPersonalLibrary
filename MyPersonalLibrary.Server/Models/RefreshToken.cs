namespace MyPersonalLibrary.Server.Models
{
    public sealed class RefreshToken
    {
        public int RefreshTokenId { get; set; }

        public int UserId { get; set; }

        public string TokenHash { get; set; } = string.Empty;

        public DateTime ExpiresAtUtc { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public bool IsRevoked { get; set; } = false;

        public DateTime? RevokedAtUtc { get; set; }

        public string? ReplacedByTokenHash { get; set; }

        public User User { get; set; } = null!;

        public bool IsValidForUse()
        {
            return !IsRevoked && DateTime.UtcNow < ExpiresAtUtc;
        }
    }
}