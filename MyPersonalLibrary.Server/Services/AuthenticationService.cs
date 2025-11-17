namespace MyPersonalLibrary.Server.Services
{

    using MyPersonalLibrary.Server.Models.DTOs;
    using MyPersonalLibrary.Server.Models;
    using MyPersonalLibrary.Server.Repositories.Interfaces;
    using MyPersonalLibrary.Server.Services.Interfaces;

    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashingService _passwordHashingService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            IUserRepository userRepository,
            IPasswordHashingService passwordHashingService,
            IJwtTokenService jwtTokenService,
            IRefreshTokenService refreshTokenService,
            ILogger<AuthenticationService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHashingService = passwordHashingService ?? throw new ArgumentNullException(nameof(passwordHashingService));
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
            _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<LoginResponseDto?> LoginAsync(
            LoginRequestDto loginRequest,
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByUsernameAsync(
                loginRequest.Username,
                cancellationToken);

            if (user is null)
            {
                _logger.LogWarning("Tentativo di login fallito: username '{Username}' non trovato", loginRequest.Username);
                return null;
            }

            var isPasswordValid = _passwordHashingService.VerifyPassword(
                loginRequest.Password,
                user.PasswordHash);

            if (!isPasswordValid)
            {
                _logger.LogWarning("Tentativo di login fallito: password errata per username '{Username}'", loginRequest.Username);
                return null;
            }

            var accessToken = _jwtTokenService.GenerateAccessToken(
                user.UserId,
                user.Username,
                user.Role);

            var (refreshToken, _) = await _refreshTokenService.CreateRefreshTokenForUserAsync(
                user.UserId,
                cancellationToken);

            await _userRepository.UpdateLastLoginAsync(user.UserId, cancellationToken);

            _logger.LogInformation("Login effettuato con successo per utente '{Username}' (ID: {UserId})",
                user.Username, user.UserId);

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAtUtc = _jwtTokenService.GetAccessTokenExpirationTime(),
                TokenType = "Bearer",
                User = new UserInfoDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role
                }
            };
        }

        public async Task<LoginResponseDto> RegisterAsync(
            RegisterRequestDto registerRequest,
            CancellationToken cancellationToken = default)
        {
            var usernameExists = await _userRepository.UsernameExistsAsync(
                registerRequest.Username,
                cancellationToken);

            if (usernameExists)
            {
                throw new InvalidOperationException($"Username '{registerRequest.Username}' già esistente");
            }

            var emailExists = await _userRepository.EmailExistsAsync(
                registerRequest.Email,
                cancellationToken);

            if (emailExists)
            {
                throw new InvalidOperationException($"Email '{registerRequest.Email}' già registrata");
            }

            var passwordHash = _passwordHashingService.HashPassword(registerRequest.Password);

            var newUser = new User
            {
                Username = registerRequest.Username,
                Email = registerRequest.Email,
                PasswordHash = passwordHash,
                Role = "Editor",
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            };

            var createdUser = await _userRepository.CreateUserAsync(newUser, cancellationToken);

            _logger.LogInformation("Nuovo utente registrato: '{Username}' (ID: {UserId})",
                createdUser.Username, createdUser.UserId);

            var accessToken = _jwtTokenService.GenerateAccessToken(
                createdUser.UserId,
                createdUser.Username,
                createdUser.Role);

            var (refreshToken, _) = await _refreshTokenService.CreateRefreshTokenForUserAsync(
                createdUser.UserId,
                cancellationToken);

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAtUtc = _jwtTokenService.GetAccessTokenExpirationTime(),
                TokenType = "Bearer",
                User = new UserInfoDto
                {
                    UserId = createdUser.UserId,
                    Username = createdUser.Username,
                    Email = createdUser.Email,
                    Role = createdUser.Role
                }
            };
        }

        public async Task<LoginResponseDto?> RefreshAccessTokenAsync(
            RefreshTokenRequestDto refreshRequest,
            CancellationToken cancellationToken = default)
        {
            var refreshToken = await _refreshTokenService.ValidateRefreshTokenAsync(
                refreshRequest.RefreshToken,
                cancellationToken);

            if (refreshToken is null)
            {
                _logger.LogWarning("Tentativo di refresh con token non valido o scaduto");
                return null;
            }

            var user = await _userRepository.GetUserByIdAsync(
                refreshToken.UserId,
                cancellationToken);

            if (user is null)
            {
                _logger.LogWarning("Utente non trovato per refresh token (UserId: {UserId})", refreshToken.UserId);
                return null;
            }

            var newAccessToken = _jwtTokenService.GenerateAccessToken(
                user.UserId,
                user.Username,
                user.Role);

            var (newRefreshToken, _) = await _refreshTokenService.CreateRefreshTokenForUserAsync(
                user.UserId,
                cancellationToken);

            await _refreshTokenService.RevokeRefreshTokenAsync(
                refreshRequest.RefreshToken,
                newRefreshToken,
                cancellationToken);

            _logger.LogInformation("Token rinnovato per utente '{Username}' (ID: {UserId})",
                user.Username, user.UserId);

            return new LoginResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiresAtUtc = _jwtTokenService.GetAccessTokenExpirationTime(),
                TokenType = "Bearer",
                User = new UserInfoDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role
                }
            };
        }

        public async Task LogoutAsync(
            string refreshToken,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return;
            }

            await _refreshTokenService.RevokeRefreshTokenAsync(refreshToken, null, cancellationToken);

            _logger.LogInformation("Logout effettuato, refresh token revocato");
        }

        public async Task LogoutFromAllDevicesAsync(
            int userId,
            CancellationToken cancellationToken = default)
        {
            await _refreshTokenService.RevokeAllUserTokensAsync(userId, cancellationToken);

            _logger.LogInformation("Logout da tutti i dispositivi per utente ID: {UserId}", userId);
        }
    }
}
