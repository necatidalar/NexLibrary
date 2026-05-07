using NexLibrary.Contracts.Books;
using NexLibrary.Contracts.Common;

namespace NexLibrary.Application.Interfaces.Services;

public interface IBookService
{
    Task<ApiResponse<PagedResponse<BookListResponse>>> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<BookDetailResponse>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<BookDetailResponse>> CreateAsync(
        BookCreateRequest request,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<BookDetailResponse>> UpdateAsync(
        int id,
        BookUpdateRequest request,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}