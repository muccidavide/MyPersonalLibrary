namespace MyPersonalLibrary.Server.Endpoints
{
    using MyPersonalLibrary.Server.Models.DTOs;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    using MyPersonalLibrary.Server.Services.Interfaces;
    using Microsoft.AspNetCore.Builder;

    public static class AuthenticationEndpoints
    {
        public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
        {
            var authGroup = app.MapGroup("/api/authentication")
                               .WithTags("Authentication API");

            authGroup.MapPost("/login", async (
                [FromBody] LoginRequestDto loginRequest,
                IAuthenticationService authService,
                CancellationToken cancellationToken) =>
            {
                var result = await authService.LoginAsync(loginRequest, cancellationToken);
                return result is not null
                    ? Results.Ok(result)
                    : Results.Unauthorized();
            })
            .WithName("Login")
            .WithDescription("Effettua il login di un utente")
            .Produces<LoginResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

            authGroup.MapPost("/register", async (
                [FromBody] RegisterRequestDto registerRequest,
                IAuthenticationService authService,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var result = await authService.RegisterAsync(registerRequest, cancellationToken);
                    return Results.Created($"/api/authentication/me", result);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }
            })
            .WithName("Register")
            .WithDescription("Registra un nuovo utente")
            .Produces<LoginResponseDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

            authGroup.MapPost("/refresh", async (
                [FromBody] RefreshTokenRequestDto refreshRequest,
                IAuthenticationService authService,
                CancellationToken cancellationToken) =>
            {
                var result = await authService.RefreshAccessTokenAsync(refreshRequest, cancellationToken);
                return result is not null
                    ? Results.Ok(result)
                    : Results.Unauthorized();
            })
            .WithName("RefreshToken")
            .WithDescription("Rinnova l'access token")
            .Produces<LoginResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

            authGroup.MapPost("/logout", async (
                [FromBody] RefreshTokenRequestDto refreshRequest,
                IAuthenticationService authService,
                CancellationToken cancellationToken) =>
            {
                await authService.LogoutAsync(refreshRequest.RefreshToken, cancellationToken);
                return Results.Ok(new { message = "Logout effettuato con successo" });
            })
            .WithName("Logout")
            .WithDescription("Effettua il logout revocando il refresh token")
            .Produces(StatusCodes.Status200OK);

            authGroup.MapPost("/logout-all", async (
                HttpContext httpContext,
                IAuthenticationService authService,
                CancellationToken cancellationToken) =>
            {
                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    return Results.Unauthorized();
                }

                await authService.LogoutFromAllDevicesAsync(userId, cancellationToken);
                return Results.Ok(new { message = "Logout da tutti i dispositivi effettuato" });
            })
            .RequireAuthorization()
            .WithName("LogoutFromAllDevices")
            .WithDescription("Effettua il logout da tutti i dispositivi")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

            authGroup.MapGet("/me", (HttpContext httpContext) =>
            {
                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var usernameClaim = httpContext.User.FindFirst(ClaimTypes.Name)
                    ?? httpContext.User.FindFirst("unique_name");
                var emailClaim = httpContext.User.FindFirst(ClaimTypes.Email);
                var roleClaim = httpContext.User.FindFirst(ClaimTypes.Role);

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    return Results.Unauthorized();
                }

                var userInfo = new UserInfoDto
                {
                    UserId = userId,
                    Username = usernameClaim?.Value ?? string.Empty,
                    Email = emailClaim?.Value ?? string.Empty,
                    Role = roleClaim?.Value ?? string.Empty
                };

                return Results.Ok(userInfo);
            })
            .RequireAuthorization()
            .WithName("GetCurrentUser")
            .WithDescription("Ottiene le informazioni dell'utente corrente")
            .Produces<UserInfoDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        }
    }
}
