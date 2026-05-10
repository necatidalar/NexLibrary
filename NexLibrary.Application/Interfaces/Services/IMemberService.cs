using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Members;

namespace NexLibrary.Application.Interfaces.Services;

public interface IMemberService
{
    Task<ApiResponse<PagedResponse<MemberListResponse>>> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<MemberDetailResponse>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<MemberDetailResponse>> CreateAsync(
        MemberCreateRequest request,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<MemberDetailResponse>> UpdateAsync(
        int id,
        MemberUpdateRequest request,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}