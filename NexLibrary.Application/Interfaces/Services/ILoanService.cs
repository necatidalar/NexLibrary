using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Loans;

namespace NexLibrary.Application.Interfaces.Services;

public interface ILoanService
{
    Task<ApiResponse<PagedResponse<LoanListResponse>>> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<LoanDetailResponse>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<LoanDetailResponse>> CreateAsync(
        LoanCreateRequest request,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<bool>> CancelAsync(
        int id,
        CancellationToken cancellationToken = default);
}