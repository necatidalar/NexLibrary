using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.Auth;
using NexLibrary.Web.Services;
using NexLibrary.Web.ViewModels.Auth;

namespace NexLibrary.Web.Controllers;

[AllowAnonymous]
public sealed class AccountController : Controller
{
    private readonly AuthApiService _authApiService;

    public AccountController(AuthApiService authApiService)
    {
        _authApiService = authApiService;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Dashboard");
        }

        var model = new LoginViewModel
        {
            ReturnUrl = returnUrl
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        LoginViewModel model,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(model.KullaniciAdi))
        {
            TempData["ErrorMessage"] = "Kullanıcı adı zorunludur.";
            return View(model);
        }

        if (string.IsNullOrWhiteSpace(model.Sifre))
        {
            TempData["ErrorMessage"] = "Şifre zorunludur.";
            return View(model);
        }

        var request = new LoginRequest
        {
            KullaniciAdi = model.KullaniciAdi.Trim(),
            Sifre = model.Sifre,
            BeniHatirla = model.BeniHatirla
        };

        var loginResult = await _authApiService.LoginAsync(
            request,
            cancellationToken);

        if (loginResult is null)
        {
            TempData["ErrorMessage"] = "Kullanıcı adı veya şifre hatalı.";
            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, loginResult.KullaniciId.ToString()),
            new Claim(ClaimTypes.Name, loginResult.KullaniciAdi),
            new Claim("AdSoyad", loginResult.AdSoyad)
        };

        if (!string.IsNullOrWhiteSpace(loginResult.Eposta))
        {
            claims.Add(new Claim(ClaimTypes.Email, loginResult.Eposta));
        }

        foreach (var role in loginResult.Roller)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = model.BeniHatirla,
            ExpiresUtc = model.BeniHatirla
                ? DateTimeOffset.UtcNow.AddDays(14)
                : DateTimeOffset.UtcNow.AddHours(8)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            authProperties);

        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return LocalRedirect(model.ReturnUrl);
        }

        return RedirectToAction("Index", "Dashboard");
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}