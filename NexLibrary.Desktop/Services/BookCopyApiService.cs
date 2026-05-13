using NexLibrary.Contracts.BookCopies;

namespace NexLibrary.Desktop.Services;

public sealed class BookCopyApiService
{
    private readonly ApiClient _apiClient;

    public BookCopyApiService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<List<BookCopyStockSummaryResponse>> GetStockSummaryAsync(
        CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.GetAsync<List<BookCopyStockSummaryResponse>>(
            "api/book-copies/stock-summary",
            cancellationToken);

        return response is null || !response.BasariliMi || response.Veri is null
            ? new List<BookCopyStockSummaryResponse>()
            : response.Veri;
    }

    public async Task<List<BookCopyListResponse>> GetAvailableByBookIdAsync(
        int kitapId,
        CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.GetAsync<List<BookCopyListResponse>>(
            $"api/book-copies/book/{kitapId}/available",
            cancellationToken);

        return response is null || !response.BasariliMi || response.Veri is null
            ? new List<BookCopyListResponse>()
            : response.Veri;
    }

    public async Task<BookCopyListResponse?> CreateAsync(
        BookCopyCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.PostAsync<BookCopyListResponse>(
            "api/book-copies",
            request,
            cancellationToken);

        return response is null || !response.BasariliMi
            ? null
            : response.Veri;
    }
}