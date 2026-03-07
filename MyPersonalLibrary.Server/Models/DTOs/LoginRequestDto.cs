
using System.ComponentModel.DataAnnotations;

namespace MyPersonalLibrary.Server.Models.DTOs
{
    public sealed record LoginRequestDto
    {
        [Required(ErrorMessage = "Email è obbligatoria")]
        [EmailAddress(ErrorMessage = "Email non valida")]
        public string Email { get; init; } = string.Empty;

        [Required(ErrorMessage = "Password è obbligatoria")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password deve essere almeno 8 caratteri")]
        public string Password { get; init; } = string.Empty;
    }
}