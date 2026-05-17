using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Users;
using NexLibrary.Web.Services;
using NexLibrary.Web.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;

namespace NexLibrary.Web.Controllers;

[Authorize(Roles = "ADMIN")]
public sealed class UsersController : Controller
{
    private readonly UserApiService _userApiService;

    public UsersController(UserApiService userApiService)
    {
        _userApiService = userApiService;
    }

    public async Task<IActionResult> Index(
        string? search = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 20 : pageSize;

        var users = await _userApiService.GetPagedAsync(
            pageNumber,
            pageSize,
            search,
            cancellationToken);

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
    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        var roles = await _userApiService.GetRolesAsync(cancellationToken);

        var model = new UserCreateViewModel
        {
            Roles = roles,
            AktifMi = true
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        UserCreateViewModel model,
        CancellationToken cancellationToken = default)
    {
        model.Roles = await _userApiService.GetRolesAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(model.KullaniciAdi))
        {
            TempData["ErrorMessage"] = "Kullanıcı adı zorunludur.";
            return View(model);
        }

        if (string.IsNullOrWhiteSpace(model.AdSoyad))
        {
            TempData["ErrorMessage"] = "Ad soyad zorunludur.";
            return View(model);
        }

        if (string.IsNullOrWhiteSpace(model.Sifre) || model.Sifre.Length < 6)
        {
            TempData["ErrorMessage"] = "Şifre en az 6 karakter olmalıdır.";
            return View(model);
        }

        if (model.RolId <= 0)
        {
            TempData["ErrorMessage"] = "Rol seçilmelidir.";
            return View(model);
        }

        var request = new UserCreateRequest
        {
            KullaniciAdi = model.KullaniciAdi.Trim(),
            AdSoyad = model.AdSoyad.Trim(),
            Eposta = string.IsNullOrWhiteSpace(model.Eposta) ? null : model.Eposta.Trim(),
            Telefon = string.IsNullOrWhiteSpace(model.Telefon) ? null : model.Telefon.Trim(),
            Sifre = model.Sifre,
            RolId = model.RolId,
            AktifMi = model.AktifMi
        };

        var result = await _userApiService.CreateAsync(
            request,
            cancellationToken);

        if (result is null)
        {
            TempData["ErrorMessage"] = "Kullanıcı oluşturulamadı. Kullanıcı adı daha önce alınmış olabilir.";
            return View(model);
        }

        TempData["SuccessMessage"] = "Kullanıcı başarıyla oluşturuldu.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(
        int id,
        CancellationToken cancellationToken = default)
    {
        var user = await _userApiService.GetByIdAsync(id, cancellationToken);

        if (user is null)
        {
            TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
            return RedirectToAction(nameof(Index));
        }

        var roles = await _userApiService.GetRolesAsync(cancellationToken);

        var model = new UserEditViewModel
        {
            Id = user.Id,
            KullaniciAdi = user.KullaniciAdi,
            AdSoyad = user.AdSoyad,
            Eposta = user.Eposta,
            Telefon = user.Telefon,
            AktifMi = user.AktifMi,
            RolId = user.Roller.FirstOrDefault()?.Id ?? 0,
            Roles = roles
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
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

        model.Roles = await _userApiService.GetRolesAsync(cancellationToken);

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

        var request = new UserUpdateRequest
        {
            Id = model.Id,
            AdSoyad = model.AdSoyad.Trim(),
            Eposta = string.IsNullOrWhiteSpace(model.Eposta) ? null : model.Eposta.Trim(),
            Telefon = string.IsNullOrWhiteSpace(model.Telefon) ? null : model.Telefon.Trim(),
            YeniSifre = string.IsNullOrWhiteSpace(model.YeniSifre) ? null : model.YeniSifre,
            RolId = model.RolId,
            AktifMi = model.AktifMi
        };

        var result = await _userApiService.UpdateAsync(
            id,
            request,
            cancellationToken);

        if (result is null)
        {
            TempData["ErrorMessage"] = "Kullanıcı güncellenemedi.";
            return View(model);
        }

        TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";

        return RedirectToAction(nameof(Index));
    }
}