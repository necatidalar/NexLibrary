using NexLibrary.Contracts.BookCopies;
using NexLibrary.Contracts.Common;

namespace NexLibrary.Application.Interfaces.Services;

public interface IBookCopyService
{
    Task<ApiResponse<List<BookCopyListResponse>>> GetByBookIdAsync(
        int kitapId,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<List<BookCopyListResponse>>> GetAvailableByBookIdAsync(
        int kitapId,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<List<BookCopyStockSummaryResponse>>> GetStockSummaryAsync(
        CancellationToken cancellationToken = default);

    Task<ApiResponse<BookCopyListResponse>> CreateAsync(
        BookCopyCreateRequest request,
        CancellationToken cancellationToken = default);
}