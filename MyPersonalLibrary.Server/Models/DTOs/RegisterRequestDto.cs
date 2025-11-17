using System.ComponentModel.DataAnnotations;

namespace MyPersonalLibrary.Server.Models.DTOs
{
    public sealed record RegisterRequestDto
    {
        [Required(ErrorMessage = "Username è obbligatorio")]
        [StringLength(100, MinimumLength = 3)]
        public string Username { get; init; } = string.Empty;

        [Required(ErrorMessage = "Email è obbligatoria")]
        [EmailAddress(ErrorMessage = "Email non valida")]
        public string Email { get; init; } = string.Empty;

        [Required(ErrorMessage = "Password è obbligatoria")]
        [StringLength(100, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]",
            ErrorMessage = "Password deve contenere maiuscole, minuscole, numeri e caratteri speciali")]
        public string Password { get; init; } = string.Empty;
    }
}