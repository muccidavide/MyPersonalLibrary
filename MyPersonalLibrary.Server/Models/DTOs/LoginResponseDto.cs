namespace MyPersonalLibrary.Server.Models.DTOs
{
    public sealed record LoginResponseDto
    {
        /// <summary>
        /// JWT access token (breve durata, da inviare in ogni richiesta).
        /// </summary>
        public string AccessToken { get; init; } = string.Empty;

        /// <summary>
        /// Refresh token (lunga durata, per ottenere nuovi access token).
        /// </summary>
        public string RefreshToken { get; init; } = string.Empty;

        /// <summary>
        /// Timestamp UTC di scadenza dell'access token.
        /// </summary>
        public DateTime AccessTokenExpiresAtUtc { get; init; }

        /// <summary>
        /// Tipo di token (sempre "Bearer").
        /// </summary>
        public string TokenType { get; init; } = "Bearer";

        /// <summary>
        /// Informazioni sull'utente loggato.
        /// </summary>
        public UserInfoDto User { get; init; } = new();
    }
}