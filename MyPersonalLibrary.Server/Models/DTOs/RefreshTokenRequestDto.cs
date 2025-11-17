using System.ComponentModel.DataAnnotations;

namespace MyPersonalLibrary.Server.Models.DTOs
{
    public sealed record RefreshTokenRequestDto
    {
        [Required(ErrorMessage = "Refresh token è obbligatorio")]
        public string RefreshToken { get; init; } = string.Empty;
    }
}