using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Permissions;

namespace NexLibrary.Application.Interfaces.Services;

public interface IPermissionService
{
    Task<ApiResponse<UserPermissionResponse>> GetUserPermissionsAsync(
        int kullaniciId,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<RolePermissionMatrixResponse>> GetRolePermissionMatrixAsync(
        int rolId,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<RolePermissionMatrixResponse>> UpdateRolePermissionsAsync(
        int rolId,
        RolePermissionUpdateRequest request,
        CancellationToken cancellationToken = default);
}