using NexLibrary.Contracts.Books;
using NexLibrary.Contracts.Common;

namespace NexLibrary.Desktop.Services;

public sealed class BookApiService
{
    private readonly ApiClient _apiClient;

    public BookApiService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<PagedResponse<BookListResponse>?> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var url = $"api/books?pageNumber={pageNumber}&pageSize={pageSize}";

        if (!string.IsNullOrWhiteSpace(search))
        {
            url += $"&search={Uri.EscapeDataString(search)}";
        }

        var response = await _apiClient.GetAsync<PagedResponse<BookListResponse>>(
            url,
            cancellationToken);

        if (response is null || !response.BasariliMi)
        {
            return null;
        }

        return response.Veri;
    }

    public async Task<BookDetailResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.GetAsync<BookDetailResponse>(
            $"api/books/{id}",
            cancellationToken);

        if (response is null || !response.BasariliMi)
        {
            return null;
        }

        return response.Veri;
    }

    public async Task<BookDetailResponse?> CreateAsync(
        BookCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.PostAsync<BookDetailResponse>(
            "api/books",
            request,
            cancellationToken);

        if (response is null || !response.BasariliMi)
        {
            return null;
        }

        return response.Veri;
    }

    public async Task<BookDetailResponse?> UpdateAsync(
        int id,
        BookUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.PutAsync<BookDetailResponse>(
            $"api/books/{id}",
            request,
            cancellationToken);

        if (response is null || !response.BasariliMi)
        {
            return null;
        }

        return response.Veri;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.DeleteAsync<bool>(
            $"api/books/{id}",
            cancellationToken);

        return response is not null && response.BasariliMi && response.Veri;
    }
}