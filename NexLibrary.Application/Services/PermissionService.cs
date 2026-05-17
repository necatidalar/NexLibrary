using Microsoft.EntityFrameworkCore;
using NexLibrary.Application.Interfaces.Repositories;
using NexLibrary.Application.Interfaces.Services;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Permissions;
using NexLibrary.Domain.Entities;

namespace NexLibrary.Application.Services;

public sealed class PermissionService : IPermissionService
{
    private readonly IUnitOfWork _unitOfWork;

    public PermissionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<UserPermissionResponse>> GetUserPermissionsAsync(
        int kullaniciId,
        CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Kullanicilar
            .Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == kullaniciId, cancellationToken);

        if (user is null)
        {
            return ApiResponse<UserPermissionResponse>.Fail("Kullanıcı bulunamadı.");
        }

        var permissions = await _unitOfWork.KullaniciRolleri
            .Query()
            .AsNoTracking()
            .Where(x =>
                x.KullaniciId == kullaniciId &&
                x.AktifMi &&
                x.Rol.AktifMi)
            .SelectMany(x => x.Rol.RolYetkileri
                .Where(y =>
                    y.AktifMi &&
                    y.YetkiTanimi.AktifMi)
                .Select(y => y.YetkiTanimi.YetkiKodu))
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync(cancellationToken);

        var response = new UserPermissionResponse
        {
            KullaniciId = user.Id,
            KullaniciAdi = user.KullaniciAdi,
            Yetkiler = permissions
        };

        return ApiResponse<UserPermissionResponse>.Success(response);
    }

    public async Task<ApiResponse<RolePermissionMatrixResponse>> GetRolePermissionMatrixAsync(
        int rolId,
        CancellationToken cancellationToken = default)
    {
        var role = await _unitOfWork.Roller
            .Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == rolId, cancellationToken);

        if (role is null)
        {
            return ApiResponse<RolePermissionMatrixResponse>.Fail("Rol bulunamadı.");
        }

        var selectedPermissionIds = await _unitOfWork.RolYetkileri
            .Query()
            .AsNoTracking()
            .Where(x => x.RolId == rolId && x.AktifMi)
            .Select(x => x.YetkiTanimiId)
            .ToListAsync(cancellationToken);

        var selectedSet = selectedPermissionIds.ToHashSet();

        var permissions = await _unitOfWork.YetkiTanimlari
            .Query()
            .AsNoTracking()
            .Where(x => x.AktifMi)
            .OrderBy(x => x.SiraNo)
            .Select(x => new RolePermissionItemResponse
            {
                YetkiTanimiId = x.Id,
                ModulKodu = x.ModulKodu,
                YetkiKodu = x.YetkiKodu,
                YetkiAdi = x.YetkiAdi,
                Aciklama = x.Aciklama,
                MenuYetkisiMi = x.MenuYetkisiMi,
                SiraNo = x.SiraNo,
                SeciliMi = selectedSet.Contains(x.Id)
            })
            .ToListAsync(cancellationToken);

        var response = new RolePermissionMatrixResponse
        {
            RolId = role.Id,
            RolKodu = role.RolKodu,
            RolAdi = role.RolAdi,
            Yetkiler = permissions
        };

        return ApiResponse<RolePermissionMatrixResponse>.Success(response);
    }

    public async Task<ApiResponse<RolePermissionMatrixResponse>> UpdateRolePermissionsAsync(
        int rolId,
        RolePermissionUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (rolId != request.RolId)
        {
            return ApiResponse<RolePermissionMatrixResponse>.Fail("Geçersiz rol bilgisi.");
        }

        var roleExists = await _unitOfWork.Roller
            .Query()
            .AnyAsync(x => x.Id == rolId, cancellationToken);

        if (!roleExists)
        {
            return ApiResponse<RolePermissionMatrixResponse>.Fail("Rol bulunamadı.");
        }

        var validPermissionIds = await _unitOfWork.YetkiTanimlari
            .Query()
            .Where(x => x.AktifMi)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        var validSet = validPermissionIds.ToHashSet();

        var requestedSet = request.YetkiTanimiIds
            .Where(validSet.Contains)
            .Distinct()
            .ToHashSet();

        var existingRolePermissions = await _unitOfWork.RolYetkileri
            .Query()
            .Where(x => x.RolId == rolId)
            .ToListAsync(cancellationToken);

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            foreach (var existing in existingRolePermissions)
            {
                var shouldBeActive = requestedSet.Contains(existing.YetkiTanimiId);

                existing.AktifMi = shouldBeActive;
                existing.GuncellemeTarihi = DateTime.UtcNow;

                _unitOfWork.RolYetkileri.Update(existing);
            }

            var existingPermissionIdSet = existingRolePermissions
                .Select(x => x.YetkiTanimiId)
                .ToHashSet();

            var permissionsToAdd = requestedSet
                .Where(x => !existingPermissionIdSet.Contains(x))
                .ToList();

            foreach (var permissionId in permissionsToAdd)
            {
                var rolePermission = new RolYetki
                {
                    RolId = rolId,
                    YetkiTanimiId = permissionId,
                    AktifMi = true,
                    OlusturmaTarihi = DateTime.UtcNow
                };

                await _unitOfWork.RolYetkileri.AddAsync(
                    rolePermission,
                    cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }, cancellationToken);

        return await GetRolePermissionMatrixAsync(rolId, cancellationToken);
    }
}