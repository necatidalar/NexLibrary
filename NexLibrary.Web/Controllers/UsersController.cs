using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Permissions;
using NexLibrary.Contracts.Users;
using NexLibrary.Web.Security;
using NexLibrary.Web.Services;
using NexLibrary.Web.ViewModels.Users;

namespace NexLibrary.Web.Controllers;

public sealed class UsersController : Controller
{
    private readonly UserApiService _userApiService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        UserApiService userApiService,
        ILogger<UsersController> logger)
    {
        _userApiService = userApiService;
        _logger = logger;
    }

    [PermissionAuthorize(PermissionCodes.UsersView)]
    public async Task<IActionResult> Index(
        string? search = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 20 : pageSize;

        PagedResponse<UserListResponse>? users;

        try
        {
            users = await _userApiService.GetPagedAsync(pageNumber, pageSize, search, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı listesi alınırken hata oluştu.");
            users = null;
        }

        if (users is null)
        {
            ViewBag.ErrorMessage = "API bağlantısı kurulamadı veya kullanıcı listesi alınamadı.";
            users = new PagedResponse<UserListResponse>
            {
                Items = new List<UserListResponse>(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = 0
            };
        }

        var model = new UsersIndexViewModel
        {
            Search = search,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Users = users
        };

        return View(model);
    }

    [HttpGet]
    [PermissionAuthorize(PermissionCodes.UsersCreate)]
    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        var model = new UserCreateViewModel
        {
            Roles = await GetRolesSafeAsync(cancellationToken),
            AktifMi = true
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [PermissionAuthorize(PermissionCodes.UsersCreate)]
    public async Task<IActionResult> Create(
        UserCreateViewModel model,
        CancellationToken cancellationToken = default)
    {
        model.Roles = await GetRolesSafeAsync(cancellationToken);

        var validationMessage = ValidateCreateModel(model);
        if (validationMessage is not null)
        {
            TempData["ErrorMessage"] = validationMessage;
            return View(model);
        }

        var request = new UserCreateRequest
        {
            KullaniciAdi = model.KullaniciAdi.Trim(),
            AdSoyad = model.AdSoyad.Trim(),
            Eposta = NormalizeNullableText(model.Eposta),
            Telefon = NormalizeNullableText(model.Telefon),
            Sifre = model.Sifre,
            RolId = model.RolId,
            AktifMi = model.AktifMi
        };

        object? result;

        try
        {
            result = await _userApiService.CreateAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı oluşturulurken hata oluştu.");
            result = null;
        }

        if (result is null)
        {
            TempData["ErrorMessage"] = "Kullanıcı oluşturulamadı. Kullanıcı adı daha önce alınmış olabilir.";
            return View(model);
        }

        TempData["SuccessMessage"] = "Kullanıcı başarıyla oluşturuldu.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [PermissionAuthorize(PermissionCodes.UsersEdit)]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken = default)
    {
        var user = await GetUserSafeAsync(id, cancellationToken);

        if (user is null)
        {
            TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
            return RedirectToAction(nameof(Index));
        }

        var model = new UserEditViewModel
        {
            Id = user.Id,
            KullaniciAdi = user.KullaniciAdi,
            AdSoyad = user.AdSoyad,
            Eposta = user.Eposta,
            Telefon = user.Telefon,
            AktifMi = user.AktifMi,
            RolId = user.Roller.FirstOrDefault()?.Id ?? 0,
            Roles = await GetRolesSafeAsync(cancellationToken)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [PermissionAuthorize(PermissionCodes.UsersEdit)]
    public async Task<IActionResult> Edit(
        int id,
        UserEditViewModel model,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0 || id != model.Id)
        {
            TempData["ErrorMessage"] = "Geçersiz kullanıcı bilgisi.";
            return RedirectToAction(nameof(Index));
        }

        model.Roles = await GetRolesSafeAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(model.AdSoyad))
        {
            TempData["ErrorMessage"] = "Ad soyad zorunludur.";
            return View(model);
        }

        if (!string.IsNullOrWhiteSpace(model.YeniSifre) && model.YeniSifre.Length < 6)
        {
            TempData["ErrorMessage"] = "Yeni şifre en az 6 karakter olmalıdır.";
            return View(model);
        }

        if (model.RolId <= 0)
        {
            TempData["ErrorMessage"] = "Rol seçilmelidir.";
            return View(model);
        }

        var currentUserId = GetCurrentUserId();

        if (currentUserId == model.Id && !model.AktifMi)
        {
            TempData["ErrorMessage"] = "Kendi hesabınızı pasif yapamazsınız.";
            return View(model);
        }

        var request = new UserUpdateRequest
        {
            Id = model.Id,
            AdSoyad = model.AdSoyad.Trim(),
            Eposta = NormalizeNullableText(model.Eposta),
            Telefon = NormalizeNullableText(model.Telefon),
            YeniSifre = NormalizeNullableText(model.YeniSifre),
            RolId = model.RolId,
            AktifMi = model.AktifMi
        };

        object? result;

        try
        {
            result = await _userApiService.UpdateAsync(id, request, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı güncellenirken hata oluştu. UserId: {UserId}", id);
            result = null;
        }

        if (result is null)
        {
            TempData["ErrorMessage"] = "Kullanıcı güncellenemedi.";
            return View(model);
        }

        TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<List<RoleResponse>> GetRolesSafeAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _userApiService.GetRolesAsync(cancellationToken)
                ?? new List<RoleResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Roller alınırken hata oluştu.");
            return new List<RoleResponse>();
        }
    }

    private async Task<UserDetailResponse?> GetUserSafeAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _userApiService.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı detayı alınırken hata oluştu. UserId: {UserId}", id);
            return null;
        }
    }

    private int GetCurrentUserId()
    {
        var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(claimValue, out var userId) ? userId : 0;
    }

    private static string? ValidateCreateModel(UserCreateViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.KullaniciAdi)) return "Kullanıcı adı zorunludur.";
        if (string.IsNullOrWhiteSpace(model.AdSoyad)) return "Ad soyad zorunludur.";
        if (string.IsNullOrWhiteSpace(model.Sifre) || model.Sifre.Length < 6) return "Şifre en az 6 karakter olmalıdır.";
        if (model.RolId <= 0) return "Rol seçilmelidir.";
        return null;
    }

    private static string? NormalizeNullableText(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
