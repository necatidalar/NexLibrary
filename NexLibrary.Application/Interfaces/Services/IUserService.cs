using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Users;

namespace NexLibrary.Application.Interfaces.Services;

public interface IUserService
{
    Task<ApiResponse<PagedResponse<UserListResponse>>> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<UserDetailResponse>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<UserDetailResponse>> CreateAsync(
        UserCreateRequest request,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<UserDetailResponse>> UpdateAsync(
        int id,
        UserUpdateRequest request,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<List<RoleResponse>>> GetRolesAsync(
        CancellationToken cancellationToken = default);
}