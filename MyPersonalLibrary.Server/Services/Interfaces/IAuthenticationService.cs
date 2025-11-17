using MyPersonalLibrary.Server.Models.DTOs;

namespace MyPersonalLibrary.Server.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponseDto?> LoginAsync(
            LoginRequestDto loginRequest,
            CancellationToken cancellationToken = default);

        Task<LoginResponseDto> RegisterAsync(
            RegisterRequestDto registerRequest,
            CancellationToken cancellationToken = default);

        Task<LoginResponseDto?> RefreshAccessTokenAsync(
            RefreshTokenRequestDto refreshRequest,
            CancellationToken cancellationToken = default);

        Task LogoutAsync(
            string refreshToken,
            CancellationToken cancellationToken = default);

        Task LogoutFromAllDevicesAsync(
            int userId,
            CancellationToken cancellationToken = default);
    }
}
