
using System.ComponentModel.DataAnnotations;

namespace MyPersonalLibrary.Server.Models.DTOs
{
    public sealed record LoginRequestDto
    {
        [Required(ErrorMessage = "Username è obbligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username deve essere tra 3 e 100 caratteri")]
        public string Username { get; init; } = string.Empty;

        [Required(ErrorMessage = "Password è obbligatoria")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password deve essere almeno 8 caratteri")]
        public string Password { get; init; } = string.Empty;
    }
}