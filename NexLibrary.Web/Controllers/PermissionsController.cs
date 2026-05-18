using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.Permissions;
using NexLibrary.Contracts.Users;
using NexLibrary.Web.Security;
using NexLibrary.Web.Services;
using NexLibrary.Web.ViewModels.Permissions;

namespace NexLibrary.Web.Controllers;

public sealed class PermissionsController : Controller
{
    private readonly PermissionApiService _permissionApiService;
    private readonly UserApiService _userApiService;
    private readonly ILogger<PermissionsController> _logger;

    public PermissionsController(
        PermissionApiService permissionApiService,
        UserApiService userApiService,
        ILogger<PermissionsController> logger)
    {
        _permissionApiService = permissionApiService;
        _userApiService = userApiService;
        _logger = logger;
    }

    [PermissionAuthorize(PermissionCodes.PermissionsView)]
    public async Task<IActionResult> Index(
        int? rolId = null,
        CancellationToken cancellationToken = default)
    {
        var roles = await GetRolesSafeAsync(cancellationToken);

        if (roles.Count == 0)
        {
            TempData["ErrorMessage"] = "Rol listesi alınamadı.";
            return View(new PermissionsIndexViewModel());
        }

        var selectedRoleId = rolId.HasValue && rolId.Value > 0
            ? rolId.Value
            : roles.First().Id;

        var selectedRole = roles.FirstOrDefault(x => x.Id == selectedRoleId)
            ?? roles.First();

        var matrix = await GetRolePermissionMatrixSafeAsync(
            selectedRole.Id,
            cancellationToken);

        if (matrix is null)
        {
            TempData["ErrorMessage"] = "Yetki matrisi alınamadı.";

            return View(CreateEmptyViewModel(
                roles,
                selectedRole));
        }

        var model = CreateViewModel(
            roles,
            matrix);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [PermissionAuthorize(PermissionCodes.PermissionsManage)]
    public async Task<IActionResult> Update(
        PermissionsIndexViewModel model,
        CancellationToken cancellationToken = default)
    {
        if (model.SelectedRoleId <= 0)
        {
            TempData["ErrorMessage"] = "Rol seçilmelidir.";
            return RedirectToAction(nameof(Index));
        }

        var selectedPermissionIds = model.SelectedPermissionIds is null
            ? new List<int>()
            : model.SelectedPermissionIds
                .Where(x => x > 0)
                .Distinct()
                .ToList();

        var request = new RolePermissionUpdateRequest
        {
            RolId = model.SelectedRoleId,
            YetkiTanimiIds = selectedPermissionIds
        };

        var result = await UpdateRolePermissionsSafeAsync(
            model.SelectedRoleId,
            request,
            cancellationToken);

        if (result is null)
        {
            TempData["ErrorMessage"] = "Rol yetkileri güncellenemedi.";
            return RedirectToAction(nameof(Index), new { rolId = model.SelectedRoleId });
        }

        TempData["SuccessMessage"] = "Rol yetkileri başarıyla güncellendi. Değişikliklerin kullanıcıya yansıması için ilgili kullanıcı çıkış yapıp tekrar giriş yapmalıdır.";

        return RedirectToAction(nameof(Index), new { rolId = model.SelectedRoleId });
    }

    private async Task<List<RoleResponse>> GetRolesSafeAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            return await _userApiService.GetRolesAsync(cancellationToken)
                ?? new List<RoleResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Rol listesi alınırken hata oluştu.");
            return new List<RoleResponse>();
        }
    }

    private async Task<RolePermissionMatrixResponse?> GetRolePermissionMatrixSafeAsync(
        int roleId,
        CancellationToken cancellationToken)
    {
        try
        {
            return await _permissionApiService.GetRolePermissionMatrixAsync(
                roleId,
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Rol yetki matrisi alınırken hata oluştu. RolId: {RolId}",
                roleId);

            return null;
        }
    }

    private async Task<object?> UpdateRolePermissionsSafeAsync(
        int roleId,
        RolePermissionUpdateRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            return await _permissionApiService.UpdateRolePermissionsAsync(
                roleId,
                request,
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Rol yetkileri güncellenirken hata oluştu. RolId: {RolId}",
                roleId);

            return null;
        }
    }

    private static PermissionsIndexViewModel CreateEmptyViewModel(
        IReadOnlyCollection<RoleResponse> roles,
        RoleResponse selectedRole)
    {
        return new PermissionsIndexViewModel
        {
            SelectedRoleId = selectedRole.Id,
            SelectedRoleName = selectedRole.RolAdi,
            SelectedRoleCode = selectedRole.RolKodu,
            Roles = roles.Select(x => new PermissionRoleOptionViewModel
            {
                RolId = x.Id,
                RolKodu = x.RolKodu,
                RolAdi = x.RolAdi
            }).ToList(),
            Groups = new List<PermissionGroupViewModel>(),
            SelectedPermissionIds = new List<int>()
        };
    }

    private static PermissionsIndexViewModel CreateViewModel(
        IReadOnlyCollection<RoleResponse> roles,
        RolePermissionMatrixResponse matrix)
    {
        var groups = matrix.Yetkiler
            .OrderBy(x => x.SiraNo)
            .GroupBy(x => x.ModulKodu)
            .Select(x => new PermissionGroupViewModel
            {
                ModulKodu = x.Key,
                Items = x
                    .OrderBy(y => y.SiraNo)
                    .ToList()
            })
            .ToList();

        return new PermissionsIndexViewModel
        {
            SelectedRoleId = matrix.RolId,
            SelectedRoleCode = matrix.RolKodu,
            SelectedRoleName = matrix.RolAdi,
            Roles = roles.Select(x => new PermissionRoleOptionViewModel
            {
                RolId = x.Id,
                RolKodu = x.RolKodu,
                RolAdi = x.RolAdi
            }).ToList(),
            Groups = groups,
            SelectedPermissionIds = matrix.Yetkiler
                .Where(x => x.SeciliMi)
                .Select(x => x.YetkiTanimiId)
                .ToList()
        };
    }
}