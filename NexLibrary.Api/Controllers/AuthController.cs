using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.Auth;
using NexLibrary.Contracts.Common;

namespace NexLibrary.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IApiClientAuthService _apiClientAuthService;

    public AuthController(
        IAuthService authService,
        IApiClientAuthService apiClientAuthService)
    {
        _authService = authService;
        _apiClientAuthService = apiClientAuthService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.KullaniciAdi))
        {
            return BadRequest(
                ApiResponse<LoginResponse>.Fail("Kullanıcı adı zorunludur."));
        }

        if (string.IsNullOrWhiteSpace(request.Sifre))
        {
            return BadRequest(
                ApiResponse<LoginResponse>.Fail("Şifre zorunludur."));
        }

        var result = await _authService.LoginAsync(
            request,
            cancellationToken);

        if (!result.BasariliMi)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("client-token")]
    public async Task<IActionResult> CreateClientToken(
        ApiClientTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.ClientId))
        {
            return BadRequest(
                ApiResponse<ApiClientTokenResponse>.Fail("ClientId zorunludur."));
        }

        if (string.IsNullOrWhiteSpace(request.ClientSecret))
        {
            return BadRequest(
                ApiResponse<ApiClientTokenResponse>.Fail("ClientSecret zorunludur."));
        }

        var result = await _apiClientAuthService.CreateTokenAsync(
            request,
            cancellationToken);

        if (!result.BasariliMi)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }
}